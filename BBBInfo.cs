using System;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace BigBeautifulBot
{
    public class BBBInfo
    {
        public BBBInfo()
        {
            var command = new SQLiteCommand("SELECT * FROM BBB;", Program.db);
            var reader = command.ExecuteReader();
            reader.Read();
            BBBID = (long)reader[0];
            Weight = (decimal)reader[1];
        }

        public long BBBID { get; }
        public decimal Weight { get; set; }

        internal async Task Save()
        {
            var command = new SQLiteCommand("UPDATE BBB SET Weight = ?;", Program.db);
            command.Parameters.Add(new SQLiteParameter("?", Weight));
            await command.ExecuteNonQueryAsync();
        }
    }
}