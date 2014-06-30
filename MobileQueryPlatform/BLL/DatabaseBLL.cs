using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class DatabaseBLL
    {
        const string ORACLE_CONNECTIONSTRING = "Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = @HOST)(PORT = 1521)))(CONNECT_DATA = (SERVICE_NAME = @SERVICE_NAME)));Persist Security Info=True;User ID=@USERID;Password=@Password";
        const string MSSQL_CONNECTIONSTRING = "Data Source=@DataSource;Initial Catalog=@InitialCatalog;Persist Security Info=True;User ID=@UserID;Password=@Password";
        public static int SaveDatabase(Database db)
        {

        }

        public static int DeleteDatabase(int ID)
        {

        }
    }
}
