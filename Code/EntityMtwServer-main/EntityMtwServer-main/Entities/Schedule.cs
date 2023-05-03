using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityMtwServer.Entities
{
    public class Schedule
    {
        public long Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string MondayStartTime { get; set; } = "00:00";
        public string MondayEndTime { get; set; } = "00:00";
        public string TuesdayStartTime { get; set; } = "00:00";
        public string TuesdayEndTime { get; set; } = "00:00";
        public string WednesdayStartTime { get; set; } = "00:00";
        public string WednesdayEndTime { get; set; } = "00:00";
        public string ThursdayStartTime { get; set; } = "00:00";
        public string ThursdayEndTime { get; set; } = "00:00";
        public string FridayStartTime { get; set; } = "00:00";
        public string FridayEndTime { get; set; } = "00:00";
        public string SaturdayStartTime { get; set; } = "00:00";
        public string SaturdayEndTime { get; set; } = "00:00";
        public string SundayStartTime { get; set; } = "00:00";
        public string SundayEndTime { get; set; } = "00:00";

        public bool MondayRemote { get; set; }
        public bool TuesdayRemote { get; set; }
        public bool WednesdayRemote { get; set; }
        public bool ThursdayRemote { get; set; }
        public bool FridayRemote { get; set; }
        public bool SaturdayRemote { get; set; }
        public bool SundayRemote { get; set; }

        [JsonIgnore]
        [NotMapped]
        public TimeSpan WeekHours
        {
            get 
            {
                return (MondayEnd - MondayStart) +
                    (TuesdayEnd - TuesdayStart) +
                    (WednesdayEnd - WednesdayStart) +
                    (ThursdayEnd - ThursdayStart) +
                    (FridayEnd - FridayStart) +
                    (SaturdayEnd - SaturdayStart) +
                    (SundayEnd - SundayStart);
            }
        }
            


        [JsonIgnore]
        [NotMapped]
        public TimeSpan MondayStart
        {
            get
            {
                return TimeSpan.Parse(MondayStartTime);
            }
            set
            {
                MondayStartTime = value.ToString("HH:mm");
            }
        }

        [JsonIgnore]
        [NotMapped]
        public TimeSpan MondayEnd
        {
            get
            {
                return TimeSpan.Parse(MondayEndTime);
            }
            set
            {
                MondayEndTime = value.ToString("HH:mm");
            }
        }

        [JsonIgnore]
        [NotMapped]
        public TimeSpan TuesdayStart
        {
            get
            {
                return TimeSpan.Parse(TuesdayStartTime);
            }
            set
            {
                TuesdayStartTime = value.ToString("HH:mm");
            }
        }

        [JsonIgnore]
        [NotMapped]
        public TimeSpan TuesdayEnd
        {
            get
            {
                return TimeSpan.Parse(TuesdayEndTime);
            }
            set
            {
                TuesdayEndTime = value.ToString("HH:mm");
            }
        }

        [JsonIgnore]
        [NotMapped]
        public TimeSpan WednesdayStart
        {
            get
            {
                return TimeSpan.Parse(WednesdayStartTime);
            }
            set
            {
                WednesdayStartTime = value.ToString("HH:mm");
            }
        }

        [JsonIgnore]
        [NotMapped]
        public TimeSpan WednesdayEnd
        {
            get
            {
                return TimeSpan.Parse(WednesdayEndTime);
            }
            set
            {
                WednesdayEndTime = value.ToString("HH:mm");
            }




        }

        [JsonIgnore]
        [NotMapped]
        public TimeSpan ThursdayStart
        {
            get
            {
                return TimeSpan.Parse(ThursdayStartTime);
            }
            set
            {
                ThursdayStartTime = value.ToString("HH:mm");
            }
        }

        [JsonIgnore]
        [NotMapped]
        public TimeSpan ThursdayEnd
        {
            get
            {
                return TimeSpan.Parse(ThursdayEndTime);
            }
            set
            {
                ThursdayEndTime = value.ToString("HH:mm");
            }

        }

        [JsonIgnore]
        [NotMapped]
        public TimeSpan FridayStart
        {
            get
            {
                return TimeSpan.Parse(FridayStartTime);
            }
            set
            {
                FridayStartTime = value.ToString("HH:mm");
            }
        }

        [JsonIgnore]
        [NotMapped]
        public TimeSpan FridayEnd
        {
            get
            {
                return TimeSpan.Parse(FridayEndTime);
            }
            set
            {
                FridayEndTime = value.ToString("HH:mm");
            }
        }

        [JsonIgnore]
        [NotMapped]
        public TimeSpan SaturdayStart
        {
            get
            {
                return TimeSpan.Parse(SaturdayStartTime);
            }
            set
            {
                SaturdayStartTime = value.ToString("HH:mm");
            }
        }

        [JsonIgnore]
        [NotMapped]
        public TimeSpan SaturdayEnd
        {
            get
            {
                return TimeSpan.Parse(SaturdayEndTime);
            }
            set
            {
                SaturdayEndTime = value.ToString("HH:mm");
            }
        }

        [JsonIgnore]
        [NotMapped]
        public TimeSpan SundayStart
        {
            get
            {
                return TimeSpan.Parse(SundayStartTime);
            }
            set
            {
                SundayStartTime = value.ToString("HH:mm");
            }
        }

        [JsonIgnore]
        [NotMapped]
        public TimeSpan SundayEnd
        {
            get
            {
                return TimeSpan.Parse(SundayEndTime);
            }
            set
            {
                SundayEndTime = value.ToString("HH:mm");
            }
        }

        [JsonIgnore]
        [NotMapped]
        public bool Monday
        {
            get
            {
                return MondayStart < MondayEnd; 
            }
        }

        [JsonIgnore]
        [NotMapped]
        public bool Tuesday
        {
            get
            {
                return TuesdayStart < TuesdayEnd;
            }
        }

        [JsonIgnore]
        [NotMapped]
        public bool Wednesday
        {
            get
            {
                return WednesdayStart < WednesdayEnd;
            }
        }

        [JsonIgnore]
        [NotMapped]
        public bool Thursday
        {
            get
            {
                return ThursdayStart < ThursdayEnd;
            }
        }

        [JsonIgnore]
        [NotMapped]
        public bool Friday
        {
            get
            {
                return FridayStart < FridayEnd;
            }
        }

        [JsonIgnore]
        [NotMapped]
        public bool Saturday
        {
            get
            {
                return SaturdayStart < SaturdayEnd;
            }
        }

        [JsonIgnore]
        [NotMapped]
        public bool Sunday
        {
            get
            {
                return SundayStart < SundayEnd;
            }
        }

    }
}
