using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Repositories.Default
{
    public class RepositoryFactory : Kooboo.CMS.Common.Runtime.Dependency.IDependencyRegistrar
    {
        public int Order
        {
            get 
            { 
                return 0; 
            }
        }

        public void Register(Common.Runtime.Dependency.IContainerManager containerManager, Common.Runtime.ITypeFinder typeFinder)
        {
            //此处将实例生命周期 注册为Transient，是因为有些任务是在后台异步线程中进行的，
            //如果注册为InRquestScope，在Http请求结束时，实例会被IOC容器显示调用Dispose()，
            //而此时异步线程还没有结束，导致其内部的DbContext无法继续使用
            containerManager.AddComponent<ILogProvider, LogProvider>("", ComponentLifeStyle.Transient);
            containerManager.AddComponent<IHyperlinkProvider, HyperlinkProvider>("", ComponentLifeStyle.Transient);
        }
    }
}
