using System;
using System.Net.NetworkInformation;

namespace MachineRebootAutomation  {

    /* Class for network functions */
    public static class NetworkHandler {

        public static Boolean PingCheck(string MachineDomainAddress) {

            Ping pingTest = new Ping();
            PingReply pingReply = pingTest.Send(MachineDomainAddress);
            if (pingReply.Status == IPStatus.Success) {
                return true;
            }
            return false;
        }


    }
}
