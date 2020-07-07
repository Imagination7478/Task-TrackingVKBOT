using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Task_TrackingVKBOT.JSON_s;
using VkNet.Abstractions;
using VkNet.Model.RequestParams;

namespace Task_TrackingVKBOT
{
    public class BotLogic
    {
        enum UserId
        {
            userId
        }
        private readonly IVkApi _Vkapi;
        private readonly SqlConnectionStringBuilder builder;

        public BotLogic(IVkApi Vkapi, string DataSource, string UserID, string Password, string InitialCatalog)
        {
            _Vkapi = Vkapi;
            builder = new SqlConnectionStringBuilder();
            builder.DataSource = DataSource;
            builder.UserID = UserID;
            builder.Password = Password;
            builder.InitialCatalog = InitialCatalog;
        }

        public void AddIntoDatabase(long vkid, long groupId)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                long[] usersarray = new long[1];
                usersarray[0] = vkid;
                var user = _Vkapi.Users.Get(usersarray).FirstOrDefault();

                try
                {
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append(" INSERT INTO dbo.Users VALUES('" + user.FirstName + "', " + "'" + user.LastName + "', " + vkid + ", " + 0 + "); ");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read()) { }
                            reader.Close();
                            connection.Close();
                            _Vkapi.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                            {
                                RandomId = new DateTime().Millisecond,
                                PeerId = vkid,
                                Message = user.FirstName + " " + user.LastName + ", вы были добавлены в базу данных. Поставьте в профиль в Jira те же Имя и Фамлию, что ВКонтакте."
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    _Vkapi.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                    {
                        RandomId = new DateTime().Millisecond,
                        PeerId = vkid,
                        Message = ex.Message
                    });
                }
            }
        }
        // Метод переназначает исполнителя и высылает новому исполнителю сообщение через вк
        public void Change(string from, string to, WebhookJSON updates)
        {
            try
            {
                var idFrom = UserInsideDatabase(from);
                if (idFrom != "0")
                {
                    if (NotificationsIsEnabled(from))
                    {
                        _Vkapi.Messages.Send(new MessagesSendParams
                        {
                            RandomId = new DateTime().Millisecond,
                            PeerId = Convert.ToUInt32(idFrom),
                            Message = from + ", в Jira проекте " + updates.issue.fields.project.key + " вы были сняты с задачи " + updates.issue.key + ". " +
                            "Если вы не хотите получать такие уведомления, напишите 'Отписаться'."
                        });
                    }
                }
            }
            catch (Exception ex) { }
            try
            {
                var idTo = UserInsideDatabase(to);

                if (idTo != "0")
                {
                    if (NotificationsIsEnabled(to))
                    {
                        _Vkapi.Messages.Send(new MessagesSendParams
                        {
                            RandomId = new DateTime().Millisecond,
                            PeerId = Convert.ToUInt32(idTo),
                            Message = to + ", в Jira проекте " + updates.issue.fields.project.key + " вы были назначены на задачу " + updates.issue.key + ". " +
                            " Если вы не хотите получать такие уведомления, напишите 'Отписаться'."
                        });
                    }
                }
            }
            catch (Exception ex) { }
        }

        // flag - флаг, по которому определяем, предложить подписаться, подписать или отписать
        public void Subscribe(long? userId, int flag)
        {
            switch (flag)
            {
                case 0:
                    _Vkapi.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                    {
                        RandomId = new DateTime().Millisecond,
                        PeerId = userId,
                        Message = "Чтобы подписаться на уведомления с Jira доски, напишите 'Подписаться', " +
                        "в дальнейшем, при желании можно отписаться, написав 'Отписаться'."
                    });
                    break;
                case 1:
                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        if (!NotificationsIsEnabled(userId))
                        {
                            connection.Open();
                            StringBuilder sb = new StringBuilder();
                            sb.Append("UPDATE dbo.Users SET notificationsIsEnabled = 1 WHERE vkid = " + userId + "; ");
                            String sql = sb.ToString();

                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read()) { }

                                    reader.Close();
                                    connection.Close();
                                }
                            }
                            _Vkapi.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                            {
                                RandomId = new DateTime().Millisecond,
                                PeerId = userId,
                                Message = "Вы успешно подписались на уведомления."
                            });
                        }
                        else
                        {
                            _Vkapi.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                            {
                                RandomId = new DateTime().Millisecond,
                                PeerId = userId,
                                Message = "Вы уже подписаны на уведомления. Вы можете отписаться командой 'Отписаться'."
                            });
                        }
                    }
                    break;
                case 2:
                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        if (NotificationsIsEnabled(userId))
                        {
                            connection.Open();
                            StringBuilder sb = new StringBuilder();
                            sb.Append("UPDATE dbo.Users SET notificationsIsEnabled = 0 WHERE vkid = " + userId + "; ");
                            String sql = sb.ToString();

                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read()) { }

                                    reader.Close();
                                    connection.Close();

                                }
                            }
                            _Vkapi.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                            {
                                RandomId = new DateTime().Millisecond,
                                PeerId = userId,
                                Message = "Вы успешно отписались от уведомлений."
                            });
                        }
                        else
                        {
                            _Vkapi.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                            {
                                RandomId = new DateTime().Millisecond,
                                PeerId = userId,
                                Message = "Вы не подписаны на уведомления."
                            });
                        }
                    }
                    break;
            }
        }

        public bool UserInsideDatabase(long? fromId)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT vkid FROM dbo.Users WHERE vkid = " + fromId + ";");
                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                reader[0].ToString();
                                reader.Close();
                                connection.Close();
                                return true;
                            }
                            catch
                            {
                                reader.Close();
                                connection.Close();
                                return false;
                            }
                        }
                    }
                }
                return false;
            }
        }
        public string UserInsideDatabase(string user)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                string[] nameSurname = new string[2];
                try
                {
                    nameSurname = user.Split();
                }
                catch (Exception ex)
                {
                    return "0";
                }
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT vkid FROM dbo.Users WHERE name = '" + nameSurname[0] + "' AND surname = '" + nameSurname[1] + "';");
                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var tmp = reader[0].ToString();
                                reader.Close();
                                connection.Close();
                                return tmp;
                            }
                            catch
                            {
                                reader.Close();
                                connection.Close();
                                return "0";
                            }
                        }
                    }
                }
                return "0";
            }
        }
        public bool NotificationsIsEnabled(long? fromId)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT notificationsIsEnabled FROM dbo.Users WHERE vkid = " + fromId + ";");
                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader[0].ToString() == "True")
                            {
                                reader.Close();
                                connection.Close();
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                return false;
            }
        }
        public bool NotificationsIsEnabled(string user)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                var nameSurname = user.Split();
                connection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT notificationsIsEnabled FROM dbo.Users WHERE name = '" + nameSurname[0] + "' AND surname = '" + nameSurname[1] + "';");
                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader[0].ToString() == "True")
                            {
                                reader.Close();
                                connection.Close();
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
                return false;
            }
        }
    }
}
