using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;

namespace Leon.Modules.Management.Services
{
    public class ServiceBase<TEntity> : Kooboo.CMS.Toolkit.Services.ServiceBase<TEntity>
        where TEntity : TextContent, new()
    {
        private const string RepositoryName = "Management";

        public ServiceBase()
            : base(new Repository(RepositoryName))
        { }
    }
}