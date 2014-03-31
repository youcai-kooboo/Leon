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
    public class PixelPerPageService : ServiceBase<PixelPerPage>
    {
        public override string FolderName
        {
            get
            {
                return FolderNames.PixelPerPage;
            }
        }

        public override IContentQuery<TextContent> CreateQuery()
        {
            return base.CreateQuery()
                .WhereEquals(Setting.FieldNames.Site, Site.Current.FullName);
        }

        public IEnumerable<PixelPerPage> GetAllBySiteName(string siteName)
        {
            return base.CreateQuery().WhereEquals(Setting.FieldNames.Site, siteName).MapTo<PixelPerPage>();
        }

        public override PixelPerPage Get(TextContent textContent)
        {
            if(textContent != null)
            {
                return new PixelPerPage(textContent);
            }
            return null;
        }

        /// <summary>
        /// Get current page pixels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PixelPerPage> GetPixels()
        {
            var currentPage = PageUtility.CurrentPage;
            if(currentPage != null)
            {
                return GetPixels(currentPage);
            }
            return Enumerable.Empty<PixelPerPage>();
        }

        public IEnumerable<PixelPerPage> GetPixels(Page pageName)
        {
            IWhereExpression startDateExpression = (IWhereExpression)base.CreateQuery()
                .WhereLessThanOrEqualDate(PixelPerPage.FieldNames.StartDate, DateTime.Now)
                .Or(new WhereEqualsExpression(null, PixelPerPage.FieldNames.StartDate, null))
                .Expression;

            IWhereExpression stopDateExpression = (IWhereExpression)base.CreateQuery()
                .WhereGreaterThanOrEqualDate(PixelPerPage.FieldNames.StopDate, DateTime.Now)
                .Or(new WhereEqualsExpression(null, PixelPerPage.FieldNames.StopDate, null))
                .Expression;

            var pixels = this.CreateQuery()
                .WhereEquals(PixelPerPage.FieldNames.PageName, pageName.FullName)
                .Where(startDateExpression)
                .Where(stopDateExpression)
                .Published()
                .DefaultOrder()
                .MapTo<PixelPerPage>()
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