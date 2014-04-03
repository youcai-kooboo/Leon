using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.ModuleArea.Monitor.Models
{
    public class Keys
    {
        public const string HTTPCONTEXT_IS_KOOBOO_PAGE = "__HttpContext_Is_Kooboo_Page__";
        public const string HTTPCONTEXT_EXCEPTION = "__HttpContext_Exception__";
        public const string HTTPCONTEXT_BEGINREQUEST_DATETIME = "__HttpContext_BeginRequest_DateTime__";

        public const string MODULE_CRAWL_DEPTH = "Module_Crawl_Depth";
        public const string MODULE_CRAWL_ENTRY = "Module_Crawl_Entry";
        public const string MODULE_CHECK_IMAGE = "Module_Check_Image";
        public const string MODULE_CHECK_JAVASCRIPT = "Module_Check_JavaScript";
        public const string MODULE_CHECK_CSS = "Module_Check_Css";
        public const string MODULE_MAX_TIMETAKEN = "Module_Max_TimeTaken";

        public const string COOKIE_NAME_FOR_TRACE = "__Module_Monitor_Trace__";
        public const string COOKIE_FOR_SITE = "__Visit_Site__";
        public const string COOKIE_FOR_VISIT_COUNT = "__Visit_Count__";
        public const string COOKIE_FOR_LAST_VISIT_DATE = "__Last_Visit_Date__";
        public const string COOKIE_FOR_UNIQUE_VISITOR_KEY = "__Unique_Visitor_Key__";

        #region Spider UserAgent
        public const string KBM_USER_AGENT = "Kooboo/Monitor/1.0";
        public const string GOOGLE_USER_AGENT = "Googlebot";
        public const string BAIDU_USER_AGENT = "Baiduspider";
        public const string YAHOO_USER_AGENT = "Yahoo!";
        public const string YODAO_USER_AGENT = "YodaoBot";
        public const string SOSO_USER_AGENT = "Sosospider";
        public const string SOGOU_USER_AGENT = "Sogou";
        #endregion


        #region Spider searck key
        public const string SEARCH_KEY_FOR_BAIDU = "wd";
        public const string SEARCH_KEY_FOR_GOOGLE = "q";
        public const string SEARCH_KEY_FOR_SOGOU = "query";
        public const string SEARCH_KEY_FOR_YAHOO = "p";
        public const string SEARCH_KEY_FOR_YODAO = "q";
        public const string SEARCH_KEY_FOR_SOSO = "w"; 
        #endregion

    }
}