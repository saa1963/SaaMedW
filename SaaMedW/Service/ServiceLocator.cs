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
        private Dictionary<Type, object> m_lst;

        ServiceLocator()
        {
            m_lst = new Dictionary<Type, object>();
            m_lst.Add(typeof(ILocalStorage), Activator.CreateInstance(typeof(SaaMedW.Service.LocalStorage)));
            m_lst.Add(typeof(ILogonService), Activator.CreateInstance(typeof(SaaMedW.Service.LogonService)));
            m_lst.Add(typeof(IAccounts), Activator.CreateInstance(typeof(SaaMedW.Service.AccountsService)));
            m_lst.Add(typeof(IKkm), Activator.CreateInstance(typeof(SaaMedW.Service.AtolService)));
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
                return m_lst[iface];
            }
            catch (Exception)
            {
                return null;
            }
        }

        public T GetService<T>()
        {
            var iface = typeof(T);
            try
            {
                return (T)m_lst[iface];
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
