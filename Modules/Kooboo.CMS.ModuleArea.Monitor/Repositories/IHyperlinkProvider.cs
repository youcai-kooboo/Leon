using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.ModuleArea.Monitor.Models;

namespace Kooboo.CMS.ModuleArea.Monitor.Repositories
{
    public interface IHyperlinkProvider:IDisposable
    {
        void Update(Hyperlink obj);

        Hyperlink QueryById(string id);
        void Add(Hyperlink link);
        void AddRange(IEnumerable<Hyperlink> links);
        void RemoveAll();
        IQueryable<Hyperlink> CreateQuery();
        HealthStatistics GetStatistics();

        IEnumerable<Hyperlink> Get(string[] ids);
        Dictionary<int, int> GroupByStatus();
        IPagedList<Hyperlink> Search(HyperlinkQuery search);
        IEnumerable<Hyperlink> GetHyperlinksByDepth(int depth);
    }
}
