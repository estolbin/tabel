namespace web_tabel.Domain
{
    public class ConfirmedPeriod
    {
        public Guid PeriodId { get; set; }
        public virtual TimeShiftPeriod Period { get; set; }

        public Guid DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public bool FirstHalfIsConfirmed { get; set; }
        public bool SecondHalfIsConfirmed { get; set; }
    }
}
