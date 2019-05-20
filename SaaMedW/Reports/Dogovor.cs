using System;
using System.Collections.Generic;
using System.IO;
using Xceed.Words.NET;

namespace SaaMedW
{
    internal class Dogovor
    {
        public Dogovor()
        {
        }

        internal void DoIt(DateTime dt, int numdog, Person person, IEnumerable<VisitBenefit> benefits)
        {
            var templateName =
                Path.Combine(Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location), "templates", "ДОГОВОР 2019.docx");
            var tmpName = Global.Source.GetTempFilename(".docx");
            File.Copy(templateName, tmpName);
            using (var fs = new FileStream(tmpName, FileMode.Open, FileAccess.ReadWrite))
            {
                using (var doc = DocX.Load(fs))
                {
                    //var numdog = Options.GetParameter<int>(enumParameterType.Номер_договора);
                    doc.InsertAtBookmark(numdog.ToString(), "numdog");
                    doc.Bookmarks["numdog"].Paragraph.Font(new Font("Times New Roman")).FontSize(9);
                    //Options.SetParameter<int>(enumParameterType.Номер_договора, numdog + 1);
                    doc.InsertAtBookmark(DateTime.Today.ToString("dd.MM.yyyy") + " г.", "datedog");
                    doc.Bookmarks["datedog"].Paragraph.Font(new Font("Times New Roman")).FontSize(9);
                    //var firma = Options.GetParameter<string>(enumParameterType.Наименование_организации);
                    //doc.InsertAtBookmark(firma, "firma");
                    //var license = Options.GetParameter<string>(enumParameterType.Лицензия);
                    //doc.InsertAtBookmark(license, "license");
                    //var fioruk = Options.GetParameter<string>(enumParameterType.ФИО_руководителя);
                    //doc.InsertAtBookmark(fioruk, "fioruk");
                    doc.InsertAtBookmark(person.Fio, "patient");
                    doc.Bookmarks["patient"].Paragraph.Font(new Font("Times New Roman")).FontSize(9);
                    doc.InsertAtBookmark(person.FullAddress, "patient_address");
                    doc.Bookmarks["patient_address"].Paragraph.Font(new Font("Times New Roman")).FontSize(9);
                    doc.InsertAtBookmark(person.Phone, "patient_phone");
                    doc.Bookmarks["patient_phone"].Paragraph.Font(new Font("Times New Roman")).FontSize(9);
                    //doc.InsertAtBookmark(inv.Person.Fio, "patient_rod");
                    //var sm = RuDateAndMoneyConverter.CurrencyToTxt(Convert.ToDouble(inv.Sm), true);
                    //doc.InsertAtBookmark(sm, "sum_prop");
                    //doc.InsertAtBookmark(firma, "firma1");
                    //var address = Options.GetParameter<string>(enumParameterType.Юридический_адрес);
                    //doc.InsertAtBookmark(address, "address");
                    //var phone = Options.GetParameter<string>(enumParameterType.Телефоны);
                    //doc.InsertAtBookmark(phone, "phone");
                    //var inn = Options.GetParameter<string>(enumParameterType.ИНН);
                    //doc.InsertAtBookmark(inn, "inn");
                    //var kpp = Options.GetParameter<string>(enumParameterType.КПП);
                    //doc.InsertAtBookmark(kpp, "kpp");
                    //var bank = Options.GetParameter<string>(enumParameterType.Наименование_банка);
                    //doc.InsertAtBookmark(bank, "bank");
                    //var bik = Options.GetParameter<string>(enumParameterType.БИК_банка);
                    //doc.InsertAtBookmark(bik, "bik");
                    //var lc = Options.GetParameter<string>(enumParameterType.Расчетный_счет);
                    //doc.InsertAtBookmark(lc, "lc");
                    //doc.InsertAtBookmark(inv.Person.Fio, "patient1");
                    //doc.InsertAtBookmark(inv.Person.DocSeria, "serpasp");
                    //doc.InsertAtBookmark(inv.Person.DocNumber, "numpasp");
                    //doc.InsertAtBookmark("11.11.1111 Отделом МВД г.Тамбова", "vidan");
                    var t = doc.Tables.Find(s => s.TableCaption == "Услуги");
                    if (t != null)
                    {
                        decimal sm = 0;
                        Row r;
                        foreach (var benefit in benefits)
                        {
                            sm += benefit.Kol * benefit.Benefit.Price;
                            r = t.InsertRow();
                            r.Cells[0].Paragraphs[0].Append(dt.ToString("dd.MM.yyyy")).Font(new Font("Times New Roman")).FontSize(9);
                            r.Cells[0].SetBorder(TableCellBorderType.Left, new Border());
                            r.Cells[0].SetBorder(TableCellBorderType.Top, new Border());
                            r.Cells[0].SetBorder(TableCellBorderType.Right, new Border());
                            r.Cells[0].SetBorder(TableCellBorderType.Bottom, new Border());
                            r.Cells[1].Paragraphs[0].Append(benefit.Benefit.Name).Font(new Font("Times New Roman")).FontSize(9);
                            r.Cells[1].SetBorder(TableCellBorderType.Left, new Border());
                            r.Cells[1].SetBorder(TableCellBorderType.Top, new Border());
                            r.Cells[1].SetBorder(TableCellBorderType.Right, new Border());
                            r.Cells[1].SetBorder(TableCellBorderType.Bottom, new Border());
                            r.Cells[2].Paragraphs[0].Append((benefit.Kol * benefit.Benefit.Price).ToString("#0.00")).Font(new Font("Times New Roman")).FontSize(9);
                            r.Cells[2].SetBorder(TableCellBorderType.Left, new Border());
                            r.Cells[2].SetBorder(TableCellBorderType.Top, new Border());
                            r.Cells[2].SetBorder(TableCellBorderType.Right, new Border());
                            r.Cells[2].SetBorder(TableCellBorderType.Bottom, new Border());
                        }
                        r = t.InsertRow();
                        r.Cells[0].Paragraphs[0].Append("ИТОГО").Font(new Font("Times New Roman")).FontSize(9);
                        r.Cells[0].SetBorder(TableCellBorderType.Left, new Border());
                        r.Cells[0].SetBorder(TableCellBorderType.Top, new Border());
                        r.Cells[0].SetBorder(TableCellBorderType.Right, new Border());
                        r.Cells[0].SetBorder(TableCellBorderType.Bottom, new Border());
                        r.Cells[1].Paragraphs[0].Append("");
                        r.Cells[1].SetBorder(TableCellBorderType.Left, new Border());
                        r.Cells[1].SetBorder(TableCellBorderType.Top, new Border());
                        r.Cells[1].SetBorder(TableCellBorderType.Right, new Border());
                        r.Cells[1].SetBorder(TableCellBorderType.Bottom, new Border());
                        r.Cells[2].Paragraphs[0].Append(sm.ToString("#0.00")).Font(new Font("Times New Roman")).FontSize(9);
                        r.Cells[2].SetBorder(TableCellBorderType.Left, new Border());
                        r.Cells[2].SetBorder(TableCellBorderType.Top, new Border());
                        r.Cells[2].SetBorder(TableCellBorderType.Right, new Border());
                        r.Cells[2].SetBorder(TableCellBorderType.Bottom, new Border());
                    }
                    doc.Save();
                }
            }
            System.Diagnostics.Process.Start(tmpName);
        }
    }
}