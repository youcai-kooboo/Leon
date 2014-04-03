using Kooboo.Web.Mvc;
using Kooboo.CMS.ModuleArea.Monitor.Models;
using Kooboo.CMS.ModuleArea.Monitor.Services;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.CMS.Common;
using Kooboo.Web.Mvc.Paging;
using System.Dynamic;
using Kooboo.CMS.ModuleArea.Monitor.Repositories;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Kooboo.CMS.ModuleArea.Monitor.Controllers
{
    public class LogController : AdminControllerBase
    {
        private ILogProvider _logProvider;
        public LogController(ILogProvider logProvider)
        {
            this._logProvider = logProvider;
        }

        public ActionResult Index(string siteName, LogQuery query)
        {
            if (query.Start.HasValue)
            {
                query.Start = new DateTime(query.Start.Value.Year, query.Start.Value.Month, query.Start.Value.Day, 0, 0, 0, DateTimeKind.Utc);
            }
            if (query.End.HasValue)
            {
                query.End = new DateTime(query.End.Value.Year, query.End.Value.Month, query.End.Value.Day, 23, 59, 59, DateTimeKind.Utc);
            }
            var lst = this._logProvider.Search(query);
            return View(lst);
        }

        public ActionResult Exceptional(string siteName, LogQuery query)
        {
            if (query.Start.HasValue)
            {
                query.Start = new DateTime(query.Start.Value.Year, query.Start.Value.Month, query.Start.Value.Day, 0, 0, 0, DateTimeKind.Utc);
            }
            if (query.End.HasValue)
            {
                query.End = new DateTime(query.End.Value.Year, query.End.Value.Month, query.End.Value.Day, 23, 59, 59, DateTimeKind.Utc);
            }
            var lst = this._logProvider.Exceptional(query);
            return View(lst);
        }

        public ActionResult DailyReferrerGroup(string siteName, int? page, int? pageSize)
        {
            var start = DateTime.UtcNow.Date;
            var end = start.AddDays(1).AddSeconds(-1);
            var lst = this._logProvider.ReferrerGroups(page ?? 1, pageSize ?? 50, start, end);
            return View("ReferrerGroup", lst);
        }


        public ActionResult WeeklyReferrerGroup(string siteName, int? page, int? pageSize)
        {
            var end = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            var start = end.AddDays(-6).Date;
            var lst = this._logProvider.ReferrerGroups(page ?? 1, pageSize ?? 50, start, end);
            return View("ReferrerGroup", lst);
        }

        public ActionResult MonthlyReferrerGroup(string siteName, int? page, int? pageSize)
        {
            var end = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            var start = end.AddMonths(-1).Date;
            var lst = this._logProvider.ReferrerGroups(page ?? 1, pageSize ?? 50, start, end);
            return View("ReferrerGroup", lst);
        }

        public ActionResult Detail(string id, string siteName)
        {
            var log = this._logProvider.QueryById(id);
            return View(log);
        }

        [HttpPost]
        public ActionResult RemoveByPrevYear(string siteName)
        {
            JsonResultData result = new JsonResultData();

            try
            {
                this._logProvider.RemoveByPrevDate(DateTime.UtcNow.AddYears(-1));
                result.ReloadPage = true;
            }
            catch (Exception ex)
            {
                result.AddException(ex);
            }
            return Json(result);
        }

        public ActionResult PVUV()
        {
            return View();
        }

        public ActionResult BotBS()
        {
            return View();
        }

        public ActionResult Referrals()
        {
            return View();
        }

        public ActionResult SearchEngine()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MonthlyPV(int? year, int? month)
        {
            var end = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            var start = end.AddMonths(-1).Date;
            if (year.HasValue && month.HasValue)
            {
                start = new DateTime(year.Value, month.Value, 1, 0, 0, 0, DateTimeKind.Utc);
                end = start.AddMonths(1).AddSeconds(-1).Date;
            }
            var span = end.Subtract(start).Days;

            int days = span + 1;
            int[] totalData = new int[days];
            int[] errorData = new int[days];


            var lst = this._logProvider.GetPvStatus(start, end);

            for (int i = 0, j = days; i < j; i++)
            {
                foreach (var item in lst)
                {
                    if (item.Date.ToString("yyyy-MM-dd").Equals(start.AddDays(i).ToString("yyyy-MM-dd"), StringComparison.OrdinalIgnoreCase))
                    {
                        totalData[i] = item.Total;
                        errorData[i] = item.Error;
                    }
                }
            }
            return Json(new
            {
                StartYear = start.Year,
                StartMonth = start.Month - 1,
                StartDay = start.Day,
                TotalData = totalData,
                ErrorData = errorData
            });
        }

        [HttpPost]
        public ActionResult WeeklyPV(int? year, int? month, int? day)
        {
            DateTime end = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            DateTime start = end.AddDays(-6).Date;
            if (year.HasValue && month.HasValue && day.HasValue)
            {
                end = new DateTime(year.Value, month.Value, day.Value, 23, 59, 59, DateTimeKind.Utc);
                start = end.AddDays(-6).Date;
            }

            int[] totalData = new int[7];
            int[] errorData = new int[7];

            var lst = this._logProvider.GetPvStatus(start, end);

            for (int i = 0, j = 7; i < j; i++)
            {
                foreach (var item in lst)
                {
                    if (item.Date.ToString("yyyy-MM-dd").Equals(start.AddDays(i).ToString("yyyy-MM-dd"), StringComparison.OrdinalIgnoreCase))
                    {
                        totalData[i] = item.Total;
                        errorData[i] = item.Error;
                    }
                }
            }
            return Json(new
            {
                StartYear = start.Year,
                StartMonth = start.Month - 1,
                StartDay = start.Day,
                TotalData = totalData,
                ErrorData = errorData
            });
        }

        [HttpPost]
        public ActionResult DailyPV()
        {
            DateTime date = DateTime.UtcNow.Date;
            var lst = this._logProvider.GetHourPvByDay(date);
            var data = lst.Select(it => new
                {
                    Hour = it.DateHour.ToLocalTime().Hour,
                    Total = it.Total,
                    Error = it.Error
                }).ToArray();
            return Json(data);
        }

        [HttpPost]
        public ActionResult DayUV()
        {
            int[] totalData = new int[7];
            DateTime end = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            DateTime start = end.AddDays(-6).Date;
            var lst = this._logProvider.GetUvStatus(start, end);

            for (int i = 0, j = 7; i < j; i++)
            {
                foreach (var item in lst)
                {
                    if (item.Date.ToString("yyyy-MM-dd").Equals(start.AddDays(i).ToString("yyyy-MM-dd"), StringComparison.OrdinalIgnoreCase))
                    {
                        totalData[i] = item.Total;
                    }
                }
            }
            return Json(new
            {
                StartYear = start.Year,
                StartMonth = start.Month - 1,
                StartDay = start.Day,
                TotalData = totalData
            });
        }

        [HttpPost]
        public ActionResult WeekUV()
        {
            int[] dayData = new int[28];
            int[] weekData = new int[4];
            int dayOfWeek = (int)DateTime.UtcNow.DayOfWeek;
            DateTime end = DateTime.UtcNow.Date.AddDays(-(dayOfWeek + 1)).AddDays(1).AddSeconds(-1);
            DateTime start = end.AddDays(-27).Date;
            var lst = this._logProvider.GetUvStatus(start, end);
            for (int i = 0, j = dayData.Length; i < j; i++)
            {
                foreach (var item in lst)
                {
                    if (item.Date.ToString("yyyy-MM-dd").Equals(start.AddDays(i).ToString("yyyy-MM-dd"), StringComparison.OrdinalIgnoreCase))
                    {
                        dayData[i] = item.Total;
                    }
                }
            }
            for (int i = 0, j = weekData.Length; i < j; i++)
            {
                weekData[i] = dayData.Skip(i * 7).Take(7).Sum();
            }
            start = start.AddDays(6);
            return Json(new
            {
                StartYear = start.Year,
                StartMonth = start.Month - 1,
                StartDay = start.Day,
                TotalData = weekData
            });
        }

        [HttpPost]
        public ActionResult MonthUV(int? year)
        {
            int y = DateTime.UtcNow.Year;
            int m = 12;
            if (year.HasValue)
            {
                y = year.Value;
            }
            var start = new DateTime(y, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var days = DateTime.DaysInMonth(y, m);
            var end = new DateTime(y, m, days, 23, 59, 59, DateTimeKind.Utc);

            int[] totalData = new int[m];
            var lst = this._logProvider.GetUvStatus(start, end);

            for (int i = 1, j = m; i <= j; i++)
            {
                totalData[i - 1] = lst.Where(it => it.Date.Month==i)
                        .Sum(it => it.Total);
            }
            return Json(new
            {
                Year = y,
                TotalData = totalData
            });
        }

        [HttpPost]
        public ActionResult DailyReferrer()
        {
            DateTime start = DateTime.UtcNow.Date;
            DateTime end = start.AddDays(1).AddSeconds(-1);
            var dict = this._logProvider.GroupByReferrer(start, end, 20);
            return Json(dict.Select(it => new
            {
                Domain = it.Key,
                Total = it.Value
            }).ToArray().OrderByDescending(it => it.Total));
        }

        [HttpPost]
        public ActionResult WeeklyReferrer()
        {
            DateTime end = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            DateTime start = end.AddDays(-6).Date;//past a week
            var dict = this._logProvider.GroupByReferrer(start, end, 20);
            return Json(dict.Select(it => new
            {
                Domain = it.Key,
                Total = it.Value
            }).ToArray().OrderByDescending(it => it.Total));
        }

        [HttpPost]
        public ActionResult MonthlyReferrer(int? year, int? month)
        {
            DateTime end = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            DateTime start = end.AddMonths(-1).Date;//past a month
            var dict = this._logProvider.GroupByReferrer(start, end, 20);
            var data = dict.Select(it => new
            {
                Domain = it.Key,
                Total = it.Value
            }).ToArray().OrderByDescending(it => it.Total);
            return Json(data);
        }

        [HttpPost]
        public ActionResult DailySearchKeyGroup(string siteName,int? page,int? pageSize)
        {
            DateTime start = DateTime.UtcNow.Date;
            DateTime end = start.AddDays(1).AddSeconds(-1);//today
            var lst = this._logProvider.SearchKeyGroups(page ?? 1, pageSize ?? 50, start, end);
            return View("SearchKeyGroup", lst);
            
        }

        [HttpPost]
        public ActionResult WeeklySearchKeyGroup(string siteName, int? page, int? pageSize)
        {
            DateTime end = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            DateTime start = end.AddDays(-6).Date;//past a week
            var lst = this._logProvider.SearchKeyGroups(page ?? 1, pageSize ?? 50, start, end);
            return View("SearchKeyGroup", lst);
        }

        [HttpPost]
        public ActionResult MonthlySearchKeyGroup(string siteName,int? page,int? pageSize)
        {
            DateTime end = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            DateTime start = end.AddMonths(-1).Date;//past a month
            var lst = this._logProvider.SearchKeyGroups(page ?? 1, pageSize ?? 50, start, end);
            return View("SearchKeyGroup", lst);
        }

        [HttpPost]
        public ActionResult DailySearchEngine()
        {
            DateTime start = DateTime.UtcNow.Date;
            DateTime end = start.AddDays(1).AddSeconds(-1);
            var result = SpiderHelper.Spiders.ToDictionary(it => it.Key, it => 0, StringComparer.OrdinalIgnoreCase);
            var dict = this._logProvider.GroupByReferrer(start, end, 20);
            foreach (var key in dict.Keys)
            {
                if (result.Keys.Contains(key))
                {
                    result[key] = dict[key];
                }
            }
            var data = result.Where(it => it.Value > 0).Select(it => new
            {
                SearchEngine = it.Key,
                Total = it.Value
            }).ToArray().OrderByDescending(it => it.Total);

            return Json(data);
        }

        [HttpPost]
        public ActionResult WeeklySearchEngine()
        {
            DateTime end = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            DateTime start = end.AddDays(-6).Date;//past a week
            var result = SpiderHelper.Spiders.ToDictionary(it => it.Key, it => 0, StringComparer.OrdinalIgnoreCase);
            var dict = this._logProvider.GroupByReferrer(start, end, 20);
            foreach (var key in dict.Keys)
            {
                if (result.Keys.Contains(key))
                {
                    result[key] = dict[key];
                }
            }
            var data = result.Where(it => it.Value > 0).Select(it => new
            {
                SearchEngine = it.Key,
                Total = it.Value
            }).ToArray().OrderByDescending(it => it.Total);

            return Json(data);
        }

        [HttpPost]
        public ActionResult MonthlySearchEngine()
        {
            DateTime end = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            DateTime start = end.AddMonths(-1).Date;//past a month
            var result = SpiderHelper.Spiders.ToDictionary(it => it.Key, it => 0, StringComparer.OrdinalIgnoreCase);
            var dict = this._logProvider.GroupByReferrer(start, end, 20);
            foreach (var key in dict.Keys)
            {
                if (result.Keys.Contains(key))
                {
                    result[key] = dict[key];
                }
            }
            var data = result.Where(it => it.Value > 0).Select(it => new
            {
                SearchEngine = it.Key,
                Total = it.Value
            }).ToArray().OrderByDescending(it => it.Total);

            return Json(data);
        }

        [HttpPost]
        public ActionResult WeeklyBot()
        {
            DateTime end = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            DateTime start = end.AddDays(-6).Date;//past a week
            var result = SpiderHelper.SpiderUserAgents.ToDictionary(it => it.Key, it => 0, StringComparer.OrdinalIgnoreCase);
            var dict = this._logProvider.GroupByUserAgent(start, end);
            foreach (var key in dict.Keys)
            {
                foreach (var item in SpiderHelper.SpiderUserAgents.Keys)
                {
                    if (key.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        result[item] += dict[key];
                    }
                }
            }
            var data = result.Where(it => it.Value > 0).Select(it => new
            {
                Spider = it.Key,
                Total = it.Value
            }).ToArray().OrderByDescending(it => it.Total);
            return Json(data);
        }

        [HttpPost]
        public ActionResult MonthlyBot(int? year, int? month)
        {
            DateTime end = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            DateTime start = end.AddDays(-30).Date;//past a month
            if (year.HasValue && month.HasValue)
            {
                start = new DateTime(year.Value, month.Value, 1, 0, 0, 0, DateTimeKind.Utc);
                end = start.AddMonths(1).AddSeconds(-1);
            }
            var result = SpiderHelper.SpiderUserAgents.ToDictionary(it => it.Key, it => 0, StringComparer.OrdinalIgnoreCase);
            var dict = this._logProvider.GroupByUserAgent(start, end);
            foreach (var key in dict.Keys)
            {
                foreach (var item in SpiderHelper.SpiderUserAgents.Keys)
                {
                    if (key.IndexOf(item, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        result[item] += dict[key];
                    }
                }
            }
            var data = result.Where(it => it.Value > 0).Select(it => new
            {
                Spider = it.Key,
                Total = it.Value
            }).ToArray().OrderByDescending(it => it.Total);
            return Json(data);
        }

        [HttpPost]
        public ActionResult WeeklyBrowser(string siteName)
        {
            DateTime end = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            DateTime start = end.AddDays(-6).Date;//past a week
            var dict = GetBrowserShare(start, end);
            var data = dict.Select(it => new
            {
                Browser = it.Key,
                Total = it.Value
            }).ToArray().OrderByDescending(it => it.Total);
            return Json(data);
        }


        [HttpPost]
        public ActionResult MonthlyBrowser(string siteName)
        {
            DateTime end = DateTime.UtcNow.Date.AddDays(1).AddSeconds(-1);
            DateTime start = end.AddDays(-30).Date;//past a month
            var dict = GetBrowserShare(start, end);
            var data = dict.Select(it => new
            {
                Browser = it.Key,
                Total = it.Value
            }).ToArray().OrderByDescending(it => it.Total);
            return Json(data);
        }

        [NonAction]
        public Dictionary<string, int> GetBrowserShare(DateTime utcStart, DateTime utcEnd)
        {
            var browsers = new Dictionary<string, int>();
            Regex kbmReg = new Regex(Models.RegexPatterns.KoobooMonitor, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            Regex ieReg = new Regex(Models.RegexPatterns.IE, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            Regex firefoxReg = new Regex(Models.RegexPatterns.Firefox, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            Regex chromeReg = new Regex(Models.RegexPatterns.Chrome, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            Regex safariReg = new Regex(Models.RegexPatterns.Safari, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            Regex operaReg = new Regex(Models.RegexPatterns.Opera, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant | RegexOptions.Compiled);

            int ie = 0, firefox = 0, chrome = 0, safari = 0, opera = 0, other = 0;
            var dict = this._logProvider.GroupByUserAgent(utcStart, utcEnd);
            foreach (var key in dict.Keys)
            {
                if (kbmReg.IsMatch(key))
                {
                    continue;
                }
                if (ieReg.IsMatch(key))
                {
                    ie += dict[key];
                    continue;
                }
                if (firefoxReg.IsMatch(key))
                {
                    firefox += dict[key];
                    continue;
                }
                if (chromeReg.IsMatch(key))
                {
                    chrome += dict[key];
                    continue;
                }
                if (safariReg.IsMatch(key))
                {
                    safari += dict[key];
                    continue;
                }
                if (operaReg.IsMatch(key))
                {
                    opera += dict[key];
                    continue;
                }
                other += dict[key];
            }

            if (ie > 0)
            {
                browsers.Add("IE", ie);
            }
            if (firefox > 0)
            {
                browsers.Add("FireFox", firefox);
            }
            if (chrome > 0)
            {
                browsers.Add("Chrome", chrome);
            }
            if (safari > 0)
            {
                browsers.Add("Safari", safari);
            }
            if (opera > 0)
            {
                browsers.Add("Opera", opera);
            }
            if (other > 0)
            {
                browsers.Add("Other", other);
            }
            return browsers;
        }

        protected override void Dispose(bool disposing)
        {
            if (this._logProvider != null)
            {
                this._logProvider.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
