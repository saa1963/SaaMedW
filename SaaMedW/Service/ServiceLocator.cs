using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.Service
{
    public class ServiceLocator
    {
        static ServiceLocator instance = null;
        static readonly object padlock = new object();
        private Dictionary<Type, Type> m_lst;

        ServiceLocator()
        {
            m_lst = new Dictionary<Type, Type>();
            m_lst.Add(typeof(ILocalStorage), typeof(SaaMedW.Service.LocalStorage));
            m_lst.Add(typeof(ILogonService), typeof(SaaMedW.Service.LogonService));
            //m_lst.Add(typeof(IDataLayer), typeof(TKPBSec.Service.ADODataService));
            //m_lst.Add(typeof(ISendFile), typeof(TKPBSec.Service.SendFileService));
        }

        public static ServiceLocator Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ServiceLocator();
                    }
                    return instance;
                }
            }
        }

        public object GetService(Type iface)
        {
            try
            {
                return Activator.CreateInstance(m_lst[iface]);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
