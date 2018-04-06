using SaaMedW.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.Service
{
    public class LocalStorage : ILocalStorage
    {
        public string GetLoginName(string username)
        {
            return Settings.Default.login ?? "";
        }

        public void SetLoginName(string username, string loginname)
        {
            Settings.Default.login = loginname;
            Settings.Default.Save();
        }
    }
}
