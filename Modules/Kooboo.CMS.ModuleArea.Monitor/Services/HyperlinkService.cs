using Kooboo.CMS.Common.Runtime;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.ModuleArea.Monitor.Models;
using Kooboo.CMS.ModuleArea.Monitor.Repositories;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Services
{
    [Dependency(typeof(HyperlinkService), ComponentLifeStyle.Transient)]
    public class HyperlinkService:IDisposable
    {
        private IHyperlinkProvider _hyperlinkProvider;
        public HyperlinkService(IHyperlinkProvider provider)
        {
            this._hyperlinkProvider = provider;
        }

        public void Save(Hyperlink entity)
        {
            if (string.IsNullOrEmpty(entity.Id))
            {
                _hyperlinkProvider.Add(entity);
            }
            else
            {
                _hyperlinkProvider.Update(entity);
            }
        }

        public void Save(IEnumerable<Hyperlink> links)
        {
            _hyperlinkProvider.AddRange(links);
        }

        public HealthStatistics GetStatistics()
        {
            return this._hyperlinkProvider.GetStatistics();
        }

        public IEnumerable<Hyperlink> GetHyperlinksByDepth(int depth)
        {
            return this._hyperlinkProvider.GetHyperlinksByDepth(depth);
        }

        public IPagedList<Hyperlink> Search(HyperlinkQuery search)
        {
            return this._hyperlinkProvider.Search(search);
        }

        public void Clear()
        {
            _hyperlinkProvider.RemoveAll();
        }

        public IEnumerable<Hyperlink> Get(string[] ids)
        {
            return this._hyperlinkProvider.Get(ids);
        }

        public Dictionary<int, int> GroupByStatus()
        {
            return this._hyperlinkProvider.GroupByStatus();
        }

        public void Dispose()
        {
            if (this._hyperlinkProvider != null)
            {
                this._hyperlinkProvider.Dispose();
            }
        }
    }
}