using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace BigBeautifulBot.Input
{
    public struct ReminderType
    {
        public UserIdentity User { get; }
        public DateTime Time { get; }
        public string RemText { get; }

        /// <summary>
        /// Returns Data saved in ReminderType object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"New Reminder: ID: {this.User.UserName}, DATETIME: {this.Time.ToString()}, Text: {this.RemText}";
        }

        public ReminderType(string user, string datetimeISO, string remText)
        {
            User = new UserIdentity(user);
            Time = DateTime.Parse(datetimeISO);
            RemText = remText;
        }
    }
}
