using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

namespace SaaMedW
{
    public class Vmesh
    {
        public string DoIt(DateTime dt, Person person)
        {
            var templateName =
                Path.Combine(Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location), "templates", "согласие2019.docx");
            var tmpName = Global.Source.GetTempFilename(".docx");
            File.Copy(templateName, tmpName);
            using (var fs = new FileStream(tmpName, FileMode.Open, FileAccess.ReadWrite))
            {
                using (var doc = DocX.Load(fs))
                {
                    doc.InsertAtBookmark(person.Fio, "patient");
                    doc.InsertAtBookmark(person.Fio, "patient_1");
                    doc.InsertAtBookmark(dt.ToString("dd.MM.yyyy") + " г.", "date");
                    doc.InsertAtBookmark(dt.ToString("dd.MM.yyyy") + " г.", "date1");
                    doc.Save();
                }
            }
            return tmpName;
        }
    }
}
