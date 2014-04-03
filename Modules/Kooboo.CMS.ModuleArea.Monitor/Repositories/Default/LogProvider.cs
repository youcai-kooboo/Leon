using Kooboo.CMS.ModuleArea.Monitor.Models;
using Kooboo.CMS.ModuleArea.Monitor.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Linq.Expressions;
using System.Data.EntityClient;
using System.Data.SqlClient;
using Kooboo.Web.Mvc.Paging;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Repositories.Default
{
    public class LogProvider : ILogProvider
    {
        public string _siteName;
        private MonitorDbContext _dbContext;
        public LogProvider()
        {
            if (HttpContext.Current != null)
            {
                this._siteName = HttpContext.Current.Request["siteName"];
            }
            if (string.IsNullOrEmpty(this._siteName) && Kooboo.CMS.Sites.Models.Site.Current != null)
            {
                this._siteName = Kooboo.CMS.Sites.Models.Site.Current.Name;
            }
            if (!string.IsNullOrWhiteSpace(this._siteName))
            {
                this._dbContext = DatabaseFactory.Get(this._siteName);
            }
        }

        private string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        public void Add(Log entity)
        {
            entity.Id = GenerateId();
            var db = DatabaseFactory.Get(entity.SiteName);
            db.Logs.Add(entity);
            db.SaveChanges();
            db.Dispose();
        }

        public IQueryable<Log> CreateQuery()
        {
            return this._dbContext.Logs.AsQueryable();
        }

        public Log QueryById(string id)
        {
            return this._dbContext.Logs.FirstOrDefault(it => it.Id.Equals(id));
        }

        public void Update(Log obj)
        {
            this._dbContext.Logs.Attach(obj);
            this._dbContext.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            this._dbContext.SaveChanges();
        }


        public void Delete(Log obj)
        {
            var old = this._dbContext.Logs.FirstOrDefault(it => it.Id.Equals(obj.Id));
            if (old != null)
            {
                this._dbContext.Logs.Remove(old);
                this._dbContext.SaveChanges();
            }
        }

        private const string SQL_FOR_PV_GROUPBY_DATE = @"SELECT
            CONVERT(NCHAR(10), IT.[DATETIME], 20) AS [DATE]
            ,COUNT(1) AS [TOTAL]
            ,SUM(CASE WHEN IT.[EXCEPTION] IS NULL THEN 0 ELSE 1 END) [ERRORS]
            FROM [KBM_MONITOR_LOGS] IT
            WHERE IT.[DATETIME]>=@start AND IT.[DATETIME]<=@end
            GROUP BY CONVERT(NCHAR(10), IT.[DATETIME], 20)
            ORDER BY CONVERT(NCHAR(10), IT.[DATETIME], 20) DESC";
        public IEnumerable<DayPV> GetPvStatus(DateTime start, DateTime end)
        {
            var lst = new List<DayPV>();
            var p0 = new SqlParameter("@start", SqlDbType.DateTime);
            p0.Value = start;
            var p1 = new SqlParameter("@end", SqlDbType.DateTime);
            p1.Value = end;
            var table = this.GetDatatable(SQL_FOR_PV_GROUPBY_DATE, p0, p1);
            DayPV info = null;
            foreach (DataRow row in table.Rows)
            {
                info = new DayPV();
                info.Date = DateTime.Parse(row["DATE"].ToString());
                info.Total = int.Parse(row["TOTAL"].ToString());
                info.Error = int.Parse(row["ERRORS"].ToString());
                lst.Add(info);
            }
            return lst;
        }

        public DayPV GetPvStatus(DateTime date)
        {
            DayPV ret = null;
            DateTime start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, date.Kind);
            DateTime end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, date.Kind);
            var p0 = new SqlParameter("@start", SqlDbType.DateTime);
            p0.Value = start;
            var p1 = new SqlParameter("@end", SqlDbType.DateTime);
            p1.Value = end;
            var table = this.GetDatatable(SQL_FOR_PV_GROUPBY_DATE, p0, p1);
            if (table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];
                ret = new DayPV();
                ret.Date = DateTime.Parse(row["DATE"].ToString());
                ret.Total = int.Parse(row["TOTAL"].ToString());
                ret.Error = int.Parse(row["ERRORS"].ToString());
            }
            else
            {
                ret = new DayPV
                {
                    Date = date,
                    Total = 0,
                    Error = 0
                };
            }
            return ret;
        }

        private DataTable GetDatatable(string cmdText, params SqlParameter[] parameters)
        {
            var table = new DataTable();
            var conn = this._dbContext.Database.Connection as SqlConnection;
            var comm = new SqlCommand(cmdText, conn);
            if (parameters.Length > 0)
            {
                comm.Parameters.AddRange(parameters);
            }
            var sda = new SqlDataAdapter(comm);
            try
            {
                conn.Open();
                sda.Fill(table);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sda.Dispose();
                comm.Dispose();
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            return table;
        }

        private int ExecuteNonQuery(string cmdText, params SqlParameter[] parameters)
        {
            var result = 0;
            var conn = this._dbContext.Database.Connection as SqlConnection;
            var comm = new SqlCommand(cmdText, conn);
            if (parameters.Length > 0)
            {
                comm.Parameters.AddRange(parameters);
            }
            try
            {
                conn.Open();
                result = comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                comm.Dispose();
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
            return result;
        }

        public IPagedList<Log> Search(LogQuery search)
        {
            PagedList<Log> lst = null;
            var query = this.CreateQuery();
            if (search.Start.HasValue)
            {
                query = query.Where(it => it.DateTime >= search.Start);
            }
            if (search.End.HasValue)
            {
                query = query.Where(it => it.DateTime <= search.End);
            }
            if (!string.IsNullOrEmpty(search.Method))
            {
                query = query.Where(it => it.Method.Equals(search.Method, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(search.UserName))
            {
                query = query.Where(it => it.UserName.Equals(search.UserName, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(search.ClientIP))
            {
                query = query.Where(it => it.ClientIP.Equals(search.ClientIP));
            }
            if (!string.IsNullOrEmpty(search.UVKey))
            {
                query = query.Where(it => it.UniqueVisitorKey.Equals(search.UVKey, StringComparison.OrdinalIgnoreCase));
            }
            if (search.Status.HasValue && search.Status > 0)
            {
                query = query.Where(it => it.Status == search.Status);
            }
            var prop = Log.GetProperty(search.SortField);
            if (prop != null)
            {
                var methodName = "OrderBy";//default is asc
                if (!string.IsNullOrEmpty(search.SortDir) && search.SortDir.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    methodName = "OrderByDescending";
                }
                var type = typeof(Log);
                var parameter = Expression.Parameter(type, "it");
                var propertyAccess = Expression.MakeMemberAccess(parameter, prop);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);
                var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { type, prop.PropertyType }, query.Expression, Expression.Quote(orderByExpression));
                query = query.Provider.CreateQuery<Log>(resultExpression);
            }
            else
            {
                query = query.OrderByDescending(it => it.DateTime);
            }
            lst = query.ToPagedList(search.Page ?? 1, search.PageSize ?? 50);
            return lst;
        }

        public IPagedList<Log> Exceptional(LogQuery search)
        {
            PagedList<Log> lst = null;
            IQueryable<Log> query = this.CreateQuery();

            if (search.Start.HasValue)
            {
                query = query.Where(it => it.DateTime >= search.Start);
            }
            if (search.End.HasValue)
            {
                query = query.Where(it => it.DateTime <= search.End);
            }
            if (!string.IsNullOrEmpty(search.UserName))
            {
                query = query.Where(it => it.UserName.Equals(search.UserName, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(search.Method))
            {
                query = query.Where(it => it.Method.Equals(search.Method, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(search.ClientIP))
            {
                query = query.Where(it => it.ClientIP.Equals(search.ClientIP));
            }
            if (search.Status.HasValue && search.Status > 0)
            {
                query = query.Where(it => it.Status == search.Status);
            }
            var prop = Log.GetProperty(search.SortField);
            if (prop != null)
            {
                string methodName = "OrderBy";//default is asc
                if (!string.IsNullOrEmpty(search.SortDir) && search.SortDir.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    methodName = "OrderByDescending";
                }
                var type = typeof(Log);
                var parameter = Expression.Parameter(type, "it");
                var propertyAccess = Expression.MakeMemberAccess(parameter, prop);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);
                var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { type, prop.PropertyType }, query.Expression, Expression.Quote(orderByExpression));
                query = query.Provider.CreateQuery<Log>(resultExpression);
            }
            else
            {
                query = query.OrderByDescending(it => it.DateTime);
            }
            lst = query.Where(it => !string.IsNullOrEmpty(it.Exception)).OrderByDescending(it => it.DateTime).ToPagedList(search.Page ?? 1, search.PageSize ?? 50);
            return lst;
        }

        public Dictionary<string, int> GroupByUserAgent(DateTime? start = null, DateTime? end = null)
        {
            var dict = new Dictionary<string, int>();
            var sql = @"SELECT IT.[USERAGENT] as UserAgent,
                        COUNT(1) as Qty
                        FROM [KBM_MONITOR_LOGS] IT
                        WHERE IT.[USERAGENT]  IS NOT NULL {0}
                        GROUP BY IT.[USERAGENT]";
            var where = new StringBuilder();
            var parameters = new List<SqlParameter>();
            if (start.HasValue)
            {
                where.Append(" AND IT.[DATETIME]>=@start");
                SqlParameter p0 = new SqlParameter("@start", SqlDbType.DateTime);
                p0.Value = start.Value;
                parameters.Add(p0);
            }
            if (end.HasValue)
            {
                where.Append(" AND IT.[DATETIME]<=@end");
                SqlParameter p1 = new SqlParameter("@end", SqlDbType.DateTime);
                p1.Value = end.Value;
                parameters.Add(p1);
            }
            var table = this.GetDatatable(string.Format(sql, where), parameters.ToArray());
            foreach (DataRow row in table.Rows)
            {
                dict.Add(Convert.ToString(row["UserAgent"]), Convert.ToInt32(row["Qty"]));
            }
            return dict;
        }

        private const string SQL_FOR_REMOVE_PRE_DATE = @"DELETE FROM [KBM_Monitor_Logs] AS [IT] WHERE [IT].[DATETIME]<=@date";
        public void RemoveByPrevDate(DateTime date)
        {
            var p0 = new SqlParameter("@date", SqlDbType.DateTime);
            this.ExecuteNonQuery(SQL_FOR_REMOVE_PRE_DATE, p0);
        }

        private const string SQL_FOR_UV_GROUP_BY_UNIQUE_VISITOR_KEY = @"SELECT 
            [T0].[DATE],
            COUNT([T0].[UNIQUEVISITORKEY]) AS [QTY]
            FROM (SELECT
            CONVERT(NCHAR(10), [IT].[DATETIME], 20) AS [DATE],
            [IT].[UniqueVisitorKey]
            FROM [KBM_MONITOR_LOGS] AS [IT]
            WHERE [IT].[UniqueVisitorKey] IS NOT NULL AND LEN([IT].[UniqueVisitorKey])>0
            AND [IT].[DATETIME]>=@start AND [IT].[DATETIME]<=@end
            GROUP BY CONVERT(NCHAR(10), [IT].[DATETIME], 20),[IT].[UniqueVisitorKey]
            )[T0]
            GROUP BY [T0].[DATE]
            ORDER BY [T0].[DATE] ASC";
        public IEnumerable<DailyUV> GetUvStatus(DateTime utcStart, DateTime utcEnd)
        {
            var lst = new List<DailyUV>();
            var p0 = new SqlParameter("@start", SqlDbType.DateTime);
            p0.Value = utcStart;
            var p1 = new SqlParameter("@end", SqlDbType.DateTime);
            p1.Value = utcEnd;
            var table = this.GetDatatable(SQL_FOR_UV_GROUP_BY_UNIQUE_VISITOR_KEY, p0, p1);
            DailyUV info = null;
            foreach (DataRow row in table.Rows)
            {
                info = new DailyUV();
                info.Date = DateTime.Parse(row["DATE"].ToString());
                info.Total = int.Parse(row["QTY"].ToString());
                lst.Add(info);
            }
            return lst;
        }

        private const string SQL_FOR_PV_GROUPYB_HOUR = @"SELECT 
            CONVERT(NCHAR(13), [IT].[DATETIME], 20)+':00' AS [DATE],
            COUNT(1) AS [TOTAL],
            SUM(CASE WHEN IT.[EXCEPTION] IS NULL THEN 0 ELSE 1 END) AS [ERROR]
            FROM [KBM_MONITOR_LOGS] AS [IT]
            WHERE [IT].[DATETIME]>=@start
            AND [IT].[DATETIME]<=@end
            GROUP BY CONVERT(NCHAR(13), [IT].[DATETIME], 20)+':00'
            ORDER BY CONVERT(NCHAR(13), [IT].[DATETIME], 20)+':00' ASC";
        public IEnumerable<HourlyPv> GetHourPvByDay(DateTime utcDate)
        {
            var lst = new List<HourlyPv>();
            var p0 = new SqlParameter("@start", SqlDbType.DateTime);
            p0.Value = utcDate.Date;
            var p1 = new SqlParameter("@end", SqlDbType.DateTime);
            p1.Value = new DateTime(utcDate.Year, utcDate.Month, utcDate.Day, 23, 59, 59);
            var table = this.GetDatatable(SQL_FOR_PV_GROUPYB_HOUR, p0, p1);
            HourlyPv info = null;
            foreach (DataRow row in table.Rows)
            {
                info = new HourlyPv();
                var dateHour = DateTime.Parse(row["DATE"].ToString());
                info.DateHour = new DateTime(dateHour.Year, dateHour.Month, dateHour.Day, dateHour.Hour, 0, 0, DateTimeKind.Utc);
                info.Total = int.Parse(row["TOTAL"].ToString());
                info.Error = int.Parse(row["ERROR"].ToString());
                lst.Add(info);
            }
            return lst;
        }

        private const string SQL_FOR_GROUP_BY_TOP_REFERRER = @"SELECT TOP {0}
            COUNT(1) as [QTY],
            [IT].[REFERRERDOMAIN] AS [DOMAIN]
            FROM KBM_MONITOR_LOGS [IT]
            WHERE [IT].[REFERRERDOMAIN] IS NOT NULL AND LEN([IT].[REFERRERDOMAIN])>0
            AND [IT].[DATETIME]>=@start AND [IT].[DATETIME]<=@end
            GROUP BY [IT].[REFERRERDOMAIN]
            ORDER BY [QTY] DESC";
        private const string SQL_FOR_GROUP_BY_REFERRER = @"SELECT
            COUNT(1) as [QTY],
            [IT].[REFERRERDOMAIN] AS [DOMAIN]
            FROM KBM_MONITOR_LOGS [IT]
            WHERE [IT].[REFERRERDOMAIN] IS NOT NULL AND LEN([IT].[REFERRERDOMAIN])>0
            AND [IT].[DATETIME]>=@start AND [IT].[DATETIME]<=@end
            GROUP BY [IT].[REFERRERDOMAIN]
            ORDER BY [QTY] DESC";
        public Dictionary<string, int> GroupByReferrer(DateTime utcStart, DateTime utcEnd, int? top)
        {
            var dict = new Dictionary<string, int>();
            var p0 = new SqlParameter("@start", SqlDbType.DateTime);
            p0.Value = utcStart;
            var p1 = new SqlParameter("@end", SqlDbType.DateTime);
            p1.Value = utcEnd;
            var table = this.GetDatatable(top.HasValue ? string.Format(SQL_FOR_GROUP_BY_TOP_REFERRER, top) : SQL_FOR_GROUP_BY_REFERRER,
                p0, p1);
            foreach (DataRow row in table.Rows)
            {
                dict.Add(row["DOMAIN"].ToString(), Convert.ToInt32(row["QTY"]));
            }
            return dict;
        }

        public IPagedList<ReferrerGroup> ReferrerGroups(int page,int pageSize,DateTime utcStart, DateTime utcEnd)
        {
            var query = this.CreateQuery()
                .Where(it => it.DateTime >= utcStart && it.DateTime <= utcEnd && it.Referrer != null)
                .GroupBy(it => it.Referrer)
                .Select(it => new ReferrerGroup
                {
                    Count = it.Count(),
                    Url = it.Key
                }).OrderByDescending(it => it.Count);
            return query.ToPagedList(page, pageSize);
        }

        public void Dispose()
        {
            if (this._dbContext != null)
            {
                this._dbContext.Dispose();
                this._dbContext = null;
            }
        }

        //此处添加析构函数，是因为考虑到LogProvider在IOC注入实例的生命周期中为ComponentLifeStyle.Transient，
        //IOC容器可能不会对其调用Dispose()方法，会导致其内部DbContext没有释放
        //但是，还不确定整个过程是否如想象中的这样，所以，暂时先注释掉
        //~LogProvider()
        //{
        //    if (this._dbContext != null)
        //    {
        //        this._dbContext.Dispose();
        //        this._dbContext = null;
        //    }
        //}

        public IPagedList<SearchKeyGroup> SearchKeyGroups(int page, int pageSize, DateTime utcStart, DateTime utcEnd)
        {
            var query = this.CreateQuery()
                .Where(it => it.DateTime >= utcStart && it.DateTime <= utcEnd && it.SearchKey != null)
                .GroupBy(it => it.SearchKey)
                .Select(it => new SearchKeyGroup
                {
                    Count = it.Count(),
                    Keyword = it.Key
                }).OrderByDescending(it => it.Count);
            return query.ToPagedList(page, pageSize);
        }
    }
}