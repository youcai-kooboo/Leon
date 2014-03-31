using System;
using System.Collections.Generic;
using System.Linq;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Toolkit;
using Kooboo.Web.Mvc.Paging;

namespace Leon.Business
{
    public class LeonServiceBase<T> : Kooboo.CMS.Toolkit.Services.ServiceBase<T>
        where T : TextContent, new()
    {
        public LeonServiceBase(): base()
        {
        }

        public LeonServiceBase(Repository repository): base(repository)
        {
        }

        public virtual IEnumerable<T> GetContentsByCategory(string categoryFolderName, string categoryUUID)
        {
            var query = this.CreateQuery()
                .Published()
                .WhereCategory(categoryFolderName, categoryUUID)
                .DefaultOrder()
                .MapTo<T>();

            return query;
        }

        public virtual IEnumerable<T> GetEmbeddedContents(string parentfolderName, string parentUUID)
        {
            var query = this.CreateQuery()
                .Published()
                .WhereEquals(SystemFieldNames.ParentUUID,parentUUID)
                .DefaultOrder()
                .MapTo<T>();

            return query;
        }

        public virtual PagedList<TextContent> GetPagedList(
            int pageIndex,
            int pageSize,
            string whereClause = null,
            string orderField = null,
            OrderDirection orderDirection = OrderDirection.Descending)
        {
            var query = this.CreateQuery().Published();
            if (!String.IsNullOrEmpty(whereClause))
            {
                query = query.Where(whereClause);
            }

            if (!String.IsNullOrEmpty(orderField))
            {
                if (orderDirection == OrderDirection.Descending)
                {
                    query = query.OrderByDescending(orderField);
                }
                else
                {
                    query = query.OrderBy(orderField);
                }
            }
            else
            {
                query = query.DefaultOrder();
            }

            return query.ToPagedList(pageIndex, pageSize);
        }

        public virtual IEnumerable<T> SearchSql(string queryText)
        {
            return this.Repository.ExecuteQuery(queryText).Select(it => (new TextContent(it)).MapTo<T>());
        }
    }
}
