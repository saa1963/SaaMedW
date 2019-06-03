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
            var tmpName = GetDogovorFname(dt, numdog, person);
            System.Diagnostics.Process.Start(tmpName);
        }

        internal string GetDogovorFname(DateTime dt, int numdog, Person person)
        {
            var templateName =
                Path.Combine(Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location), "templates", "ДОГОВОР 2019 для ПО.docx");
            var tmpName = Global.Source.GetTempFilename(".docx");
            File.Copy(templateName, tmpName);
            using (var fs = new FileStream(tmpName, FileMode.Open, FileAccess.ReadWrite))
            {
                using (var doc = DocX.Load(fs))
                {
                    doc.InsertAtBookmark(numdog.ToString(), "numdog");
                    doc.Bookmarks["numdog"].Paragraph.Font(new Font("Times New Roman")).FontSize(9);
                    doc.InsertAtBookmark(DateTime.Today.ToString("dd.MM.yyyy") + " г.", "datedog");
                    doc.Bookmarks["datedog"].Paragraph.Font(new Font("Times New Roman")).FontSize(9);
                    doc.InsertAtBookmark(person.Fio, "patient");
                    doc.Bookmarks["patient"].Paragraph.Font(new Font("Times New Roman")).FontSize(9);

                    doc.InsertAtBookmark(person.FullAddress, "patient_address");
                    doc.Bookmarks["patient_address"].Paragraph.Font(new Font("Times New Roman")).FontSize(9);
                    doc.InsertAtBookmark(person.Phone, "patient_phone");
                    doc.Bookmarks["patient_phone"].Paragraph.Font(new Font("Times New Roman")).FontSize(9);
                    doc.Save();
                }
            }
            return tmpName;
        }
    }
}