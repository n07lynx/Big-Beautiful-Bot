using System;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace BigBeautifulBot
{
    public class BBBInfo
    {
        public double Weight { get; internal set; }

        internal async Task Save()
        {
            var command = new SQLiteCommand("UPDATE BBB SET Weight = ?;");
            command.Parameters.Add(Weight);
            await command.ExecuteNonQueryAsync();
        }
    }
}