using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Toolkit;

namespace Leon.Modules.Management.Models
{
    public enum AttachOption
    {
        None = 0,

        /// <summary>
        /// Attach the layer to a single page
        /// </summary>
        SingelPage = 1,

        /// <summary>
        /// Attach the layer to a template
        /// </summary>
        Layout = 2,

        /// <summary>
        /// Attach the layer to all pages except the homepage
        /// </summary>
        AllPagesExceptHompage = 3,

        /// <summary>
        /// Attach the layer to all pages
        /// </summary>
        AllPages = 4,
    }

    public class Layer : TextContent
    {
        public Layer()
            : base(new TextContent())
        { }

        public Layer(TextContent textContent)
            : base(textContent)
        { }

        public class FieldNames
        {
            public const string Site = "Site";
            public const string Title = "Title";
            public const string Content = "Content";
            public const string AttachOption = "AttachOption";
            public const string PageUrl = "PageUrl";
            public const string Layout = "Layout";
            public const string StartDate = "StartDate";
            public const string StopDate = "StopDate";
        }

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

        public string Title
        {
            get
            {
                return this.GetString(FieldNames.Title);
            }
            set
            {
                this[FieldNames.Title] = value;
            }
        }

        public string Content
        {
            get
            {
                return this.GetString(FieldNames.Content);
            }
            set
            {
                this[FieldNames.Content] = value;
            }
        }

        public AttachOption AttachOption
        {
            get
            {
                return (AttachOption)this.GetInt(FieldNames.AttachOption);
            }
            set
            {
                this[FieldNames.AttachOption] = (int)value;
            }
        }

        public string PageUrl
        {
            get
            {
                return this.GetString(FieldNames.PageUrl);
            }
            set
            {
                this[FieldNames.PageUrl] = value;
            }
        }

        public string Layout
        {
            get
            {
                return this.GetString(FieldNames.Layout);
            }
            set
            {
                this[FieldNames.Layout] = value;
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
    }
}