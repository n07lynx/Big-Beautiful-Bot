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

        private BBBSettings _Config;

        public decimal MaxAppetite => Weight / _Config.WeightAppetiteRatio;
        public decimal WellFormedOverfeedLimit => -Math.Abs(_Config.OverfeedLimit);
        public bool IsOverfed => Appetite < WellFormedOverfeedLimit;
        public const string TheCreator = "FairyMaku#0920";


        public BBBInfo(BBBSettings config)
        {
            _Config = config;

            //Connect to database
            db = db ?? new SQLiteConnection("Data Source=bbb.db;Version=3;").OpenAndReturn();

            //Get values from database
            var command = new SQLiteCommand("SELECT * FROM BBB;", db);
            var reader = command.ExecuteReader();
            reader.Read();
            BBBID = (long)reader[nameof(BBBID)];
            Weight = (decimal)reader[nameof(Weight)];
            Appetite = (decimal)reader[nameof(Appetite)];

            Activities = GetActivities().ToArray();
        }

        public IEnumerable<string> GetActivities()
        {
            var command = new SQLiteCommand("SELECT Name FROM Games;", db);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return (string)reader["Name"];
            }
        }

        public static async Task AddReminder(Input.UserIdentity user, string datetimeISO, string remText)
        {
            var command = new SQLiteCommand($"INSERT INTO Reminders(Activity, Dt, Id) VALUES (@Activity, @Datetime, '@User');", db);

            command.Parameters.AddWithValue("@User", user.UserName);
            command.Parameters.AddWithValue("@Datetime", DateTime.Parse(datetimeISO));
            command.Parameters.AddWithValue("@Activity", remText);

            Console.WriteLine($"New Reminder: ID: {user.UserName}, DATETIME: {datetimeISO}, Text: {remText}");


            await command.ExecuteNonQueryAsync();
        }

        public static IEnumerable<Input.ReminderType> GetReminders()
        {
            var reminders = GetRemindersUtil();
            if (reminders == null)
            {
                yield return new Input.ReminderType("", "0001-01-01 00:00:00", "");
            }

            foreach (var i in reminders)
            {
                yield return i;
            }
        }


        private static IEnumerable<Input.ReminderType> GetRemindersUtil()
        {
            var command = new SQLiteCommand("SELECT * FROM Reminders;", db);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var row = reader.GetValues();

                var user = row[2];
                var datetime = row[1] == "" ? "0001-01-01 00:00:00" : row[1];
                var remText = row[0];

                yield return new Input.ReminderType(user, datetime, remText);
            } 
        }

        public static async Task RemoveReminder(Input.ReminderType query)
        {
            var command = new SQLiteCommand("DELETE FROM Reminders where Activity = @Activity & Dt = @Datetime & Id = @User;", db);

            command.Parameters.AddWithValue("@User", query.User.UserName);
            command.Parameters.AddWithValue("@Datetime", query.Time);
            command.Parameters.AddWithValue("@Activity", query.RemText);

            Console.WriteLine($"Removed Reminder: ID: {query.User.UserName}, DATETIME: {query.Time}, Text: {query.RemText}");

            await command.ExecuteNonQueryAsync();
        }

        public string[] Activities { get; }
        public long BBBID { get; }
        public decimal Weight { get; set; }
        public decimal Appetite { get; set; }

        internal async Task Save()
        {
            var command = new SQLiteCommand("UPDATE BBB SET Weight = ?, Appetite = @A;", db);
            command.Parameters.Add(new SQLiteParameter("?", Weight));
            command.Parameters.Add(new SQLiteParameter("@A", Appetite));
            await command.ExecuteNonQueryAsync();
        }

        internal async Task<string> GetFatFact()
        {
            var command = new SQLiteCommand("SELECT Fact FROM FatFact;", db);
            var reader = await command.ExecuteReaderAsync();

            var factList = new List<string>();
            while (await reader.ReadAsync())
            {
                factList.Add((string)reader["Fact"]);
            }

            return Program.GetRandomElement(factList.ToArray());
        }
    }
}