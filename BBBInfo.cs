using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace BigBeautifulBot
{
    public class BBBInfo
    {
        public static SQLiteConnection db;

        public BBBInfo()
        {
            //Connect to database
            db = db ?? new SQLiteConnection("Data Source=bbb.db;Version=3;").OpenAndReturn();

            //Get values from database
            var command = new SQLiteCommand("SELECT * FROM BBB;", db);
            var reader = command.ExecuteReader();
            reader.Read();
            BBBID = (long)reader[0];
            Weight = (decimal)reader[1];

            Activities = GetActivities().ToArray();
        }

        private IEnumerable<string> GetActivities()
        {
            var command = new SQLiteCommand("SELECT Name FROM Games;", db);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return (string)reader[0];
            }
        }

        public string[] Activities { get; }
        public long BBBID { get; }
        public decimal Weight { get; set; }

        internal async Task Save()
        {
            var command = new SQLiteCommand("UPDATE BBB SET Weight = ?;", db);
            command.Parameters.Add(new SQLiteParameter("?", Weight));
            await command.ExecuteNonQueryAsync();
        }
    }
}