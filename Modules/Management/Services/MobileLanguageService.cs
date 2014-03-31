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

namespace Leon.Modules.Management.Services {

    public class MobileLanguageService : ServiceBase<MobileLanguage> {

        public override string FolderName {
            get {
                return FolderNames.MobileLanguage;
            }
        }

        public IContentQuery<TextContent> CreateQuery(string siteFullName) {
            return base.CreateQuery()
                .WhereEquals(MobileLanguage.FieldNames.Site, siteFullName);
        }

        public override IContentQuery<TextContent> CreateQuery() {
            return this.CreateQuery(Site.Current.FullName);
        }

        public override MobileLanguage Get(TextContent textContent) {
            if (textContent != null) {
                return new MobileLanguage(textContent);
            }
            return null;
        }

        public IEnumerable<MobileLanguage> GetLanguages() {
            return GetLanguages(Site.Current.FullName);
        }

        public IEnumerable<MobileLanguage> GetLanguages(string siteFullName) {
            return this.CreateQuery()
                .WhereEquals(MobileLanguage.FieldNames.Site, siteFullName)
                .Published()
                .DefaultOrder()
                .MapTo<MobileLanguage>();
        }
    }
}
