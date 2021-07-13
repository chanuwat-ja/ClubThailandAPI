using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Npgsql;
using Model.ClubThailand;
using System.Data;
using Dapper;
using SshNet;
using Renci.SshNet;

namespace DataAccess.Helper
{
    public class DatabaseHelper
    {
        private DbConnection dbConection;
        public DatabaseHelper(DbConnection dbCon)
        {
            dbConection = dbCon;
        }

        public DataTable Query(string sql)
        {
            DataTable dt = new DataTable();

            using (var client = new SshClient(dbConection.SSHHost, int.Parse(dbConection.SSHPort), dbConection.SSHUsername, dbConection.SSHPassword))
            {
                client.Connect();

                var port = new ForwardedPortLocal(dbConection.LocalHost, uint.Parse(dbConection.SSHPort), dbConection.LocalHost, uint.Parse(dbConection.LocalPort));
                client.AddForwardedPort(port);
                port.Start();

                using (var conn = new NpgsqlConnection(dbConection.ConnectionString))
                {
                    conn.Open();
                    //ret = conn.Query<dynamic>(sql, commandType: CommandType.Text).ToList();  

                    using (var cmd = new NpgsqlCommand(sql, conn))
                    {
                        using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }

                    conn.Close();
                }

                port.Stop();
                client.Disconnect();

            }

            return dt;
        }

        public int ExecuteQuery(string sql, DynamicParameters param)
        {
            int ret = 0;
            using (var client = new SshClient(dbConection.SSHHost, int.Parse(dbConection.SSHPort), dbConection.SSHUsername, dbConection.SSHPassword))
            {
                client.Connect();

                var port = new ForwardedPortLocal(dbConection.LocalHost, uint.Parse(dbConection.SSHPort), dbConection.LocalHost, uint.Parse(dbConection.LocalPort));
                client.AddForwardedPort(port);
                port.Start();

                using (var conn = new NpgsqlConnection(dbConection.ConnectionString))
                {
                    conn.Open();
                    //ret = conn.Query<dynamic>(sql, commandType: CommandType.Text).ToList();          
                    ret = conn.Execute(sql, param, commandType: CommandType.Text);

                    conn.Close();
                }

                port.Stop();
                client.Disconnect();

            }

            return ret;
        }


        public List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (var pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
