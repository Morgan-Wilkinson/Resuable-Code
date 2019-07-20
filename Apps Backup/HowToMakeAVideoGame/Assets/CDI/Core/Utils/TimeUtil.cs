using System;

namespace cdi
{
    /// <summary>
    /// 
    /// </summary>
    public class TimeUtil
    {
        /// <summary>
        /// 
        /// </summary>
        public static long NowMilliseconds
        {
            get { return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string NowText
        {
            get
            {
                var datetime = DateTime.Now;
                return datetime.ToString("hh:mm:ss.FFF");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static double SecondsFrom(DateTime date)
        {
            return (DateTime.Now - date).TotalSeconds;
        }
    }
}