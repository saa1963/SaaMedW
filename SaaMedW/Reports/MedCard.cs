using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Xceed.Words.NET;
using Microsoft.Office.Interop.Word;

namespace SaaMedW
{
    public class MedCard
    {
        public string DoIt(Person person)
        {
            const int fsz = 12;
            var templateName =
                Path.Combine(Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location), "templates", "Медкарта.docx");
            var tmpName = Global.Source.GetTempFilename(".docx");
            File.Copy(templateName, tmpName);
            var app = new Application();
            var doc = app.Documents.Open(tmpName);

            //        doc.InsertAtBookmark(person.CreateDate.Day.ToString(), "day_date");
            var r = doc.Bookmarks["day_date"].Range;
            r.Text = person.CreateDate.Day.ToString();
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = fsz;
            //        doc.InsertAtBookmark(Global.Source.GetNameOfMonth(person.CreateDate.Month), "month_date");
            r = doc.Bookmarks["month_date"].Range;
            r.Text = Global.Source.GetNameOfMonth(person.CreateDate.Month);
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = fsz;
            //        doc.InsertAtBookmark(person.CreateDate.Year.ToString(), "year_date");
            r = doc.Bookmarks["year_date"].Range;
            r.Text = person.CreateDate.Year.ToString();
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = fsz;
            //        doc.InsertAtBookmark(person.LastName + " " + person.FirstName + " " + person.MiddleName, "fio");
            r = doc.Bookmarks["fio"].Range;
            r.Text = person.LastName + " " + person.FirstName + " " + person.MiddleName;
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = fsz;
            //        doc.InsertAtBookmark("  " + (person.Sex.HasValue ? ((int)person.Sex.Value).ToString() : ""), "sex");
            r = doc.Bookmarks["sex"].Range;
            r.Text = "  " + (person.Sex.HasValue ? ((int)person.Sex.Value).ToString() : "");
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = fsz;
            if (person.BirthDate.HasValue)
            {
                //            doc.InsertAtBookmark(person.BirthDate.Value.Day.ToString(), "day_birth");
                r = doc.Bookmarks["day_birth"].Range;
                r.Text = person.BirthDate.Value.Day.ToString();
                r.Font.Bold = 1;
                r.Font.Italic = 1;
                r.Font.Size = fsz;
                //            doc.InsertAtBookmark(Global.Source.GetNameOfMonth(person.BirthDate.Value.Month), "month_birth");
                r = doc.Bookmarks["month_birth"].Range;
                r.Text = Global.Source.GetNameOfMonth(person.BirthDate.Value.Month);
                r.Font.Bold = 1;
                r.Font.Italic = 1;
                r.Font.Size = fsz;
                //            doc.InsertAtBookmark(person.BirthDate.Value.Year.ToString(), "year_birth");
                r = doc.Bookmarks["year_birth"].Range;
                r.Text = person.BirthDate.Value.Year.ToString();
                r.Font.Bold = 1;
                r.Font.Italic = 1;
                r.Font.Size = fsz;
            }
            //        doc.InsertAtBookmark(person.AddressSubject ?? "", "region");
            r = doc.Bookmarks["region"].Range;
            r.Text = person.AddressSubject ?? "";
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = fsz;
            //        doc.InsertAtBookmark(person.AddressRaion ?? "", "district");
            r = doc.Bookmarks["district"].Range;
            r.Text = person.AddressRaion ?? "";
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = fsz;
            //        doc.InsertAtBookmark(person.AddressCity ?? "", "city");
            r = doc.Bookmarks["city"].Range;
            r.Text = person.AddressCity ?? "";
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = fsz;
            //        doc.InsertAtBookmark(person.AddressPunkt ?? "", "punkt");
            r = doc.Bookmarks["punkt"].Range;
            r.Text = person.AddressPunkt ?? "";
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = fsz;
            //        doc.InsertAtBookmark(person.AddressStreet ?? "", "street");
            r = doc.Bookmarks["street"].Range;
            r.Text = person.AddressStreet ?? "";
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = fsz;
            //        doc.InsertAtBookmark(person.AddressHouse ?? "", "house");
            r = doc.Bookmarks["house"].Range;
            r.Text = person.AddressHouse ?? "";
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = fsz;
            //        doc.InsertAtBookmark(person.AddressFlat ?? "", "flat");
            r = doc.Bookmarks["flat"].Range;
            r.Text = person.AddressFlat ?? "";
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = fsz;
            //        doc.InsertAtBookmark(person.Phone ?? "", "phone");
            r = doc.Bookmarks["phone"].Range;
            r.Text = person.Phone ?? "";
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = fsz;
            //        doc.InsertAtBookmark("  " + (person.Mestnost.HasValue ? person.Mestnost.Value.ToString() : ""), "mestnost");
            r = doc.Bookmarks["mestnost"].Range;
            r.Text = "  " + (person.Mestnost.HasValue ? person.Mestnost.Value.ToString() : "");
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = fsz;
            if (person.DocumentTypeId != null)
            {
                //            doc.InsertAtBookmark(person.DocumentType.Name, "doc_vid");
                r = doc.Bookmarks["doc_vid"].Range;
                r.Text = person.DocumentType.Name;
                r.Font.Bold = 1;
                r.Font.Italic = 1;
                r.Font.Size = fsz;
            }
            if (person.DocSeria != null)
            {
                //            doc.InsertAtBookmark(person.DocSeria, "doc_seria");
                r = doc.Bookmarks["doc_seria"].Range;
                r.Text = person.DocSeria;
                r.Font.Bold = 1;
                r.Font.Italic = 1;
                r.Font.Size = fsz;
            }
            if (person.DocNumber != null)
            {
                //            doc.InsertAtBookmark(person.DocNumber, "doc_number");
                r = doc.Bookmarks["doc_number"].Range;
                r.Text = person.DocNumber;
                r.Font.Bold = 1;
                r.Font.Italic = 1;
                r.Font.Size = fsz;
            }

            doc.Save();
            doc.Close();
            app.Quit();
            //using (var fs = new FileStream(tmpName, FileMode.Open, FileAccess.ReadWrite))
            //{
            //    using (var doc = DocX.Load(fs))
            //    {
            //        doc.InsertAtBookmark(person.CreateDate.Day.ToString(), "day_date");
            //        doc.InsertAtBookmark(Global.Source.GetNameOfMonth(person.CreateDate.Month), "month_date");
            //        doc.InsertAtBookmark(person.CreateDate.Year.ToString(), "year_date");
            //        doc.InsertAtBookmark(person.LastName + " " + person.FirstName + " " + person.MiddleName, "fio");
            //        doc.InsertAtBookmark("  " + (person.Sex.HasValue ? ((int)person.Sex.Value).ToString() : ""), "sex");
            //        if (person.BirthDate.HasValue)
            //        {
            //            doc.InsertAtBookmark(person.BirthDate.Value.Day.ToString(), "day_birth");
            //            doc.InsertAtBookmark(Global.Source.GetNameOfMonth(person.BirthDate.Value.Month), "month_birth");
            //            doc.InsertAtBookmark(person.BirthDate.Value.Year.ToString(), "year_birth");
            //        }
            //        doc.InsertAtBookmark(person.AddressSubject ?? "", "region");
            //        doc.InsertAtBookmark(person.AddressRaion ?? "", "district");
            //        doc.InsertAtBookmark(person.AddressCity ?? "", "city");
            //        doc.InsertAtBookmark(person.AddressPunkt ?? "", "punkt");
            //        doc.InsertAtBookmark(person.AddressStreet ?? "", "street");
            //        doc.InsertAtBookmark(person.AddressHouse ?? "", "house");
            //        doc.InsertAtBookmark(person.AddressFlat ?? "", "flat");
            //        doc.InsertAtBookmark(person.Phone ?? "", "phone");
            //        doc.InsertAtBookmark("  " + (person.Mestnost.HasValue ? person.Mestnost.Value.ToString() : ""), "mestnost");
            //        if (person.DocumentTypeId != null)
            //        {
            //            doc.InsertAtBookmark(person.DocumentType.Name, "doc_vid");
            //        }
            //        if (person.DocSeria != null)
            //        {
            //            doc.InsertAtBookmark(person.DocSeria, "doc_seria");
            //        }
            //        if (person.DocNumber != null)
            //        {
            //            doc.InsertAtBookmark(person.DocNumber, "doc_number");
            //        }
            //        doc.Save();
            //    }
            //}
            return tmpName;
            //System.Diagnostics.Process.Start(tmpName);
        }
    }
}
