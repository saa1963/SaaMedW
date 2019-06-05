using System;
using System.Collections.Generic;
using System.IO;
//using Xceed.Words.NET;
using Microsoft.Office.Interop.Word;

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
            var app = new Application();
            var doc = app.Documents.Open(tmpName);
            var r = doc.Bookmarks["numdog"].Range;
            r.Text = numdog.ToString();
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = 12;
            r = doc.Bookmarks["datedog"].Range;
            r.Text = dt.ToString("dd.MM.yyyy") + " г.";
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = 12;
            r = doc.Bookmarks["patient"].Range;
            r.Text = person.Fio;
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = 12;
            r = doc.Bookmarks["patient_address"].Range;
            r.Text = person.FullAddress;
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = 12;
            r = doc.Bookmarks["patient_phone"].Range;
            r.Text = person.Phone;
            r.Font.Bold = 1;
            r.Font.Italic = 1;
            r.Font.Size = 12;
            doc.Save();
            doc.Close();
            app.Quit();

            return tmpName;
        }
    }
}