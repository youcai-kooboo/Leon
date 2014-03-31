using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Toolkit;
using Leon.Modules.Management.Models;

namespace Leon.Modules.Management.Services
{
    public class TopBannerService : ServiceBase<TopBanner>
    {
        public override string FolderName
        {
            get
            {
                return FolderNames.TopBannerSetting;
            }
        }

        public TopBanner GetTopBanner()
        {
            return this.CreateQuery()
                .DefaultOrder()
                .Published()
                .WhereEquals(Setting.FieldNames.Site, Site.Current.FullName)
                .FirstOrDefault()
                .MapTo<TopBanner>();
        }
    }
}
