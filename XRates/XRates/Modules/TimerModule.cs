using System;
using System.Threading;
using System.Web;
using XRates.BLL;

namespace XRates.Modules
{
    public class TimerModule : IHttpModule
    {
        static Timer timer;
        long interval = 30000;
        static object synclock = new object();
        static bool updated = false;

        public void Init(HttpApplication app)
        {
            timer = new Timer(new TimerCallback(UpdateRates), null, 0, interval);
        }

        private void UpdateRates(object obj)
        {
            lock (synclock)
            {
                DateTime dt = DateTime.Now;
                DateTime updateTime = Convert.ToDateTime(Properties.Settings.Default.UpdateTime);
                if (dt.Hour == updateTime.Hour && dt.Minute == updateTime.Minute && !updated)
                {
                    RateService.Instance.UpdateRates(dt);
                    updated = true;
                }
                else if (dt.Hour != updateTime.Hour && dt.Minute != updateTime.Minute)
                {
                    updated = false;
                }
            }
        }
        public void Dispose()
        { }
    }
}