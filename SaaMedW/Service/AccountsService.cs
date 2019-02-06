using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.Service
{
    public class AccountsService : IAccounts
    {
        log4net.ILog log;
        public AccountsService()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
        }

        public bool BackMoneyOneInvoice(Invoice p_invoice, out string message)
        {
            message = String.Empty;
            using (SaaMedEntities ctx = new SaaMedEntities())
            {
                Invoice invoice = ctx.Invoice.Find(p_invoice.Id);
                var oPay = new Pays()
                {
                    Dt = DateTime.Today,
                    Sm = invoice.Payed,
                    Person = invoice.Person,
                    PaymentType = enumPaymentType.Возврат
                };
                ctx.Pays.Add(oPay);
                // откатываем оплату счета
                invoice.Payed = 0;
                invoice.Status = enumStatusInvoice.Неоплачен;
                try
                {
                    ctx.SaveChanges();
                }
                catch (Exception)
                {
                    invoice.Payed = p_invoice.Payed;
                    invoice.Status = p_invoice.Status;
                    ctx.Pays.Remove(oPay);
                    message = "Ошибка записи возврата платежа";
                    return false;
                }
                return true;
            }
        }

        public bool PayOneInvoice(decimal pay, Invoice p_invoice, enumPaymentType paymentType)
        {
            bool rt = false;
            using (SaaMedEntities ctx = new SaaMedEntities())
            {
                Invoice invoice = ctx.Invoice.Find(p_invoice.Id);
                var oPay = new Pays()
                {
                    Dt = DateTime.Today,
                    Sm = pay,
                    Person = invoice.Person,
                    PaymentType = paymentType
                };
                ctx.Pays.Add(oPay);
                invoice.Payed += pay;
                if (invoice.Payed == invoice.Sm)
                    invoice.Status = enumStatusInvoice.Оплачен;
                else
                    invoice.Status = enumStatusInvoice.Оплачен_частично;
                try
                {
                    ctx.SaveChanges();
                    rt = true;
                }
                catch (Exception e)
                {
                    ctx.Pays.Remove(oPay);
                    invoice.Payed -= pay;
                    if (invoice.Payed == 0)
                        invoice.Status = enumStatusInvoice.Неоплачен;
                    else
                        invoice.Status = enumStatusInvoice.Оплачен_частично;
                    log.Error("Ошибка сохранения платежа", e);
                }
                return rt;
            }
        }
    }
}
