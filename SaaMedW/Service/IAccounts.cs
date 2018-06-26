using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public interface IAccounts
    {
        /// <summary>
        /// Оплатить один счет
        /// </summary>
        /// <param name="Sm">Сумма для оплаты</param>
        /// <param name="invoice">Оплачиваемый счет</param>
        /// <param name="paymentType">Вид оплаты</param>
        bool PayOneInvoice(decimal Sm, Invoice invoice, enumPaymentType paymentType);
        bool BackMoneyOneInvoice(Invoice invoice, out string message);
    }
}
