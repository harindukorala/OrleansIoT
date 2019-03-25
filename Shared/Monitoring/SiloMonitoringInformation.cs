using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Monitoring
{
    [Serializable]
    public class SiloMonitoringInformation
    {
        public string timestamp;
        public string cpuUsage;
        public string memoryUsage;
        public string sentMessages;
        public string recievedMessages;
    }
}
