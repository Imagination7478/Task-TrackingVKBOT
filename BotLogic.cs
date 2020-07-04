using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using VkNet.Abstractions;

namespace Task_TrackingVKBOT
{
    public class BotLogic
    {
        private readonly MySqlConnection connection;
        public BotLogic() { }
        public BotLogic(string databaseAddr)
        {
            connection = new MySqlConnection(databaseAddr);
        }

        public void AddIntoDatabase()
        {

        }
        // Метод переназначает исполнителя и высылает новому исполнителю сообщение через вк
        public void Change(string from, string to)
        {
            // TODO
        }

        // flag - флаг, по которому определяем, предложить подписаться, подписать или отписать
        public void Subscribe(IVkApi _Vkapi, long? userId, long groupId, int flag)
        {
            switch (flag)
            {
                case 0:
                    _Vkapi.Messages.Send(new VkNet.Model.RequestParams.MessagesSendParams
                    {
                        UserId = userId,
                        RandomId = new DateTime().Millisecond,
                        PeerId = -groupId,
                        Message = "Чтобы подписаться на уведомления с Jira доски, напишите 'Подписаться', "+
                        "в дальнейшем, при желании можно отписаться, написав 'Отписаться'."
                    });
                    break;
                case 1:
                    connection.Open();
                    string Subscribe = "UPDATE dbo.Table SET notificationsIsEnabled = 1 WHERE vkid = " + userId + "; ";
                    MySqlCommand commandSub = new MySqlCommand(Subscribe, connection);
                    MySqlDataReader readerSub = commandSub.ExecuteReader();
                    readerSub.Close();
                    connection.Close();
                    break;
                case 2:
                    connection.Open();
                    string UnSubscribe = "UPDATE dbo.Table SET notificationsIsEnabled = 0 WHERE vkid = " + userId + "; ";
                    MySqlCommand commandUnSub = new MySqlCommand(UnSubscribe, connection);
                    MySqlDataReader readerUnSub = commandUnSub.ExecuteReader();
                    readerUnSub.Close();
                    connection.Close();
                    break;
            }
        }

        public bool UserInsideDatabase(long? fromId)
        {
            connection.Open();
            string FindUser = "SELECT vkid FROM [dbo].[Users] WHERE vkid = " + fromId + ";"; 
            // id, name, surname, vkid, notificationsIsEnabled - столбцы в бд

            MySqlCommand command = new MySqlCommand(FindUser, connection);
            MySqlDataReader reader = command.ExecuteReader();
            if (reader.ToString() != null)
            {
                reader.Close();
                connection.Close();
                return true;
            }
            else
            {
                reader.Close();
                connection.Close();
                return false;
            }
        }

    }
}
