using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Sites.Controllers;

namespace Leon.KB.Extensions
{
    public class DependencyRegistrar:IDependencyRegistrar
    {
        private class ResolvingObserver:IResolvingObserver
        {
            public object OnResolved(object resolvedObject)
            {
                if (resolvedObject is PageController)
                {
                    return new Pages.PageController();
                }

                return resolvedObject;
            }

            public int Order
            {
                get { return 1000; }
            }
        }

        public int Order
        {
            get { return 1000; }
        }

        public void Register(IContainerManager containerManager, Kooboo.CMS.Common.Runtime.ITypeFinder typeFinder)
        {
            containerManager.AddResolvingObserver(new ResolvingObserver());
        }
    }
}
