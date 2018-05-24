using System;
using System.Data;
using System.Data.SqlClient;

namespace MachineRebootAutomation  {
    
    /* Class for SQL functions */
    class SQLHandler
    {
        private string SQLUser;
        private string SQLPass;
        private string database;
        private string server;

        // Constructor
        public SQLHandler(string server, string database, string SQLUser, string SQLPass)
        {
            this.server = server;
            this.database = database;
            this.SQLUser = SQLUser;
            this.SQLPass = SQLPass;
        }

        /* Returns a DataTable with results from our query */
        public DataTable GetData(string queryString)
        {
            string connectionString = String.Format("Server={0};Database={1};User Id={2};Password={3};", this.server, this.database, this.SQLUser, this.SQLPass);
            SqlDataAdapter queryAdapter = new SqlDataAdapter(queryString, connectionString);
            DataSet queryData = new DataSet();
            queryAdapter.Fill(queryData, "SQLData");
            DataTable SQLresult = queryData.Tables["SQLData"];
            return SQLresult;
        }
    }
}
