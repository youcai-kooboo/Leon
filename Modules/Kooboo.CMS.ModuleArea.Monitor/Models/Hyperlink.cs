using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Models
{
    public class Hyperlink : IEntity
    {
        public Hyperlink()
        {
            this.UtcCreationDate = DateTime.UtcNow;
        }

        public Hyperlink(string url)
        {
            this.Url = url;
            this.UtcCreationDate = DateTime.UtcNow;
        }

        public string Id
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public string ParentUrl
        {
            get;
            set;
        }

        public string InnerText
        {
            get;
            set;
        }

        public int Depth
        {
            get;
            set;
        }

        public int Status
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }

        public long ContentLength
        {
            get;
            set;
        }

        public bool IsPage
        {
            get
            {
                return this.ResourceType!=null&&this.ResourceType.Equals(Models.ResourceType.PAGE, StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool IsImage
        {
            get
            {
                return this.ResourceType != null && this.ResourceType.Equals(Models.ResourceType.IMAGE, StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool IsCss
        {
            get
            {
                return this.ResourceType != null && this.ResourceType.Equals(Models.ResourceType.CSS, StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool IsJavascript
        {
            get
            {
                return this.ResourceType != null && this.ResourceType.Equals(Models.ResourceType.JAVASCRIPT, StringComparison.OrdinalIgnoreCase);
            }
        }

        public int ExtractQty
        {
            get;
            set;
        }

        public DateTime UtcCreationDate
        {
            get;
            set;
        }

        public DateTime? UtcVisitDate
        {
            get;
            set;
        }

        /// <summary>
        /// 访问页面所耗费的时长,单位:秒
        /// </summary>
        public double VisitTime
        {
            get;
            set;
        }

        /// <summary>
        /// 提取数据所耗费的总时长,单位:秒
        /// </summary>
        public double ExtractTime
        {
            get;
            set;
        }

        public string ResourceType
        {
            get;
            set;
        }

        #region static member
        private static Dictionary<string, PropertyInfo> _properties;
        static Hyperlink()
        {
            _properties = typeof(Hyperlink).GetProperties().ToDictionary(it => it.Name.ToLower());
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
        #endregion
    }

    #region HyperlinkComparer
    public class HyperlinkComparer : IEqualityComparer<Hyperlink>
    {
        public bool Equals(Hyperlink x, Hyperlink y)
        {
            return x.Url.ToLower().Equals(y.Url.ToLower());
        }

        public int GetHashCode(Hyperlink obj)
        {
            return obj.Url.GetHashCode();
        }
    }
    #endregion

    public class ResourceType
    {
        public const string PAGE = "Page";
        public const string CSS = "Css";
        public const string IMAGE = "Image";
        public const string JAVASCRIPT = "JavaScript";
    }
}
