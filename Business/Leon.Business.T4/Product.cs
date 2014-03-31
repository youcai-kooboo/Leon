using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Toolkit;

namespace Leon.Business.Models {

    public class Product :TextContent {

      public Product()
            : base() {
        }

        public Product(TextContent textContent)
            : base(textContent) {
        }

        public class FieldNames {
            public const string Name = "Name";
        }

        public string Name {
            get {
                return this.GetString(FieldNames.Name);
            }
            set {
                this[FieldNames.Name] = value;
            }
        }
    }
}
