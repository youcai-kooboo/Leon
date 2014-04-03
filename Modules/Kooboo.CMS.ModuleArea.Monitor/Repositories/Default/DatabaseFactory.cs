using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Content.Models.Paths;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Persistence.SqlServer;
using Kooboo.CMS.Sites.Persistence.EntityFramework;

namespace Kooboo.CMS.ModuleArea.Monitor.Repositories.Default
{
    //[Dependency(typeof(DatabaseFactory),ComponentLifeStyle.InRequestScope)]
    public class DatabaseFactory//:IDisposable
    {
        //private MonitorDbContext _dbContext=null;
        public static MonitorDbContext Get(string siteName)
        {
            //if (this._dbContext != null)
            //{
            //    return this._dbContext;
            //}
            //else
            //{
            //    string path = Path.Combine(RepositoryPath.BasePhysicalPath, "Monitor");
            //    string filePath = Path.Combine(path, string.Format("{0}_data.sdf", siteName));
            //    if (!Directory.Exists(path))
            //    {
            //        Directory.CreateDirectory(path);
            //    }
            //    this._dbContext = new MonitorDbContext(filePath);
            //    this._dbContext.Database.CreateIfNotExists();
            //    return this._dbContext;
            //}
            //if (string.IsNullOrWhiteSpace(siteName))
            //{
            //    throw new ArgumentNullException("siteName");
            //}
            //var filePath = GetFilePath(siteName);
            //var connStr = GetConnStr(filePath);
 
            //var dbContext = new MonitorDbContext(connStr);
            ////dbContext.Database.CreateIfNotExists();
            return new MonitorDbContext(SqlServerSettings.Instance.SharingDatabaseConnectionString);
        }

        public static string ContentFolder
        {
            get
            {
                return Path.Combine(RepositoryPath.BasePhysicalPath, "Monitor");
            }
        }

        public static string GetConnStr(string filePath)
        {
            //默认情况下，SqlCE的数据库文件有最大限制，这里给它显示指定一个较大的数值
            return string.Format(@"Data Source={0};Max Database Size=2047", filePath); 
        }

        public static string GetFilePath(string siteName)
        {
            if (!Directory.Exists(DatabaseFactory.ContentFolder))
            {
                Directory.CreateDirectory(DatabaseFactory.ContentFolder);
            }
            return Path.Combine(ContentFolder, string.Format("{0}_data.sdf", siteName));
        }

        //public void Dispose()
        //{
        //    //if (this._dbContext != null)
        //    //{
        //    //    this._dbContext.Dispose();
        //    //}
        //}
    }
}
