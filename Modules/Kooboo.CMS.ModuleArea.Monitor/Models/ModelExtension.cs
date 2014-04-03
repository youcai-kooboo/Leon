using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Monitor.Models
{
    public static class ModelExtension
    {
        public static int AsInt32(this object obj, int def)
        {
            try
            {
               return Convert.ToInt32(obj);
            }
            catch { }
            return def;
        }
    }
}
