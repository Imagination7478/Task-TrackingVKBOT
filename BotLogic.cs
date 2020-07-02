using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using VkNet.Abstractions;

namespace Task_TrackingVKBOT
{
    public class BotLogic
    {
        MySqlConnection connection;
        public BotLogic(string databaseAddr)
        {
            connection = new MySqlConnection(databaseAddr);
        }

        // Метод переназначает исполнителя и высылает новому исполнителю сообщение через вк
        public void Change(string from, string to)
        {
            // TODO
        }
        public void Subscribe()
        {

        }

        public bool UserInsideDatabase(long? fromId)
        {
            connection.Open();
            string FindUser = "SELECT fromId WHERE vkid = " + fromId + ";";

            // TODO: Сделать выполнение запроса
        }

    }
}
