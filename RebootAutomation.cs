using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace MachineRebootAutomation {
    class RebootAutomationMain {
        static void Main(string[] args) {

            /*** MAIL SERVER CONFIG AND SETUP ***/
            string mailHost = "10.10.10.10";
            int mailPort = 25;
            string mailDomain = @"@domain.com";
            MailHandler mailer = new MailHandler(mailHost, mailPort, mailDomain);

            /*** SQL CONFIG AND SETUP ***/
            string SQLserver = "11.11.11.11";
            string SQLdatabase = "Monitoring";
            string SQLuser = "sql_username";
            string SQLpass = "sql_password";
            SQLHandler SQL = new SQLHandler(SQLserver, SQLdatabase, SQLuser, SQLpass);

            /*** PSTOOLS CONFIG AND SETUP ***/
            string TVMuser = "machine_username";
            string TVMpass = "machine_password";
            PSToolsHandler PSTools = new PSToolsHandler(TVMuser, TVMpass, mailer);
            
           //SQL query to get machine data
            string queryMachine = "SELECT Site, Machine, DomainAddress FROM DeviceStatus WHERE EquipmentType = 2";
            DataTable SQLresult = SQL.GetData(queryMachine);
                       
            // Example case for main sequence with parallell processing
            Parallel.ForEach(SQLresult.AsEnumerable(), Machine => {
            
                //Retrieving variables from datatable
                string EquipmentIPAddress = Machine.Field<string>("DomainAddress");
                string SiteName = Machine.Field<string>("Site");
                string MachineName = Machine.Field<string>("Machine");

                if (NetworkHandler.pingTest(EquipmentIPAddress)) {
                    PSTools.DeleteEODfilesAndReboot(DomainAddress);
                }

            }); 
            mailer.SendMail("toMail", "fromMail", "This is the subject", "This is the body");
            
            // For user input before closing
            Console.ReadLine();
        }
    }
}
