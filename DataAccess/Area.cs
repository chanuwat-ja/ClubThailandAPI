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
using DataAccess.Helper;
namespace DataAccess
{
    public class Area
    {
        private AppSetting appSetting;
        private DatabaseHelper dbHelper;

        public Area(AppSetting app)
        {

            appSetting = new AppSetting();
            appSetting.DatabaseConnection = new DbConnection();
            appSetting.DatabaseConnection = app.DatabaseConnection;
            dbHelper = new DatabaseHelper(app.DatabaseConnection);

        }

        public List<M_Area> All()
        {
            List<M_Area> ret = new List<M_Area>();
            var sql = @"SELECT * FROM ""M_Area""; ";
            try
            {
                ret = dbHelper.ConvertDataTable<M_Area>(dbHelper.Query(sql));
            }
            catch(Exception ex)
            {
                ret = null;
            }
           


            return ret;
        }

        public string Add(M_Area mArea)
        {

            string strRet = "";

            var sql = @"INSERT INTO
                        ""M_Area""(""AreaCode"", ""AreaNameEng"", ""AreaNameJapan"")
	                      VALUES (@AreaCode, @AreaNameEng, @AreaNameJapan); ";
            var p = new DynamicParameters();
            p.Add("@AreaCode", mArea.AreaCode);
            p.Add("@AreaNameEng", mArea.AreaNameEng);
            p.Add("@AreaNameJapan", mArea.AreaNameJapan);
            try
            {
                int retExcute = 0;
                retExcute = dbHelper.ExecuteQuery(sql, p);
                if(retExcute == 1)
                {
                    strRet = "Success";
                }
                else
                {
                    strRet = "Error";
                }
               
            }
            catch(Exception ex)
            {
                strRet = ex.Message;
            }
            
            return strRet;
        }

        public string Update(M_Area mArea)
        {
            string strRet = "";

            var sql = @"Update 
                        ""M_Area""
                        set ""AreaNameEng"" = @AreaNameEng,
                        ""AreaNameJapan"" =  @AreaNameJapan
                        where ""AreaCode"" = @AreaCode ;";
            var p = new DynamicParameters();
            p.Add("@AreaCode", mArea.AreaCode);
            p.Add("@AreaNameEng", mArea.AreaNameEng);
            p.Add("@AreaNameJapan", mArea.AreaNameJapan);
            try
            {
                int retExcute = 0;
                retExcute = dbHelper.ExecuteQuery(sql, p);
                if (retExcute == 1)
                {
                    strRet = "Success";
                }
                else
                {
                    strRet = "Error";
                }

            }
            catch (Exception ex)
            {
                strRet = ex.Message;
            }
            return strRet;
        }

        public string Delete(string AreaCode)
        {
            string strRet = "";

            var sql = @"Delete
                        From ""M_Area""
                        where ""AreaCode"" = @AreaCode;";
            var p = new DynamicParameters();
            p.Add("@AreaCode", AreaCode);
            try
            {
                int retExcute = 0;
                retExcute = dbHelper.ExecuteQuery(sql, p);
                if (retExcute == 1)
                {
                    strRet = "Success";
                }
                else
                {
                    strRet = "Error";
                }

            }
            catch (Exception ex)
            {
                strRet = ex.Message;
            }

            return strRet;
        }




    }
}
