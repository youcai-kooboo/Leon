using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Kooboo.CMS.ModuleArea.Monitor
{
    public class SpiderHelper
    {
        public static Dictionary<string, string> Spiders = new Dictionary<string, string>()
        {
            {"www.baidu.com","wd"},
            {"www.google.com","q"},
            {"www.yahoo.com","p"},
            {"www.youdao.com","q"},
            {"www.soso.com","w"},
            {"www.sogou.com","query"}
        };

        public static Dictionary<string, string> SpiderUserAgents = new Dictionary<string, string>()
        {
            {"www.baidu.com","Baiduspider"},
            {"www.google.com","Googlebot"},
            {"www.yahoo.com","Yahoo!"},
            {"www.yodao.com","YodaoBot"},
            {"www.soso.com","Sosospider"},
            {"www.sogou.com","Sogou"}
        };

        public static string ExtractSearchKey(Uri uri)
        {
            string searchKey = string.Empty;
            foreach (var item in Spiders.Keys)
            {
                if (new Regex(item).IsMatch(uri.DnsSafeHost))
                {
                    var nameValues = HttpUtility.ParseQueryString(uri.Query);
                    searchKey = nameValues[Spiders[item]];
                    break;
                }
            }
            return searchKey;
        }
    }
}
