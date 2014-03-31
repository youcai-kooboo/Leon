using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Form.Html.Controls;
using Kooboo.CMS.Toolkit;
using Leon.Business;
using Leon.KB.Extensions;

namespace Leon.Business.Jobs
{
    public class LeonJob: CustomJobBase
    {
        public override void Execute(object executionState)
        {
            //if (!(bool)executionState || (DateTime.UtcNow.Hour >= 5 && DateTime.UtcNow.Hour <= 6))
            //{
            //    return;
            //}
            Time = DateTime.Now.ToString();
        }

        public static string _time;
        public static string Time
        {
            get { return String.IsNullOrEmpty(_time) ? DateTime.Now.ToString() : _time; }
            set { _time = value; }
        }
    }
}
