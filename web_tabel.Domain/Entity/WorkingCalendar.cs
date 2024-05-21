using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace web_tabel.Domain
{
    public class WorkingCalendar
    {
        public DateTime Date { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DayType Type { get; set; }
        [JsonIgnore]
        public int Year { get; set; }

    }

    public enum DayType
    {
        WorkDay = 1,
        Weekend = 2,
        Celebrate = 3
    }
}
