﻿using System;
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

        private IEnumerable<string> GetActivities()
        {
            var command = new SQLiteCommand("SELECT Name FROM Games;", db);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return (string)reader["Name"];
            }
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