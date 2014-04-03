using Kooboo.CMS.Common.Runtime;
using Leon.Business.Jobs;

namespace Leon.Business
{
    public class StartupTask : IStartupTask {

        public void Execute() {

            //Do jobs
            new LeonJob().AttachJob(1 * 60 * 1000, true);

            //Scan the page search
            new PageSearchJob().AttachJob(1 * 60 * 1000, true);
        }

        public int Order {
            get {
                return 1000;
            }
        }
    }
}
