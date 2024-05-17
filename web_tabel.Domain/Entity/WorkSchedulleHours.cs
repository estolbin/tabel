using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace web_tabel.Domain
{
    public class WorkSchedulleHours 
    {
        private string? _typeOfWorkingTimeName;
        public virtual int DayNumber { get; set; }
        public virtual float Hours { get; set; }
        public virtual TypeOfWorkingTime TypeOfWorkingTime { get; set; }
        public virtual Guid WorkScheduleId { get; set; }
        public virtual string TypeOfWorkingTimeName
        {
            get { return _typeOfWorkingTimeName ?? TypeOfWorkingTime.Name; }
            set { _typeOfWorkingTimeName = TypeOfWorkingTime.Name ?? value; }
        }
    }
}
