using System;
using System.IO;
using Xceed.Words.NET;

namespace SaaMedW
{
    internal class Dogovor
    {
        public Dogovor()
        {
        }

        internal void DoIt(Invoice inv)
        {
            var templateName =
                Path.Combine(Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location), "templates", "dogovor-usluga-medichina.docx");
            var tmpName = Global.Source.GetTempFilename(".docx");
            File.Copy(templateName, tmpName);
            using (var fs = new FileStream(tmpName, FileMode.Open, FileAccess.ReadWrite))
            {
                using (var doc = DocX.Load(fs))
                {
                    var numdog = Options.GetParameter<int>(enumParameterType.Номер_договора);
                    doc.InsertAtBookmark(numdog.ToString(), "numdog");
                    Options.SetParameter<int>(enumParameterType.Номер_договора, numdog + 1);
                    doc.InsertAtBookmark(DateTime.Today.ToString("dd.MM.yyyy") + " г.", "datedog");
                    var firma = Options.GetParameter<string>(enumParameterType.Наименование_организации);
                    doc.InsertAtBookmark(firma, "firma");
                    var license = Options.GetParameter<string>(enumParameterType.Лицензия);
                    doc.InsertAtBookmark(license, "license");
                    var fioruk = Options.GetParameter<string>(enumParameterType.ФИО_руководителя);
                    doc.InsertAtBookmark(fioruk, "fioruk");
                    doc.InsertAtBookmark(inv.Person.Fio, "patient");
                    doc.InsertAtBookmark(inv.Person.Fio, "patient_rod");
                    var sm = RuDateAndMoneyConverter.CurrencyToTxt(Convert.ToDouble(inv.Sm), true);
                    doc.InsertAtBookmark(sm, "sum_prop");
                    doc.InsertAtBookmark(firma, "firma1");
                    var address = Options.GetParameter<string>(enumParameterType.Юридический_адрес);
                    doc.InsertAtBookmark(address, "address");
                    var phone = Options.GetParameter<string>(enumParameterType.Телефоны);
                    doc.InsertAtBookmark(phone, "phone");
                    var inn = Options.GetParameter<string>(enumParameterType.ИНН);
                    doc.InsertAtBookmark(inn, "inn");
                    var kpp = Options.GetParameter<string>(enumParameterType.КПП);
                    doc.InsertAtBookmark(kpp, "kpp");
                    var bank = Options.GetParameter<string>(enumParameterType.Наименование_банка);
                    doc.InsertAtBookmark(bank, "bank");
                    var bik = Options.GetParameter<string>(enumParameterType.БИК_банка);
                    doc.InsertAtBookmark(bik, "bik");
                    var lc = Options.GetParameter<string>(enumParameterType.Расчетный_счет);
                    doc.InsertAtBookmark(lc, "lc");
                    doc.InsertAtBookmark(inv.Person.Fio, "patient1");
                    doc.InsertAtBookmark(inv.Person.DocSeria, "serpasp");
                    doc.InsertAtBookmark(inv.Person.DocNumber, "numpasp");
                    doc.InsertAtBookmark("11.11.1111 Отделом МВД г.Тамбова", "vidan");
                    doc.Save();
                }
            }
            System.Diagnostics.Process.Start(tmpName);
        }
    }
}