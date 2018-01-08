using System;
using MySql.Data.MySqlClient;
using SimpleServerLib;
using SimpleServerTester;

namespace SimpleServerUtil.TestHandlers
{
    public class TestHandler : IRequestHandler2
    {
        /*private BaseDataStore _dataStore;
        private int count;

        public TestHandler()
        {
            _dataStore = new BaseDataStore();
            count = 0;
        }

        public int Handle(int hello)
        {
            Console.WriteLine(count);
            count++;
            return count;
            /*using (var conn = _dataStore.Connect("Game", "", null))
            {
                MySqlCommand command = conn.CreateCommand();
                command.CommandText = $"INSERT INTO game.game (CardId) VALUES ({hello});";
                command.ExecuteNonQuery();
            }
            return hello;
        }*/
    }
}
