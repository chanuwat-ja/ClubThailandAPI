using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using Npgsql;
using NpgsqlTypes;
using DataAccess;
using Model.ClubThailand;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ClubThailandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        private readonly Area _area;

        private AppSetting appSetting;

        public AreaController(IConfiguration iconfig, IWebHostEnvironment env)
        {
            _configuration = iconfig;
            _env = env;
            appSetting = new AppSetting();
            appSetting.DatabaseConnection = new DbConnection();
            appSetting.DatabaseConnection.ConnectionString = _configuration.GetSection("DbConnection").GetSection("ConnectionString").Value;
            appSetting.DatabaseConnection.SSHHost = _configuration.GetSection("DbConnection").GetSection("SSHHost").Value;
            appSetting.DatabaseConnection.SSHPort = _configuration.GetSection("DbConnection").GetSection("SSHPort").Value;
            appSetting.DatabaseConnection.SSHUsername = _configuration.GetSection("DbConnection").GetSection("SSHUsername").Value;
            appSetting.DatabaseConnection.SSHPassword = _configuration.GetSection("DbConnection").GetSection("SSHPassword").Value;
            appSetting.DatabaseConnection.LocalHost = _configuration.GetSection("DbConnection").GetSection("LocalHost").Value;
            appSetting.DatabaseConnection.LocalPort = _configuration.GetSection("DbConnection").GetSection("LocalPort").Value;

            this._area = new Area(appSetting);

        }


        [HttpGet]
        [Route("all")]
        public JsonResult GetAllArea()
        {

            List<M_Area> ret = _area.All();
            return new JsonResult(ret);
        }


        [HttpPost]
        [Route("add")]
        public JsonResult AddArea(M_Area mArea)
        {
            string strRet = "";
            strRet = _area.Add(mArea);           
            return new JsonResult(strRet);
        }

        [HttpPut]
        [Route("update")]
        public JsonResult UpdateArea(M_Area mArea)
        {
            string strRet = "";
            strRet = _area.Update(mArea);
            return new JsonResult(strRet);
        }

        [HttpDelete]
        [Route("delete")]
        public JsonResult DeleteArea(string AreaCode)
        {
            string strRet = "";
            strRet = _area.Delete(AreaCode);
            return new JsonResult(strRet);
        }

        [HttpPost]
        [Route("savefile")]
        public JsonResult SaveFile()
        {
            string strRet = "";
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];

                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using(var stream = new FileStream(physicalPath,FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);

            }
            catch(Exception ex)
            {
                return new JsonResult("anonymous.png");
            }
         
            return new JsonResult(strRet);
        }



    }
}
