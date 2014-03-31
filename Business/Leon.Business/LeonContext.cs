using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Toolkit;
using Leon.Business.Services;

namespace Leon.Business
{
    public class LeonContext : ContextBase
    {

        public static LeonContext Current
        {
            get
            {
                return ContextUtility.GetOrAdd<LeonContext>("LeonContext", () => new LeonContext());
            }
        }

        public CategoryService CategoryService
        {
            get
            {
                return this.GetService<CategoryService>();
            }
        }

        public ProductService ProductService
        {
            get
            {
                return this.GetService<ProductService>();
            }
        }

        public CommentService CommentService
        {
            get
            {
                return this.GetService<CommentService>();
            }
        }

    }
}
