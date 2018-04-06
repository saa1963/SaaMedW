using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.Service
{
    public class LogonService : ILogonService
    {
        //private static pdbUser m_user;
        public bool RegisterUser(string login, string password)
        {
            var connstr = ConfigurationManager.ConnectionStrings["TKPBSec.Properties.Settings.conn"].
                ConnectionString.Replace("%password%", password)
                .Replace("%login%", login)
                .Replace("%database%", database)
                .Replace("%server%", server);
            clsGlobal.Default.ConnectionString = connstr;
            try
            {
                var dl = (IDataLayer)ServiceLocator.Instance.GetService(Type.GetType("TKPBSec.IDataLayer"));
                clsGlobal.Default.CurrentUser = dl.getUserBySysname(login, DateTime.Today);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
    }
}
