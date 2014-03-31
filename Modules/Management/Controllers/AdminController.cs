using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Toolkit;

namespace Leon.Modules.Management.Controllers
{
    public class AdminController : ModuleAreaControllerBase
    {
        #region Get pages

        public JsonResult GetPages(string siteName)
        {
            var treeNodes = new List<TreeNode>();
            var site = new Site(siteName);
            var pages = ServiceFactory.PageManager.All(site, String.Empty);
            foreach(var page in pages)
            {
                var treeNode = new TreeNode()
                {
                    Text = page.Name,
                    Value = page.FullName
                };

                AddChildPages(site, page, treeNode);
                treeNodes.Add(treeNode);
            }
            return Json(treeNodes, JsonRequestBehavior.AllowGet);
        }

        private void AddChildPages(Site site, Page page, TreeNode treeNode)
        {
            var childPages = ServiceFactory.PageManager.ChildPages(site, page.FullName, String.Empty);
            foreach(var childPage in childPages)
            {
                var childTreeNode = new TreeNode()
                {
                    Text = childPage.Name,
                    Value = childPage.FullName
                };

                AddChildPages(site, childPage, childTreeNode);
                treeNode.Children.Add(childTreeNode);
            }
        }

        #endregion

        #region Export

        [HttpGet]
        public ActionResult Export()
        {
            return View();
        }

        [HttpPost, ActionName("Export")]
        public ActionResult ExportPOST()
        {
            XDocument document = new XDocument();
            document.Add(new XElement("root"));

            // Export pages
            var site = Site.Current;
            var allPages = ServiceFactory.PageManager.All(site, String.Empty);
            RecursionVisitor.Visit(allPages, visit => pages =>
            {
                if(pages.Count() > 0)
                {
                    foreach(var page in pages)
                    {
                        Export(document, page);

                        var childPages = ServiceFactory.PageManager.ChildPages(site, page.FullName, String.Empty);
                        visit(childPages);
                    }
                }
            });

            return View();
        }

        private void Export(XDocument document, Page page)
        {
            XElement pageElement = new XElement("Page",
                new XAttribute("FullName", page.FullName));

            pageElement.Add(new XElement("ContentTitle", page.ContentTitle));
            pageElement.Add(new XElement("HtmlMeta",
                new XElement("HtmlTitle", page.HtmlMeta.HtmlTitle),
                new XElement("Keywords", page.HtmlMeta.Keywords),
                new XElement("Description", page.HtmlMeta.Description)));

            pageElement.Add(new XElement("Navigation"));
            pageElement.Add(new XElement("PagePositions"));
            pageElement.Add(new XElement("CustomFields"));

            document.Root.Add(pageElement);
        }

        #endregion

        #region Import

        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }

        [HttpPost, ActionName("Import")]
        public ActionResult ImportPOST()
        {
            return View();
        }

        #endregion
    }
}