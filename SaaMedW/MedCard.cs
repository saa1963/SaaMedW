using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace SaaMedW
{
    public class MedCard
    {
        public void DoIt(Person person)
        {
            var templateName =
                Path.Combine(Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location), "templates", "Медкарта.docx");
            var tmpName = Global.Source.GetTempFilename(".docx");
            File.Copy(templateName, tmpName);
            using (var fs = new FileStream(tmpName, FileMode.Open, FileAccess.ReadWrite))
            {
                using (var doc = DocX.Load(fs))
                {
                    doc.InsertAtBookmark(person.CreateDate.Day.ToString(), "day_date");
                    doc.InsertAtBookmark(Global.Source.GetNameOfMonth(person.CreateDate.Month), "month_date");
                    doc.InsertAtBookmark(person.CreateDate.Year.ToString(), "year_date");
                    doc.InsertAtBookmark(person.LastName + " " + person.FirstName + " " + person.MiddleName, "fio");
                    doc.InsertAtBookmark("  " + (person.Sex.HasValue ? person.Sex.Value.ToString() : ""), "sex");
                    if (person.BirthDate.HasValue)
                    {
                        doc.InsertAtBookmark(person.BirthDate.Value.Day.ToString(), "day_birth");
                        doc.InsertAtBookmark(Global.Source.GetNameOfMonth(person.BirthDate.Value.Month), "month_birth");
                        doc.InsertAtBookmark(person.BirthDate.Value.Year.ToString(), "year_birth");
                    }
                    doc.InsertAtBookmark(person.AddressSubject ?? "", "region");
                    doc.InsertAtBookmark(person.AddressRaion ?? "", "district");
                    doc.InsertAtBookmark(person.AddressCity ?? "", "city");
                    doc.InsertAtBookmark(person.AddressPunkt ?? "", "punkt");
                    doc.InsertAtBookmark(person.AddressStreet ?? "", "street");
                    doc.InsertAtBookmark(person.AddressHouse ?? "", "house");
                    doc.InsertAtBookmark(person.AddressFlat ?? "", "flat");
                    doc.InsertAtBookmark(person.Phone ?? "", "phone");
                    doc.InsertAtBookmark("  " + (person.Mestnost.HasValue ? person.Mestnost.Value.ToString() : ""), "mestnost");
                    if (person.DocumentTypeId != null)
                    {
                        doc.InsertAtBookmark(person.DocumentType.Name, "doc_vid");
                    }
                    if (person.DocSeria != null)
                    {
                        doc.InsertAtBookmark(person.DocSeria, "doc_seria");
                    }
                    if (person.DocNumber != null)
                    {
                        doc.InsertAtBookmark(person.DocNumber, "doc_number");
                    }
                    doc.Save();
                }
            }
            System.Diagnostics.Process.Start(tmpName);
        }
    }
}
