using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Mvc;
using Kooboo.CMS.Content.EventBus;
using Kooboo.CMS.Toolkit.Modules;
//using Kooboo.CMS.Toolkit.Intercept;
using Leon.Modules.Management.Services;
using Leon.Modules.Management.Controllers;
using Kooboo.CMS.Common.Runtime.Dependency;

//[assembly: System.Web.PreApplicationStartMethod(typeof(Leon.Modules.Management.ManagementStartup), "Start")]

namespace Leon.Modules.Management
{
    public class ManagementStartup : IDependencyRegistrar
    {
        //public static void Start()
        //{
        //    ModuleInitializers.Initializers.Add(new ManagementInitializer());

        //    EventBus.Subscribers.Add(new Subscribers.ClearCacheSubscriber());

        //    // Auto generated meta
        //    ApplicationInitialization.RegisterInitializerMethod(delegate()
        //    {
        //        Kooboo.CMS.Sites.ControllerTypeCache.RegisterController(
        //            "Kooboo.CMS.Web.Areas.Sites.Controllers.PageController",
        //            typeof(PageController));

        //        Kooboo.CMS.Sites.ControllerTypeCache.RegisterController(
        //            "Kooboo.CMS.Web.Areas.Contents.Controllers.TextContentController",
        //            typeof(TextContentController));
        //    }, 100);

        //    // register action for purge CDN
        //    Kooboo.CMS.Toolkit.Intercept.CDNResources.Subscription.Instance.Register((string fileName, InterceptActionType action) => {
        //        //if (action == Kooboo.CMS.Toolkit.Intercept.ActionType.Update) {
        //            Leon.Modules.Management.ManagementContext.Current.TurbobytesCDNService.Purge(fileName);
        //        //}
        //    });
        //}
        public int Order
        {
            get { return 100; }
        }


        public void Register(IContainerManager containerManager, Kooboo.CMS.Common.Runtime.ITypeFinder typeFinder)
        {
            ModuleInitializers.Initializers.Add(new ManagementInitializer());

            EventBus.Subscribers.Add(new Subscribers.ClearCacheSubscriber());
            
            //// register action for purge CDN
            //Kooboo.CMS.Toolkit.Intercept.CDNResources.Subscription.Instance.Register((string fileName, InterceptActionType action) =>
            //{
            //    //if (action == Kooboo.CMS.Toolkit.Intercept.ActionType.Update) {
            //    Leon.Modules.Management.ManagementContext.Current.TurbobytesCDNService.Purge(fileName);
            //    //}
            //});
        }
    }
}