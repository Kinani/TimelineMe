using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimelineMe.Common
{
    public static class TLMESettings
    {
        public static DateTimeOffset ScheduledDueTime { get; set; } = DateTimeOffset.UtcNow.AddSeconds(5);
        //public static DateTimeOffset ScheduledDueTime { get; set; } = DateTimeOffset.Parse("");
        public static bool EnableOxford { get; set; } = true;
        public static int DurationInSecForEachItem { get; set; } = 2;
        public static bool UseExternalCamApp { get; set; } = true;
    }
}
