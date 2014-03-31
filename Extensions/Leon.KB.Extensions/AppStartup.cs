using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: System.Web.PreApplicationStartMethod(typeof(Leon.KB.Extensions.AppStartup), "Main")]
namespace Leon.KB.Extensions
{
    public class AppStartup
    {
        public static void Main()
        {
            // Removing technical and detailed version information from the HTTP header.
            DynamicModuleUtility.RegisterModule(typeof(CloakHttpHeaderModule));
        }
    }
}
