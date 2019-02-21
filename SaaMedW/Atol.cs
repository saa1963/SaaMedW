using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atol.Drivers10.Fptr;
using log4net;

namespace SaaMedW
{
    public class Atol: IKkm, IDisposable
    {
        private IFptr fptr = null;
        ILog log;

        public Atol()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
        }

        ~Atol()
        {
            Dispose(false);
        }

        public bool IsInitialized => fptr != null;

        public bool Init()
        {
            bool rt = false;
            string config = Options.GetParameter<string>(enumParameterType.Настройки_ФР);
            try
            {
                if (config == null)
                {
                    throw new Exception("Параметр Настройки_ФР равен null");
                }
                fptr = new Fptr();
                fptr.setSettings((string)config);
                rt = true;
            }
            catch (Exception e)
            {
                var msg = "Ошибка инициализации драйвера ККМ";
                log.Error(msg, e);
                fptr = null;
            }
            return rt;
        }

        public bool Back(decimal sm)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (fptr != null)
                {
                    fptr.destroy();
                    fptr = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Register(decimal sm)
        {
            return true;
        }

        public string ShowProperties()
        {
            int res = fptr.showProperties(Constants.LIBFPTR_GUI_PARENT_NATIVE, (IntPtr)0);
            if (res == 0)
                return fptr.getSettings();
            else
                return null;
        }

        public bool ZReport()
        {
            throw new NotImplementedException();
        }
    }
}
