using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atol.Drivers10.Fptr;
using log4net;

namespace SaaMedW.Service
{
    public class AtolService: IKkm, IDisposable
    {
        private IFptr fptr = null;
        ILog log;

        public AtolService()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
        }

        ~AtolService()
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
                    throw new Exception("Параметр Настройки_ФР равен null");
                fptr = new Fptr();
                fptr.setSettings((string)config);
                rt = true;
            }
            catch (Exception e)
            {
                var msg = "Ошибка инициализации драйвера ККТ";
                log.Error(msg, e);
                fptr = null;
            }
            return rt;
        }

        private bool Open()
        {
            bool rt = false;
            try
            {
                fptr.open();
                if (!fptr.isOpened())
                    throw new Exception("Невозможно установить соединение с кассой.");

                rt = true;
            }
            catch (Exception e)
            {
                var msg = "Ошибка открытия кассы.";
                log.Error(msg, e);
            }
            return rt;
        }

        private void Close()
        {
            if (fptr == null) return;
            if (!fptr.isOpened()) return;
            fptr.close();
        }

        private bool RegisterUser()
        {
            bool rt = false;
            try
            {
                if (fptr == null)
                    throw new Exception("Не проинициализирован рабочий экземпляр драйвера.");
                fptr.setParam(1021, Global.Source.RUser.Fio);
                fptr.setParam(1203, Global.Source.RUser.Inn);
                if (fptr.operatorLogin() != 0) throw new Exception();
                rt = true;
            }
            catch (Exception e)
            {
                var msg = "Ошибка регистрации кассира";
                log.Error(msg, e);
            }
            return rt;
        }

        public bool Back(decimal sm)
        {
            throw new NotImplementedException();
        }

        public void Destroy()
        {
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (fptr != null)
                {
                    if (fptr.isOpened())
                    {
                        fptr.close();
                    }
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

        public static string ShowProperties()
        {
            Fptr fptr = null;
            try
            {
                fptr = new Fptr();
                int res = fptr.showProperties(Constants.LIBFPTR_GUI_PARENT_NATIVE, (IntPtr)0);
                if (res == 0)
                    return fptr.getSettings();
                else
                    return null;
            }
            finally
            {
                if (fptr != null)
                    fptr.destroy();
            }
        }

        public bool ZReport()
        {
            throw new NotImplementedException();
        }

        public string Model => "АТОЛ";
    }
}
