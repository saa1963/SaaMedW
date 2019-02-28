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

        /// <summary>
        /// Пробить чек
        /// </summary>
        /// <param name="sm">имя, кол-во, цена</param>
        /// <param name="emailOrPhone"></param>
        /// <param name="electron"></param>
        /// <returns></returns>
        public bool Register(List<Tuple<string, int, decimal>> sm, string emailOrPhone, bool electron)
        {
            bool rt = false;
            try
            {
                var taxSystem = Options.GetParameter<enTaxSystem>(enumParameterType.Система_налогообложения);
                if (fptr.open() < 0) throw AtolException();
                fptr.setParam(1021, Global.Source.RUser.Fio);
                fptr.setParam(1203, Global.Source.RUser.Inn);
                if (fptr.operatorLogin() < 0) throw AtolException();

                // Открытие чека
                fptr.setParam(Constants.LIBFPTR_PARAM_RECEIPT_TYPE, Constants.LIBFPTR_RT_SELL);
                if (electron)
                {
                    fptr.setParam(Constants.LIBFPTR_PARAM_RECEIPT_ELECTRONICALLY, true);
                    fptr.setParam(1008, emailOrPhone);
                }
                if (fptr.openReceipt() < 0) throw AtolException();

                foreach (var o in sm)
                {
                    var ch_name = o.Item1;
                    var ch_quantity = o.Item2;
                    var ch_price = o.Item3;
                    // Регистрация позиции
                    fptr.setParam(Constants.LIBFPTR_PARAM_COMMODITY_NAME, ch_name);
                    fptr.setParam(Constants.LIBFPTR_PARAM_PRICE, Convert.ToDouble(ch_price));
                    fptr.setParam(Constants.LIBFPTR_PARAM_QUANTITY, ch_quantity);
                    if (taxSystem == enTaxSystem.Общая)
                    {
                        fptr.setParam(Constants.LIBFPTR_PARAM_TAX_TYPE, Constants.LIBFPTR_TAX_VAT20);
                    }
                    fptr.setParam(1212, 1);
                    fptr.setParam(1214, 7);
                    fptr.registration();
                }

                // Регистрация итога (отрасываем копейки)
                fptr.setParam(Constants.LIBFPTR_PARAM_SUM, 369.0);
                fptr.receiptTotal();

                // Оплата наличными
                fptr.setParam(Constants.LIBFPTR_PARAM_PAYMENT_TYPE, Constants.LIBFPTR_PT_CASH);
                fptr.setParam(Constants.LIBFPTR_PARAM_PAYMENT_SUM, 1000);
                fptr.payment();

                // Закрытие чека
                fptr.closeReceipt();
                rt = true;
            }
            catch (Exception e)
            {
                var msg = "Ошибка закрытия смены.";
                log.Error(msg, e);
            }
            if (fptr.isOpened()) fptr.close();
            return rt;
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
            bool rt = false;
            try
            {
                if (fptr.open() < 0) throw AtolException();
                fptr.setParam(1021, Global.Source.RUser.Fio);
                fptr.setParam(1203, Global.Source.RUser.Inn);
                if (fptr.operatorLogin() < 0) throw AtolException();

                fptr.setParam(Constants.LIBFPTR_PARAM_REPORT_TYPE, Constants.LIBFPTR_RT_CLOSE_SHIFT);
                if (fptr.report() < 0) throw AtolException();

                if (fptr.checkDocumentClosed() < 0) throw AtolException();

                if (!fptr.getParamBool(Constants.LIBFPTR_PARAM_DOCUMENT_CLOSED))
                {
                    // Документ не закрылся. Требуется его отменить (если это чек) и сформировать заново
                    throw new Exception("Смена не закрыта.");
                }

                if (!fptr.getParamBool(Constants.LIBFPTR_PARAM_DOCUMENT_PRINTED))
                {
                    // Можно сразу вызвать метод допечатывания документа, он завершится с ошибкой, если это невозможно
                    log.Warn("Смена закрыта. Документ не допечатан.");
                }

                rt = true;
            }
            catch (Exception e)
            {
                var msg = "Ошибка закрытия смены.";
                log.Error(msg, e);
            }
            if (fptr.isOpened()) fptr.close();
            return rt;
        }

        private Exception AtolException()
        {
            return new Exception($"Код ошибки - {fptr.errorCode()} {fptr.errorDescription()}");
        }

        public string Model => "АТОЛ";
    }
}
