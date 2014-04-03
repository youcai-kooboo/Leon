using Kooboo.CMS.ModuleArea.Monitor.Models;
using Kooboo.CMS.ModuleArea.Monitor.Repositories;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.ModuleArea.Monitor.Repositories
{
    public interface ILogProvider : IDisposable
    {
        void Add(Log obj);
        void Update(Log obj);
        Log QueryById(string id);
        IPagedList<Log> Search(LogQuery search);
        IPagedList<Log> Exceptional(LogQuery search);
        IQueryable<Log> CreateQuery();
        IEnumerable<DailyUV> GetUvStatus(DateTime utcStart,DateTime utcEnd);
        IEnumerable<DayPV> GetPvStatus(DateTime utcStart, DateTime utcEnd);
        IEnumerable<HourlyPv> GetHourPvByDay(DateTime utcDate);
        DayPV GetPvStatus(DateTime utcDate);
        Dictionary<string, int> GroupByUserAgent(DateTime? utcStart = null, DateTime? utcEnd = null);
        void RemoveByPrevDate(DateTime utcDate);
        Dictionary<string, int> GroupByReferrer(DateTime utcStart, DateTime utcEnd, int? top);
        IPagedList<ReferrerGroup> ReferrerGroups(int page, int pageSize, DateTime utcStart, DateTime utcEnd);
        IPagedList<SearchKeyGroup> SearchKeyGroups(int page, int pageSize, DateTime utcStart, DateTime utcEnd);
    }
}