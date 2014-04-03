using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common;
using Kooboo.CMS.ModuleArea.Monitor.Models;
using Kooboo.CMS.ModuleArea.Monitor.Services;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.Web.Mvc;

namespace Kooboo.CMS.ModuleArea.Monitor.Controllers
{
    public class HealthController : AdminControllerBase
    {
        private HyperlinkService _hyperlinkService;
        public HealthController(HyperlinkService hyperlinkService)
        {
            this._hyperlinkService = hyperlinkService;
        }

        public ActionResult Index(string siteName, HyperlinkQuery search)
        {
            var lst = this._hyperlinkService.Search(search);
            return View(lst);
        }

        public ActionResult Report(string siteName)
        {
            var model = this._hyperlinkService.GetStatistics();
            model.StatusAndTotal = this._hyperlinkService.GroupByStatus();
            return View(model);
        }

        public ActionResult LastCheckResult()
        {
            var statistics = this._hyperlinkService.GetStatistics();
            var result = new HealthJsonResultData();
            result.TotalPage = statistics.TotalPage;
            result.TotalJs = statistics.TotalJs;
            result.TotalImage = statistics.TotalImage;
            result.TotalCss = statistics.TotalCss;
            result.VisitedCss = statistics.VisitedCss;
            result.VisitedImage = statistics.VisitedImage;
            result.VisitedJs = statistics.VisitedJs;
            result.VisitedPage = statistics.VisitedPage;
            result.ErrorCss = statistics.ErrorCss;
            result.ErrorImage = statistics.ErrorImage;
            result.ErrorJs = statistics.ErrorJs;
            result.ErrorPage = statistics.ErrorPage;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReVisit(string siteName, Hyperlink[] model)
        {
            var result = new HealthJsonResultData();
            try
            {
                var service = HealthService.Get(siteName);
                if (service.IsRunning)
                {
                    result.AddErrorMessage("Has a task is running.");
                }
                else
                {
                    lock (service)
                    {
                        service.ReVisit(model);
                        //result.AddMessage("Start ReVisit.");
                    }
                }
                result.State = service.State;
                result.Running = service.IsRunning;
                result.TotalPage = service.TotalPage;
                result.TotalJs = service.TotalJs;
                result.TotalImage = service.TotalImage;
                result.TotalCss = service.TotalCss;
                result.VisitedCss = service.VisitedCss;
                result.VisitedImage = service.VisitedImage;
                result.VisitedJs = service.VisitedJs;
                result.VisitedPage = service.VisitedPage;
                result.Begin = service.Begin;
                result.End = service.End;
            }
            catch (Exception ex)
            {
                result.AddErrorMessage(ex.Message);
            }
            return Json(result);
        }

        public ActionResult ReCheck(string siteName)
        {
            var resultEntry = new HealthJsonResultData();
            try
            {
                var service = HealthService.Get(siteName);
                if (service.IsRunning)
                {
                    resultEntry.AddErrorMessage("Has a task is running.");
                }
                else
                {
                    lock (service)
                    {
                        service.ReCheck();
                        //resultEntry.AddMessage("The task has been started successfully.");
                    }
                }

                resultEntry.State = service.State;
                resultEntry.RunType = service.RunType;
                resultEntry.Running = service.IsRunning;
                resultEntry.TotalPage = service.TotalPage;
                resultEntry.TotalJs = service.TotalJs;
                resultEntry.TotalImage = service.TotalImage;
                resultEntry.TotalCss = service.TotalCss;
                resultEntry.VisitedCss = service.VisitedCss;
                resultEntry.VisitedImage = service.VisitedImage;
                resultEntry.VisitedJs = service.VisitedJs;
                resultEntry.VisitedPage = service.VisitedPage;
                resultEntry.Begin = service.Begin;
                resultEntry.End = service.End;
            }
            catch (Exception ex)
            {
                resultEntry.AddErrorMessage(ex.Message);
            }
            return Json(resultEntry, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Stop(string siteName)
        {
            var result = new HealthJsonResultData();
            try
            {
                var service = HealthService.Get(siteName);
                if (service.IsRunning)
                {
                    lock (service)
                    {
                        service.Stop();
                        //result.AddMessage("Task has been stopped.");
                    }
                }
                else
                {
                    result.AddErrorMessage("Has no task is running.");
                }

                result.State = service.State;
                result.RunType = service.RunType;
                result.Running = service.IsRunning;
                result.TotalPage = service.TotalPage;
                result.TotalJs = service.TotalJs;
                result.TotalImage = service.TotalImage;
                result.TotalCss = service.TotalCss;
                result.VisitedCss = service.VisitedCss;
                result.VisitedImage = service.VisitedImage;
                result.VisitedJs = service.VisitedJs;
                result.VisitedPage = service.VisitedPage;
                result.Begin = service.Begin;
                result.End = service.End;

            }
            catch (Exception ex)
            {
                result.AddErrorMessage(ex.Message);
            }
            return Json(result);
        }

        public ActionResult IsRunning(string siteName)
        {
            var service = HealthService.Get(siteName);
            return Content(service.IsRunning ? "true" : "false");
        }

        public ActionResult AsyncCurrentState(string siteName)
        {
            var result = new HealthJsonResultData();
            var service = HealthService.Get(siteName);
            if (service != null)
            {
                result.State = service.State;
                result.RunType = service.RunType;
                result.Running = service.IsRunning;
                result.TotalPage = service.TotalPage;
                result.TotalJs = service.TotalJs;
                result.TotalImage = service.TotalImage;
                result.TotalCss = service.TotalCss;
                result.VisitedCss = service.VisitedCss;
                result.VisitedImage = service.VisitedImage;
                result.VisitedJs = service.VisitedJs;
                result.VisitedPage = service.VisitedPage;
                result.Begin = service.Begin;
                result.End = service.End;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (this._hyperlinkService != null)
            {
                this._hyperlinkService.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class HealthJsonResultData : JsonResultData
    {
        public HealthJsonResultData()
        {

        }

        public HealthJsonResultData(ModelStateDictionary modelState)
            : base(modelState)
        {

        }
        public bool Running { get; set; }
        public HealthServiceState State { get; set; }
        public RunType RunType { get; set; }

        public int TotalPage { get; set; }
        public int VisitedPage { get; set; }
        public int ErrorPage { get; set; }

        public int TotalJs { get; set; }
        public int VisitedJs { get; set; }
        public int ErrorJs { get; set; }

        public int TotalImage { get; set; }
        public int VisitedImage { get; set; }
        public int ErrorImage { get; set; }

        public int TotalCss { get; set; }
        public int VisitedCss { get; set; }
        public int ErrorCss { get; set; }

        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }
    }
}
