using System;
using System.Collections.Generic;
using System.Text;
using BigBeautifulBot.Output;
using BigBeautifulBot.Properties;
using BigBeautifulBot.Input;
using System.Threading.Tasks;
using System.Timers;

namespace BigBeautifulBot
{
    class ReminderProcessor
    {

        public static IEnumerable<ReminderType> Reminders { get; set; }

        /// <summary>
        /// Wrapper for Respond() method from input 
        /// </summary>
        public static Action<string> sendMsg { get; private set; }

        public static IEnumerable<ReminderType> CheckReminder(IEnumerable<ReminderType> reminders)
        {
            if (reminders != null)
            {
                foreach (var i in reminders)
                {
                    if ($"{DateTime.UtcNow.ToShortDateString()} {DateTime.UtcNow.ToShortTimeString()}" == $"{i.Time.ToShortDateString()} {i.Time.ToShortTimeString()}")
                    {
                        yield return i;
                    }
                }
            }
        }

        public static void StartTimers()
        {
            var deletionTimer = new Timer(86400000);
            var timer = new Timer(60000);

            timer.Elapsed += (x, y) =>
            {
                //Console.WriteLine("Reminder 1min check!");
                foreach (var i in CheckReminder(BBBInfo.GetReminders()))
                {
                    sendMsg.Invoke($"{i.User.UserName} asked me to remind him of {i.RemText} on the {i.Time.ToShortDateString()} at {i.Time.ToShortTimeString()}");
                    BBBInfo.RemoveReminder(i);
                }
            };
            timer.Enabled = true;

            deletionTimer.Elapsed += (x, y) =>
            {
                foreach (var i in BBBInfo.GetReminders())
                {
                    if (DateTime.UtcNow > i.Time)
                    {
                        BBBInfo.RemoveReminder(i);
                    }
                }
            };

        }


        public async Task Process(CommandInput message)
        {
            
            try
            {
                string msg = message.Text.Substring(10, message.Text.Length - 10);
                var date = new DateTime(int.Parse(msg.Substring(6, 4)), int.Parse(msg.Substring(3, 2)), int.Parse(msg.Substring(0, 2)));

                TimeSpan time;
                string timeStr = "", dateStr = "";
                int dayCounter = 0;

                if (int.TryParse(msg.Substring(11, 2), out int hh) &&
                     int.TryParse(msg.Substring(14, 2), out int mm) &&
                     int.TryParse(msg.Substring(18, 2), out int tzhh) &&
                     int.TryParse(msg.Substring(21, 2), out int tzmm))
                {
                    //here used to be a comment but I fixed it, yay! (yes this code used to be even worse...)
                    var timeZoneOffset = new TimeSpan(tzhh, tzmm, 0);
                    var userTime = new TimeSpan(hh, mm, 0);
                    time = msg[17] == '-' ? userTime + timeZoneOffset : userTime - timeZoneOffset;
                    timeStr = time.ToString();

                    //23:11:00 = Less than 24h / 1.23:11:00 = between 1 and 10 days
                    if (timeStr.Length > 8)
                    {
                         if (time.ToString()[0] == '-')
                         {
                             time = new TimeSpan(24 + time.Hours, 60 + time.Minutes, 0);
                             timeStr = time.ToString().Substring(0, timeStr.Length - 4);
                             dayCounter--;
                         }
                         else
                         {
                             timeStr = timeStr.Substring(2, timeStr.Length - 3);
                             dayCounter++;
                         }
                    }
                }

                date = date.AddDays(dayCounter);
                string dateShortStr = date.ToShortDateString();

                for (int i = dateShortStr.Length - 1; i >= 0; i--)
                {
                    dateStr += dateShortStr[dateShortStr.Length - 1 - i] == '/' ? '-' : dateShortStr[dateShortStr.Length - 1 - i];
                }

                string datetimeISO = $"{dateStr} {timeStr.Substring(0, 5)}";
                string remText = msg.Substring(24, msg.Length - 24);

                await message.Respond($"I will remind you, {message.Author.UserName}, of {remText} on the {datetimeISO} UTC");

                sendMsg = async str => await message.Respond(str);
                await BBBInfo.AddReminder(message.Author, datetimeISO, remText);

            }

            catch
            {
                await message.Respond("If you want me to remind you of something, you gotta follow the template sweety ;p");
            }
        }
    }
}
