using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Toolkit;

namespace Leon.Modules.Management.Models
{
    public class MetaSetting : TextContent
    {
        public MetaSetting()
            : base(new TextContent())
        { }

        public MetaSetting(TextContent textContent)
            : base(textContent)
        { }

        public class FieldNames
        {
            public const string Site = "Site";
            public const string Layout = "Layout";
            public const string Folder = "Folder";
            public const string Title = "Title";
            public const string Description = "Description";
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

        public string Folder
        {
            get
            {
                return this.GetString(FieldNames.Folder);
            }
            set
            {
                this[FieldNames.Folder] = value;
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
    }
}