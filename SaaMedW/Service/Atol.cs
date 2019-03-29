using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                log.Info("Экземпляр драйвера инициализирован");
                fptr.setSettings((string)config);
                if (fptr.open() != 0) throw AtolException();
                log.Info("Соединение с ККТ открыто");
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
                        log.Info("Соединение с ККТ закрыто");
                    }
                    fptr.destroy();
                    log.Info("Экземпляр драйвера деинициализирован");
                    fptr = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void OpenConnection()
        {
            fptr.setParam(Constants.LIBFPTR_PARAM_DATA_TYPE, Constants.LIBFPTR_DT_SHORT_STATUS);
            if (fptr.queryData() < 0)
            {
                var errCode = fptr.errorCode();
                if (errCode == Constants.LIBFPTR_ERROR_CONNECTION_DISABLED
                    || errCode == Constants.LIBFPTR_ERROR_NO_CONNECTION)
                {
                    if (fptr.open() < 0) throw AtolException();
                }
                else
                {
                    throw AtolException();
                }
            }
            fptr.setParam(1021, Global.Source.RUser.Fio);
            //fptr.setParam(1203, Global.Source.RUser.Inn);
            if (fptr.operatorLogin() < 0) throw AtolException();
        }

        /// <summary>
        /// Пробить чек
        /// </summary>
        /// <param name="sm">имя, кол-во, цена</param>
        /// <param name="emailOrPhone"></param>
        /// <param name="electron"></param>
        /// <returns></returns>
        public bool Register(List<Tuple<string, int, decimal>> uslugi, 
            decimal oplata, enumPaymentType vidOplata, 
            string emailOrPhone, bool electron)
        {
            bool rt = false;
            try
            {
                OpenConnection();

                // Открытие чека
                fptr.setParam(Constants.LIBFPTR_PARAM_RECEIPT_TYPE, Constants.LIBFPTR_RT_SELL);
                if (electron)
                {
                    fptr.setParam(Constants.LIBFPTR_PARAM_RECEIPT_ELECTRONICALLY, true);
                    fptr.setParam(1008, emailOrPhone);
                }
                if (fptr.openReceipt() < 0) throw AtolException();

                foreach (var o in uslugi)
                {
                    var ch_name = o.Item1;
                    var ch_quantity = o.Item2;
                    var ch_price = o.Item3;
                    // Регистрация позиции
                    fptr.setParam(Constants.LIBFPTR_PARAM_COMMODITY_NAME, ch_name);
                    fptr.setParam(Constants.LIBFPTR_PARAM_PRICE, Convert.ToDouble(ch_price));
                    fptr.setParam(Constants.LIBFPTR_PARAM_QUANTITY, ch_quantity);
                    fptr.setParam(Constants.LIBFPTR_PARAM_TAX_TYPE, Constants.LIBFPTR_TAX_NO);
                    fptr.setParam(1214, 4); // признак способа расчета (в случае 4 - необязателен)
                    fptr.registration();
                }
                // Оплата наличными
                fptr.setParam(Constants.LIBFPTR_PARAM_PAYMENT_TYPE, Constants.LIBFPTR_PT_CASH);
                fptr.setParam(Constants.LIBFPTR_PARAM_PAYMENT_SUM, 1000);
                fptr.payment();

                // Закрытие чека
                fptr.closeReceipt();

                CheckDocumentClosed(true);

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
                OpenConnection();

                fptr.setParam(Constants.LIBFPTR_PARAM_REPORT_TYPE, Constants.LIBFPTR_RT_CLOSE_SHIFT);
                if (fptr.report() < 0) throw AtolException();

                CheckDocumentClosed();

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

        private Exception AtolException(string msg = "")
        {
            return new Exception($"{msg} Код ошибки - {fptr.errorCode()} {fptr.errorDescription()}");
        }

        public bool NumberOfUnsentDocuments(out uint unsentCount, out DateTime timeOfFirstUnsentDocument)
        {
            unsentCount = 0;
            timeOfFirstUnsentDocument = DateTime.MinValue;
            bool rt = false;
            try
            {
                if (fptr.open() < 0) throw AtolException();
                fptr.setParam(Constants.LIBFPTR_PARAM_FN_DATA_TYPE, Constants.LIBFPTR_FNDT_OFD_EXCHANGE_STATUS);
                fptr.fnQueryData();
                unsentCount = fptr.getParamInt(Constants.LIBFPTR_PARAM_DOCUMENTS_COUNT);
                timeOfFirstUnsentDocument = fptr.getParamDateTime(Constants.LIBFPTR_PARAM_DATE_TIME);
                fptr.close();
                rt = true;
            }
            catch (Exception e)
            {
                var msg = "Ошибка получения информации о неотправленных документах.";
                log.Error(msg, e);
            }
            return rt;
        }

        public bool OpenShift()
        {
            bool rt = false;
            try
            {
                OpenConnection();

                if (fptr.openShift() != 0) throw AtolException();

                CheckDocumentClosed();

                rt = true;
            }
            catch (Exception e)
            {
                var msg = "Ошибка открытия смены.";
                log.Error(msg, e);
            }
            return rt;
        }

        private void CheckDocumentClosed(bool IsRegistration = false)
        {
            while (fptr.checkDocumentClosed() < 0)
            {
                var msg = String.Join("\r\n",
                "Невозможно проверить статус закрытого документа",
                $"Код ошибки - {fptr.errorCode()} {fptr.errorDescription()}",
                "Попробовать еще?");
                if (MessageBox.Show(msg, "", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                {
                    throw AtolException();
                }
            }
            if (!fptr.getParamBool(Constants.LIBFPTR_PARAM_DOCUMENT_CLOSED))
            {
                // Документ не закрылся. Требуется его отменить (если это чек) и сформировать заново
                if (IsRegistration)
                {
                    fptr.cancelReceipt();
                }
                throw AtolException("Constants.LIBFPTR_PARAM_DOCUMENT_CLOSED == false");
            }

            if (!fptr.getParamBool(Constants.LIBFPTR_PARAM_DOCUMENT_PRINTED))
            {
                // Можно сразу вызвать метод допечатывания документа, он завершится с ошибкой, если это невозможно
                while (fptr.continuePrint() < 0)
                {
                    // Если не удалось допечатать документ - показать пользователю ошибку и попробовать еще раз.
                    //Console.WriteLine(String.Format("Не удалось напечатать документ (Ошибка \"{0}\"). Устраните неполадку и повторите.", fptr.errorDescription()));
                    var msg = String.Join("\r\n",
                    "Ошибка при допечатывании документа.",
                    $"Код ошибки - {fptr.errorCode()} {fptr.errorDescription()}",
                    "Попробовать еще?");
                    if (MessageBox.Show(msg, "", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                    {
                        throw AtolException();
                    }
                }
            }
        }

        public int GetNumShift()
        {
            int rt = -3;
            try
            {
                OpenConnection();

                fptr.setParam(Constants.LIBFPTR_PARAM_DATA_TYPE, Constants.LIBFPTR_DT_STATUS);
                if (fptr.queryData() < 0) throw AtolException();
                uint shiftState = fptr.getParamInt(Constants.LIBFPTR_PARAM_SHIFT_STATE);
                if (shiftState == Constants.LIBFPTR_SS_CLOSED)
                {
                    rt = -1;
                }
                else if (shiftState == Constants.LIBFPTR_SS_EXPIRED)
                {
                    rt = -2;
                }
                else
                {
                    rt = (int)fptr.getParamInt(Constants.LIBFPTR_PARAM_SHIFT_NUMBER);
                }
            }
            catch (Exception e)
            {
                var msg = "Получения номера открытой смены.";
                log.Error(msg, e);
            }
            return rt;
        }

        public bool ReadCheck(uint num)
        {
            bool rt = false;
            try
            {
                OpenConnection();

                fptr.setParam(Constants.LIBFPTR_PARAM_JSON_DATA, 
                    "{\"type\": \"getFnDocument\", \"fiscalDocumentNumber\": " + num.ToString() + "}");
                if (fptr.processJson() < 0) throw AtolException();
                String result = fptr.getParamString(Constants.LIBFPTR_PARAM_JSON_DATA);

                string s = "\r\n";
                s += "--------------- Документ из ФН _________________________";
                s += "\r\n";

                s += result;

                s += "\r\n";
                s += "--------------- Конец Документ из ФН _________________________";
                s += "\r\n";
                log.Info(s);
                rt = true;
            }
            catch (Exception e)
            {
                var msg = "Ошибка чтения документа.";
                log.Error(msg, e);
            }
            return rt;
        }

        public string Model => "АТОЛ";
    }
}
