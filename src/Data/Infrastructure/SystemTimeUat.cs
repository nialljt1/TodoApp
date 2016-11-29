using System;

namespace Data.Infrastructure
{
    public class SystemTimeUat : SystemTime
    {
        public override DateTime Now
        {
            get
            {
                return OverridenTime.HasValue ? OverridenTime.Value : base.Now;
            }
        }

        public DateTime? OverridenTime { get; set; }
    }
}
