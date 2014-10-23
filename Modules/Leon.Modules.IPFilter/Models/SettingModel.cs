using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Leon.Modules.IPFilter.Models
{
    public class SettingModel
    {
        public int FilterType
        {
            get;
            set;
        }

        public int FilterScope
        {
           get;
           set;
        }

        public string ForbiddenHtml
        {
            get;
            set;
        }

        public bool Enable
        {
            get;
            set;
        }
        public IEnumerable<SelectListItem> FilterTypeList { get; set; }
        public IEnumerable<SelectListItem> FilterScopeList { get; set; }

    }
}
