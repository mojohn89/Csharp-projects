using System;
using System.Diagnostics;
using System.Threading;

namespace MachineRebootAutomation {

    /* Functions for using PSTools
     - Kill processes
     - Launch processes
     - Delete files */
    public class PSToolsHandler {

        private MailHandler mail;

        /* paths to pstools executables 
         - in our environment psexec doesn't work properly unless calling it directly */
        private string psexec = "psexec";
        private string pskill = @"path\to\pskill.exe";
        private string psshutdown = @"path\to\psshutdown.exe";
        private string pstoolsLogin;

        public PSToolsHandler(string username, string password, MailHandler mail) {
            this.pstoolsLogin = " -u " + username + " -p " + password + " ";
            this.mail = mail;
        }

        /*########################
         PUBLIC FUNCTIONS IN CLASS 
         ##########################*/
        
        public void DeleteFilesAndReboot(string MachineDomainAddress) {
            if (!MachineDomainAddress.ToUpper().Contains("machinetype_prefix")) {
                Console.WriteLine("Error in domain " + MachineDomainAddress);
                return;
            }
            KillProcess(MachineDomainAddress, "process1.exe");
            KillProcess(MachineDomainAddress, "PROCESS2.exe");
            KillProcess(MachineDomainAddress, "process3.exe");
            DeleteFiles(MachineDomainAddress);
            RestartMachine(MachineDomainAddress);
        }

        /* Kills set processes at the host and then reboots the machine */
        public void KillProcessesAndReboot(string MachineDomainAddress) {

            if (!MachineDomainAddress.ToUpper().Contains("machinetype_prefix")) {
                Console.WriteLine("Error in domain " + MachineDomainAddress);
                return;
            }
            // Sleep to give host time to finish
            KillProcess(MachineDomainAddress, "process1.exe");
            Thread.Sleep(2000);
            KillProcess(MachineDomainAddress, "PROCESS2.exe");
            Thread.Sleep(2000);
            KillProcess(MachineDomainAddress, "process3.exe");
            Thread.Sleep(2000);
            RestartMachine(MachineDomainAddress);
            Thread.Sleep(2000);
        }

        /* Kills a single process and restarts it via .bat file on host */
        public void KillProcessAndRestart(string MachineDomainAddress) {
        
            if (!MachineDomainAddress.ToUpper().Contains("machinetype_prefix")) {
                Console.WriteLine("Error in domain " + MachineDomainAddress);
                return;
            }

            KillProcess(MachineDomainAddress, "process1.exe");
            Thread.Sleep(4000);
            LaunchProcess(MachineDomainAddress);
            Thread.Sleep(2000);
        }


          
        /*########################
         PRIVATE FUNCTIONS IN CLASS 
         ##########################*/

        /* Launches cmd-line to delete all files in folder */
        private void DeleteFiles(string MachineDomainAddress) {
        
            string parameters = @" -d -accepteula \\" + MachineDomainAddress + this.pstoolsLogin + @" cmd /c (del /Q C:\Path\to\files\*.*) ";
            try {
                Process proc = System.Diagnostics.Process.Start(psexec, parameters);
                proc.WaitForExit(12000);
            }
            catch {
                Console.WriteLine("Files on " + MachineDomainAddress + " could not be deleted");
            }
        }


        /* Launches launchProcess.bat locally on Machine */
        private void LaunchProcess(string MachineDomainAddress) {

            string parameters = @" -d -accepteula \\" + MachineDomainAddress + this.pstoolsLogin + @" C:\path\to\launchProcess.bat";

            try {
                Process proc = System.Diagnostics.Process.Start(psexec, parameters);
                proc.WaitForExit(12000);
            }
            catch {
                Console.WriteLine("Process on " + MachineDomainAddress + " could not be launched");
            }
        }


        /* Reboots host */
        private void RestartMachine(string MachineDomainAddress) {

            string parameters = @" -r -f -accepteula " + @"\\" + MachineDomainAddress + this.pstoolsLogin;
            try {
                Process proc = System.Diagnostics.Process.Start(psshutdown, parameters);
                proc.WaitForExit(12000);
            }
            catch {
                Console.WriteLine("Machine " + MachineDomainAddress + " could not be killed");
            }
        }



        /* Kills specified process at specified domain */
        private void KillProcess(string MachineDomainAddress, string processName) {

            string parameters = @" -t -accepteula " + @"\\" + MachineDomainAddress + this.pstoolsLogin + processName;
            try {
                Process proc = System.Diagnostics.Process.Start(pskill, parameters);
                proc.WaitForExit(12000);
            }
            catch {
                Console.WriteLine("Process " + processName + " could not be killed");
            }
        }
    }
}
