using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace web_tabel.Domain
{
    public class WorkSchedulleHours 
    {
        private string _typeOfWorkingTypeName;
        public virtual int DayNumber { get; set; }
        public virtual float Hours { get; set; }
        public virtual TypeOfWorkingTime TypeOfWorkingTime { get; set; }
        public virtual Guid WorkScheduleId { get; set; }
        [JsonIgnore]
        public virtual string TypeOfWorkingTimeName
        {
            get { return _typeOfWorkingTypeName ?? TypeOfWorkingTime.Name; }
            set { _typeOfWorkingTypeName = value;}
        }
    }
}
