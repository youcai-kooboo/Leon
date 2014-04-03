using System.Data.SqlClient;
using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.ModuleArea.Monitor.Models;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;

namespace Kooboo.CMS.ModuleArea.Monitor.Repositories.Default
{
    public class HyperlinkProvider : IHyperlinkProvider
    {
        private string _siteName;
        public HyperlinkProvider()
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

        private MonitorDbContext _dbContext;

        private string GenerateId()
        {
            return Guid.NewGuid().ToString();
        }

        public void Add(Hyperlink link)
        {
            link.Id = GenerateId();
            this._dbContext.Hyperlinks.Add(link);
            this._dbContext.SaveChanges();
        }

        public void AddRange(IEnumerable<Hyperlink> links)
        {
            if (links.Any())
            {
                foreach (var item in links)
                {
                    item.Id = GenerateId();
                    this._dbContext.Hyperlinks.Add(item);
                }
                this._dbContext.SaveChanges();
            }
        }

        public void RemoveAll()
        {
            this._dbContext.Database.ExecuteSqlCommand("DELETE FROM [KBM_Monitor_Hyperlinks]");
            this._dbContext.SaveChanges();
        }

        public IQueryable<Hyperlink> CreateQuery()
        {
            return this._dbContext.Hyperlinks.AsQueryable();
        }

        public Hyperlink QueryById(string id)
        {
            return this.CreateQuery().FirstOrDefault(it => it.Id.Equals(id));
        }

        public void Update(Hyperlink obj)
        {
            this._dbContext.Hyperlinks.Attach(obj);
            this._dbContext.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            this._dbContext.SaveChanges();
        }

        private const string SQL_FOR_STATIISTICS = @"SELECT 
SUM(CASE WHEN T1.[RESOURCETYPE]='Page' THEN 1 ELSE 0 END) AS TotalPage,
SUM(CASE WHEN T1.[RESOURCETYPE]='JavaScript' THEN 1 ELSE 0 END) AS TotalJs,
SUM(CASE WHEN T1.[RESOURCETYPE]='Css' THEN 1 ELSE 0 END) AS TotalCss,
SUM(CASE WHEN T1.[RESOURCETYPE]='Image' THEN 1 ELSE 0 END) AS TotalImage,

SUM(CASE WHEN T1.[RESOURCETYPE]='Page' AND T1.[UTCVISITDATE] IS NOT NULL AND (T1.[STATUS]= 0 OR T1.[STATUS]>=400) THEN 1 ELSE 0 END) AS ErrorPage,
SUM(CASE WHEN T1.[RESOURCETYPE]='JavaScript' AND T1.[UTCVISITDATE] IS NOT NULL AND (T1.[STATUS]= 0 OR T1.[STATUS]>=400) THEN 1 ELSE 0 END) AS ErrorJs,
SUM(CASE WHEN T1.[RESOURCETYPE]='Css' AND T1.[UTCVISITDATE] IS NOT NULL AND (T1.[STATUS]= 0 OR T1.[STATUS]>=400) THEN 1 ELSE 0 END) AS ErrorCss,
SUM(CASE WHEN T1.[RESOURCETYPE]='Image' AND T1.[UTCVISITDATE] IS NOT NULL AND (T1.[STATUS]= 0 OR T1.[STATUS]>=400) THEN 1 ELSE 0 END) AS ErrorImage,

SUM(CASE WHEN T1.[RESOURCETYPE]='Page' AND T1.[UTCVISITDATE] IS NOT NULL THEN 1 ELSE 0 END) AS VisitedPage,
SUM(CASE WHEN T1.[RESOURCETYPE]='JavaScript' AND T1.[UTCVISITDATE] IS NOT NULL THEN 1 ELSE 0 END) AS VisitedJs,
SUM(CASE WHEN T1.[RESOURCETYPE]='Css' AND T1.[UTCVISITDATE] IS NOT NULL THEN 1 ELSE 0 END) AS VisitedCss,
SUM(CASE WHEN T1.[RESOURCETYPE]='Image' AND T1.[UTCVISITDATE] IS NOT NULL THEN 1 ELSE 0 END) AS VisitedImage
FROM KBM_MONITOR_HYPERLINKS T1";
        public HealthStatistics GetStatistics()
        {
            var table = this.GetDatatable(SQL_FOR_STATIISTICS);
            var statistics = new HealthStatistics();
            if (table.Rows.Count > 0)
            {
                DataRow row = table.Rows[0];
                if (!(row["TotalPage"] is DBNull))
                {
                    statistics.TotalPage = Convert.ToInt32(row["TotalPage"]);
                }
                if (!(row["TotalJs"] is DBNull))
                {
                    statistics.TotalJs = Convert.ToInt32(row["TotalJs"]);
                }
                if (!(row["TotalImage"] is DBNull))
                {
                    statistics.TotalImage = Convert.ToInt32(row["TotalImage"]);
                }
                if (!(row["TotalCss"] is DBNull))
                {
                    statistics.TotalCss = Convert.ToInt32(row["TotalCss"]);
                }
                if (!(row["ErrorPage"] is DBNull))
                {
                    statistics.ErrorPage = Convert.ToInt32(row["ErrorPage"]);
                }
                if (!(row["ErrorJs"] is DBNull))
                {
                    statistics.ErrorJs = Convert.ToInt32(row["ErrorJs"]);
                }
                if (!(row["ErrorImage"] is DBNull))
                {
                    statistics.ErrorImage = Convert.ToInt32(row["ErrorImage"]);
                }
                if (!(row["ErrorCss"] is DBNull))
                {
                    statistics.ErrorCss = Convert.ToInt32(row["ErrorCss"]);
                }

                if (!(row["VisitedCss"] is DBNull))
                {
                    statistics.VisitedCss = Convert.ToInt32(row["VisitedCss"]);
                }
                if (!(row["VisitedImage"] is DBNull))
                {
                    statistics.VisitedImage = Convert.ToInt32(row["VisitedImage"]);
                }
                if (!(row["VisitedJs"] is DBNull))
                {
                    statistics.VisitedJs = Convert.ToInt32(row["VisitedJs"]);
                }
                if (!(row["VisitedPage"] is DBNull))
                {
                    statistics.VisitedPage = Convert.ToInt32(row["VisitedPage"]);
                }
            }
            return statistics;
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

        public void Dispose()
        {
            if (this._dbContext != null)
            {
                this._dbContext.Dispose();
                this._dbContext = null;
            }
        }


        public IEnumerable<Hyperlink> Get(string[] ids)
        {
            var query = this.CreateQuery();
            if (ids.Any())
            {
                query = query.Where(it => ids.Contains(it.Id));
            }
            return query.ToList();
        }

        public Dictionary<int, int> GroupByStatus()
        {
            return this.CreateQuery()
                .GroupBy(it => it.Status, it => it.Url)
                .Select(it => new
                {
                    Status = it.Key,
                    Total = it.Count()
                }).ToDictionary(it => it.Status, it => it.Total);
        }

        public IPagedList<Hyperlink> Search(HyperlinkQuery search)
        {
            PagedList<Hyperlink> lst = null;
            var query = this.CreateQuery();
            if (!string.IsNullOrEmpty(search.ResourceType))
            {
                query = query.Where(it => it.ResourceType.Equals(search.ResourceType, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(search.Url))
            {
                query = query.Where(it => it.Url.Contains(search.Url));
            }
            if (search.Status.HasValue && search.Status.Value > 0)
            {
                query = query.Where(it => it.Status == search.Status.Value);
            }
            if (!string.IsNullOrEmpty(search.Type))
            {
                if (string.Equals("UnVisit", search.Type, StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(it => it.UtcVisitDate == null);
                }
                else if (string.Equals("Error", search.Type, StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(it => (it.Status == 0 || it.Status >= 400) && it.UtcVisitDate != null);
                }
                else if (string.Equals("Normal", search.Type, StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(it => it.Status > 0 && it.Status < 400);
                }
            }
            var prop = Hyperlink.GetProperty(search.SortField);
            if (prop != null)
            {
                string methodName = "OrderBy";//default is asc
                if (!string.IsNullOrEmpty(search.SortDir) && search.SortDir.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    methodName = "OrderByDescending";
                }
                var modelType = typeof(Hyperlink);
                var parameter = Expression.Parameter(modelType, "it");
                var propertyAccess = Expression.MakeMemberAccess(parameter, prop);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);
                var resultExpression = Expression.Call(typeof(Queryable), methodName, new Type[] { modelType, prop.PropertyType }, query.Expression, Expression.Quote(orderByExpression));
                query = query.Provider.CreateQuery<Hyperlink>(resultExpression);
            }
            else
            {
                query = query.OrderBy(it => it.Depth).OrderBy(it => it.UtcCreationDate);
            }
            lst = query.ToPagedList(search.Page ?? 1, search.PageSize ?? 50);
            return lst;
        }

        public IEnumerable<Hyperlink> GetHyperlinksByDepth(int depth)
        {
            return this.CreateQuery()
               .Where(it => it.Depth == depth).ToList();
        }
    }
}