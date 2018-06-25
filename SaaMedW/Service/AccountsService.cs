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
            log4net.LogManager.GetLogger(this.GetType());
        }
        public bool PayOneInvoice(decimal pay, Invoice p_invoice, enumPaymentType paymentType)
        {
            bool rt = false;
            SaaMedEntities ctx = new SaaMedEntities();
            Invoice invoice = ctx.Invoice.Find(p_invoice.Id);
            var oPay = new Pays()
            {
                Dt = DateTime.Today,
                Sm = pay,
                Person = invoice.Person,
                PaymentType = paymentType
            };
            invoice.Payed += pay;
            if (invoice.Payed == invoice.Sm)
                invoice.Status = 2;
            else
                invoice.Status = 1;
            ctx.Pays.Add(oPay);
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
                    invoice.Status = 0;
                else
                    invoice.Status = 1;
                log.Error("Ошибка сохранения платежа", e);
            }
            return rt;
        }
    }
}
