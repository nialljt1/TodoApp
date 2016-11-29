using System;

namespace Data.Infrastructure
{
    public class SystemTime
    {
        public virtual DateTime Today
        {
            get
            {
                return DateTime.Today;
            }
        }

        public virtual DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }

        public virtual DateTime UtcNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }

        public virtual DateTimeOffset OffsetNow
        {
            get
            {
                return DateTimeOffset.Now;
            }
        }
    }
}
