using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Kooboo.CMS.ModuleArea.Monitor.Models
{
    public class Log:IEntity
    {
        public Log()
        {
            this.DateTime = DateTime.UtcNow;
        }

        public string Id
        {
            get;
            set;
        }

        public DateTime DateTime
        {
            get;
            set;
        }

        public string Method
        {
            get;
            set;
        }

        public string UriStem
        {
            get;
            set;
        }

        public string UriQuery
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string ClientIP
        {
            get;
            set;
        }

        public string UserAgent
        {
            get;
            set;
        }

        public string Referrer
        {
            get;
            set;
        }

        public string ReferrerDomain
        {
            get;
            set;
        }

        public int Status
        {
            get;
            set;
        }

        public int SubStatus
        {
            get;
            set;
        }

        public string Exception
        {
            get;
            set;
        }

        public string StackTrace
        {
            get;
            set;
        }

        public string SiteName
        {
            get;
            set;
        }

        public bool HasException
        {
            get
            {
                return !string.IsNullOrEmpty(this.Exception) || !string.IsNullOrEmpty(this.StackTrace);
            }
        }

        public double TimeTaken
        {
            get;
            set;
        }

        public string UniqueVisitorKey
        {
            get;
            set;
        }

        /// <summary>
        /// 访问本站点的次数统计
        /// </summary>
        public int VisitCount
        {
            get;
            set;
        }

        /// <summary>
        /// 上一次访问本站点的时间
        /// </summary>
        public DateTime? LastVisitDate
        {
            get;
            set;
        }

        public string SearchKey
        {
            get;
            set;
        }

        private static Dictionary<string, PropertyInfo> _properties;
        static Log()
        {
            _properties = typeof(Log).GetProperties().ToDictionary(it => it.Name.ToLower());
        }
        public static PropertyInfo GetProperty(string propName)
        {
            PropertyInfo prop = null;
            propName = propName.TrimOrNull();
            if (!string.IsNullOrEmpty(propName))
            {
                propName = propName.ToLower();
                if (_properties.Keys.Contains(propName))
                {
                    prop = _properties[propName];
                }
            }
            return prop;
        }
    }
}