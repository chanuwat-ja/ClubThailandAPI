using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ClubThailand
{
    public class AppSetting
    {
        public DbConnection DatabaseConnection { get; set; }

    }

    public class DbConnection
    {
        public string ConnectionString { get; set; }
        public string SSHHost { get; set; }
        public string SSHPort { get; set; }
        public string SSHUsername { get; set; }
        public string SSHPassword { get; set; }
        public string LocalHost { get; set; }
        public string LocalPort { get; set; }       
    }
}
