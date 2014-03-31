using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Toolkit;

namespace Leon.Business.Models {

    public class Comment :TextContent {

      public Comment()
            : base() {
        }

        public Comment(TextContent textContent)
            : base(textContent) {
        }

        public class FieldNames {
            public const string Title = "Title";
            public const string Remark = "Remark";
        }

        public string Title {
            get {
                return this.GetString(FieldNames.Title);
            }
            set {
                this[FieldNames.Title] = value;
            }
        }

        public string Remark {
            get {
                return this.GetString(FieldNames.Remark);
            }
            set {
                this[FieldNames.Remark] = value;
            }
        }
    }
}
