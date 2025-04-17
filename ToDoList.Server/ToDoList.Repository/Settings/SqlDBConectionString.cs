using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Repository.Settings
{
    public class SqlDBConectionString
    {
        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        public SqlDBConectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
