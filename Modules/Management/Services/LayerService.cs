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
    public class LayerService : ServiceBase<Layer>
    {
        public override string FolderName
        {
            get
            {
                return FolderNames.Layer;
            }
        }

        public override IContentQuery<TextContent> CreateQuery()
        {
            return base.CreateQuery()
                .WhereEquals(Setting.FieldNames.Site, Site.Current.FullName);
        }

        public override Layer Get(TextContent textContent)
        {
            if(textContent != null)
            {
                return new Layer(textContent);
            }
            return null;
        }

        public Layer GetLayer(Page_Context pageContext)
        {
            string pageUrl = PageUtility.CurrentPageUrl;
            string pageLayout = pageContext.PageLayout;

            var layers = GetLayers();
            layers = Filter(pageContext, layers);

            var layer = layers.FirstOrDefault(it => it.AttachOption == AttachOption.SingelPage && it.PageUrl.Equals(pageUrl, StringComparison.OrdinalIgnoreCase));
            if(layer == null)
            {
                layer = layers.FirstOrDefault(it => it.AttachOption == AttachOption.Layout && it.Layout.Equals(pageLayout, StringComparison.OrdinalIgnoreCase));
                if(layer == null)
                {
                    if(pageContext.PageRequestContext.Page.IsDefault)
                    {
                        layer = layers.FirstOrDefault(it => it.AttachOption == AttachOption.AllPages);
                    }
                    else
                    {
                        layer = layers.FirstOrDefault(it => it.AttachOption == AttachOption.AllPages || it.AttachOption == AttachOption.AllPagesExceptHompage);
                    }
                }
            }

            return layer;
        }

        public IEnumerable<Layer> GetLayers()
        {
            return GetLayers(Site.Current.FullName);
        }

        public IEnumerable<Layer> GetLayers(string siteFullName)
        {
            // TODO: Cache
            return GetAll();
        }

        private IEnumerable<Layer> Filter(Page_Context pageContext, IEnumerable<Layer> layers)
        {
            DateTime now = DateTime.Now;
            var minDate = DateTime.MinValue.ToLocalTime();

            var firstVisitedUUIDsCookie = pageContext.PageRequestContext.ControllerContext.RequestContext.HttpContext.Request.Cookies["Layer.ShowOnlyOnFirstVisit"];
            var firstVisitedUUIDs = String.Empty;
            if(firstVisitedUUIDsCookie != null)
            {
                firstVisitedUUIDs = firstVisitedUUIDsCookie.Value.AsString();
            }

            return layers.Where(it =>
                (it.StartDate == minDate || (it.StartDate != minDate && it.StartDate <= now)) &&
                (it.StopDate == minDate || (it.StopDate != minDate && it.StopDate >= now)) && !firstVisitedUUIDs.Contains(it.UUID));
        }
    }
}