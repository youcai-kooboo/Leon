using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Toolkit;
using Kooboo.CMS.Toolkit.Templates;
using Leon.Modules.Management.Models;

namespace Leon.Modules.Management.Services
{
    public class PixelPerLayoutService : ServiceBase<PixelPerLayout>
    {
        public override string FolderName
        {
            get
            {
                return FolderNames.PixelPerLayout;
            }
        }

        public override IContentQuery<TextContent> CreateQuery()
        {
            return base.CreateQuery()
                .WhereEquals(Setting.FieldNames.Site, Site.Current.FullName);
        }

        public IEnumerable<PixelPerLayout> GetAllBySiteName(string siteName)
        {
            return base.CreateQuery().WhereEquals(Setting.FieldNames.Site, siteName).MapTo<PixelPerLayout>();
        }

        public override PixelPerLayout Get(TextContent textContent)
        {
            if(textContent != null)
            {
                return new PixelPerLayout(textContent);
            }
            return null;
        }

        /// <summary>
        /// Get current layout pixels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PixelPerLayout> GetPixels()
        {
            var currentPage = PageUtility.CurrentPage;
            if(currentPage != null)
            {
                string currentPageLayout = currentPage.Layout;
                return GetPixels(currentPageLayout);
            }
            return Enumerable.Empty<PixelPerLayout>();
        }

        public IEnumerable<PixelPerLayout> GetPixels(string layout)
        {
            IWhereExpression startDateExpression = (IWhereExpression)base.CreateQuery()
                .WhereLessThanOrEqualDate(PixelPerLayout.FieldNames.StartDate, DateTime.Now)
                .Or(new WhereEqualsExpression(null, PixelPerLayout.FieldNames.StartDate, null))
                .Expression;

            IWhereExpression stopDateExpression = (IWhereExpression)base.CreateQuery()
                .WhereGreaterThanOrEqualDate(PixelPerLayout.FieldNames.StopDate, DateTime.Now)
                .Or(new WhereEqualsExpression(null, PixelPerLayout.FieldNames.StopDate, null))
                .Expression;

            var pixels = this.CreateQuery()
                .WhereEquals(PixelPerLayout.FieldNames.Layout, layout)
                .Where(startDateExpression)
                .Where(stopDateExpression)
                .Published()
                .DefaultOrder()
                .MapTo<PixelPerLayout>()
                .ToList();

            if(pixels.Count > 0)
            {
                foreach(var pixel in pixels)
                {
                    if(!String.IsNullOrEmpty(pixel.Code))
                    {
                        var templateRender = new TemplateRender(pixel.Code);
                        templateRender.Text("PageName", Page_Context.Current.PageRequestContext.TempPage().Navigation.DisplayText);
                        pixel.Code = templateRender.ToString();
                    }
                }
            }

            return pixels;
        }
    }
}