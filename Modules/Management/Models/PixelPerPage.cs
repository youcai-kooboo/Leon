using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Toolkit;

namespace Leon.Modules.Management.Models
{
    public class PixelPerPage : TextContent
    {
        public PixelPerPage()
            : base(new TextContent())
        { }

        public PixelPerPage(TextContent textContent)
            : base(textContent)
        { }

        public class FieldNames
        {
            public const string Site = "Site";
            public const string PageName = "PageName";
            public const string Description = "Description";
            public const string Code = "Code";
            public const string StartDate = "StartDate";
            public const string StopDate = "StopDate";
            public const string DisplayPosition = "DisplayPosition";
        }

        /// <summary>
        /// Site full name
        /// </summary>
        public string Site
        {
            get
            {
                return this.GetString(FieldNames.Site);
            }
            set
            {
                this[FieldNames.Site] = value;
            }
        }

        public string PageName
        {
            get
            {
                return this.GetString(FieldNames.PageName);
            }
            set
            {
                this[FieldNames.PageName] = value;
            }
        }

        public string Description
        {
            get
            {
                return this.GetString(FieldNames.Description);
            }
            set
            {
                this[FieldNames.Description] = value;
            }
        }

        public string Code
        {
            get
            {
                return this.GetString(FieldNames.Code);
            }
            set
            {
                this[FieldNames.Code] = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this.GetDateTime(FieldNames.StartDate).ToLocalTime();
            }
            set
            {
                this[FieldNames.StartDate] = value.ToUniversalTime();
            }
        }

        public DateTime StopDate
        {
            get
            {
                return this.GetDateTime(FieldNames.StopDate).ToLocalTime();
            }
            set
            {
                this[FieldNames.StopDate] = value.ToUniversalTime();
            }
        }

        public string DisplayPosition
        {
            get
            {
                return this.GetString(FieldNames.DisplayPosition);
            }
            set
            {
                this[FieldNames.DisplayPosition] = value;
            }
        }
    }
}