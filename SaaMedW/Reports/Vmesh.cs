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
        public void DoIt(Person person)
        {
            var templateName =
                Path.Combine(Path.GetDirectoryName(
                    System.Reflection.Assembly.GetExecutingAssembly().Location), "templates", "Согласие.docx");
            var tmpName = Global.Source.GetTempFilename(".docx");
            File.Copy(templateName, tmpName);
            using (var fs = new FileStream(tmpName, FileMode.Open, FileAccess.ReadWrite))
            {
                using (var doc = DocX.Load(fs))
                {
                    doc.InsertAtBookmark(person.Fio, "fio");
                    if (person.BirthDate.HasValue)
                    {
                        var birthDate = person.BirthDate.Value;
                        doc.InsertAtBookmark(
                            birthDate.Day.ToString() + " " +
                            Global.Source.GetNameOfMonth(birthDate.Month) + " " +
                            birthDate.Year.ToString(), "birth");
                    }
                    doc.InsertAtBookmark(person.FullAddress, "address");
                    doc.InsertAtBookmark(
                        Options.GetParameter<string>(enumParameterType.Наименование_организации), "firma");

                    // Лица для информирования
                    var info = "";
                    if (person.Person_Person2.Count > 0)
                    {
                        foreach (var o in person.Person_Person2)
                        {
                            info += $"{o.Fio} {o.Phone}, ";
                        }
                        info = info.Trim();
                        if (info[info.Length - 1] == ',')
                            info = info.Substring(0, info.Length - 1);
                        info = info.Replace("  ", " ");
                    }
                    doc.InsertAtBookmark(info,  "info");

                    doc.InsertAtBookmark(person.Fio, "fio1");
                    doc.InsertAtBookmark(DateTime.Now.ToString("dd.MM.yyyy") + " г.", "dt");
                    doc.Save();
                }
            }
            System.Diagnostics.Process.Start(tmpName);
        }
    }
}
