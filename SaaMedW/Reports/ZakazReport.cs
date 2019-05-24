using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace SaaMedW
{
    public class ZakazReport
    {
        internal string DoIt(DateTime dt, int num, Person person, bool dms, IEnumerable<BenefitForZakaz> benefits)
        {
            SaaMedEntities ctx = new SaaMedEntities();
            var personalList = ctx.Personal.ToList();
            var templateName =
                Path.Combine(Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location), "templates", "zakaz.docx");
            var tmpName = Global.Source.GetTempFilename(".docx");
            File.Copy(templateName, tmpName);
            using (var fs = new FileStream(tmpName, FileMode.Open, FileAccess.ReadWrite))
            {
                using (var doc = DocX.Load(fs))
                {
                    doc.InsertAtBookmark(num.ToString(), "num");
                    doc.Bookmarks["num"].Paragraph.Font(new Font("Times New Roman")).FontSize(11);
                    doc.InsertAtBookmark(dt.ToString("dd.MM.yyyy") + " г.", "date");
                    doc.Bookmarks["date"].Paragraph.Font(new Font("Times New Roman")).FontSize(11);

                    doc.InsertAtBookmark(person.FullAddress ?? "", "address");
                    doc.Bookmarks["address"].Paragraph.Font(new Font("Times New Roman")).FontSize(11);
                    doc.InsertAtBookmark(person.BirthDate?.ToString("dd.MM.yyyy") ?? "", "birthday");
                    doc.Bookmarks["birthday"].Paragraph.Font(new Font("Times New Roman")).FontSize(11);
                    doc.InsertAtBookmark(dms ? "Да" : "Нет", "dms");
                    doc.Bookmarks["dms"].Paragraph.Font(new Font("Times New Roman")).FontSize(11);
                    doc.InsertAtBookmark(person.Fio ?? "", "patient");
                    doc.Bookmarks["patient"].Paragraph.Font(new Font("Times New Roman")).FontSize(11);
                    doc.InsertAtBookmark(person.Phone ?? "", "phone");
                    doc.Bookmarks["phone"].Paragraph.Font(new Font("Times New Roman")).FontSize(11);

                    var t = doc.Tables.Find(s => s.TableCaption == "Услуги");
                    if (t != null)
                    {
                        decimal sm = 0;
                        Row r;
                        int i = 1;
                        foreach (var benefit in benefits)
                        {
                            sm += benefit.Kol * benefit.Price;
                            r = t.InsertRow();
                            r.Cells[0].Paragraphs[0].Append(i++.ToString())
                                .Font(new Font("Times New Roman")).FontSize(9);
                            r.Cells[0].SetBorder(TableCellBorderType.Left, new Border());
                            r.Cells[0].SetBorder(TableCellBorderType.Top, new Border());
                            r.Cells[0].SetBorder(TableCellBorderType.Right, new Border());
                            r.Cells[0].SetBorder(TableCellBorderType.Bottom, new Border());
                            r.Cells[1].Paragraphs[0].Append(benefit.BenefitName ?? "")
                                .Font(new Font("Times New Roman")).FontSize(9);
                            r.Cells[1].SetBorder(TableCellBorderType.Left, new Border());
                            r.Cells[1].SetBorder(TableCellBorderType.Top, new Border());
                            r.Cells[1].SetBorder(TableCellBorderType.Right, new Border());
                            r.Cells[1].SetBorder(TableCellBorderType.Bottom, new Border());
                            r.Cells[2].Paragraphs[0].Append(personalList.Find(s => s.Id == benefit.PersonalId)?.Fio ?? "")
                                .Font(new Font("Times New Roman")).FontSize(9);
                            r.Cells[2].SetBorder(TableCellBorderType.Left, new Border());
                            r.Cells[2].SetBorder(TableCellBorderType.Top, new Border());
                            r.Cells[2].SetBorder(TableCellBorderType.Right, new Border());
                            r.Cells[2].SetBorder(TableCellBorderType.Bottom, new Border());
                            r.Cells[3].Paragraphs[0].Append(benefit.Price.ToString("#0.00"))
                                .Font(new Font("Times New Roman")).FontSize(9);
                            r.Cells[3].SetBorder(TableCellBorderType.Left, new Border());
                            r.Cells[3].SetBorder(TableCellBorderType.Top, new Border());
                            r.Cells[3].SetBorder(TableCellBorderType.Right, new Border());
                            r.Cells[3].SetBorder(TableCellBorderType.Bottom, new Border());
                            r.Cells[4].Paragraphs[0].Append(benefit.Kol.ToString("#0"))
                                .Font(new Font("Times New Roman")).FontSize(9);
                            r.Cells[4].SetBorder(TableCellBorderType.Left, new Border());
                            r.Cells[4].SetBorder(TableCellBorderType.Top, new Border());
                            r.Cells[4].SetBorder(TableCellBorderType.Right, new Border());
                            r.Cells[4].SetBorder(TableCellBorderType.Bottom, new Border());
                            r.Cells[5].Paragraphs[0].Append(benefit.Sm.ToString("#0.00"))
                                .Font(new Font("Times New Roman")).FontSize(9);
                            r.Cells[5].SetBorder(TableCellBorderType.Left, new Border());
                            r.Cells[5].SetBorder(TableCellBorderType.Top, new Border());
                            r.Cells[5].SetBorder(TableCellBorderType.Right, new Border());
                            r.Cells[5].SetBorder(TableCellBorderType.Bottom, new Border());
                        }
                        r = t.InsertRow();
                        r.Cells[0].Paragraphs[0].Append("ИТОГО").Font(new Font("Times New Roman")).FontSize(9);
                        r.Cells[5].Paragraphs[0].Append(sm.ToString("#0.00")).Font(new Font("Times New Roman")).FontSize(9);
                    }

                    doc.Save();
                }
            }
            return tmpName;
        }
    }
}
