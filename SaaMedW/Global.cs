using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SaaMedW
{
    public sealed class Global
    {
        private Global() { }

        private static readonly Lazy<Global> lazy =
            new Lazy<Global>(() => new Global());

        public static Global Source { get { return lazy.Value; } }

        public Users rUser { get; set; }

        internal void SaveColumnsWidth(UserControl uc)
        {
            var fName = uc.Name;
            DataGrid g = (DataGrid)FindInVisualTreeDown(uc, "DataGrid");
            foreach (DataGridColumn col in g.Columns)
            {
                if (col.Width.IsAbsolute)
                    Global.Source.SaveParam<double>(fName + "_" + col.DisplayIndex.ToString(), col.Width.Value);
            }
        }

        internal void SetColumnsWidth(UserControl uc)
        {
            var fName = uc.Name;
            DataGrid g = (DataGrid)FindInVisualTreeDown(uc, "DataGrid");
            foreach (DataGridColumn col in g.Columns)
            {
                if (col.Width.IsAbsolute)
                {
                    var rt = Global.Source.GetParam<double>(fName + "_" + col.DisplayIndex.ToString());
                    if (rt != 0) col.Width = rt;
                }
            }
        }

        public void SaveParam<T>(string name, T value)
        {
            var pth = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SaaMedW");
            var fname = Path.Combine(pth, typeof(T).Name + "Params.bin");
            Dictionary<string, T> d = null;
            BinaryFormatter formatter = new BinaryFormatter();
            if (!Directory.Exists(pth))
            {
                Directory.CreateDirectory(pth);
            }
            if (!File.Exists(fname))
            {
                d = new Dictionary<string, T>();
            }
            else
            {
                try
                {
                    using (FileStream fs = new FileStream(fname, FileMode.Open))
                    {
                        d = (Dictionary<string, T>)formatter.Deserialize(fs);
                    }
                }
                catch
                {
                    File.Delete(fname);
                    d = new Dictionary<string, T>();
                }
            }
            d[name] = value;
            using (FileStream fs = new FileStream(fname, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, d);
            }
        }

        public T GetParam<T>(string name) where T : new()
        {
            var pth = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SaaMedW");
            var fname = Path.Combine(pth, typeof(T).Name + "Params.bin");
            Dictionary<string, T> d = null;
            BinaryFormatter formatter = new BinaryFormatter();
            if (!Directory.Exists(pth))
            {
                Directory.CreateDirectory(pth);
            }
            if (!File.Exists(fname))
            {
                return default(T);
            }
            else
            {
                try
                {
                    using (FileStream fs = new FileStream(fname, FileMode.Open))
                    {
                        d = (Dictionary<string, T>)formatter.Deserialize(fs);
                    }
                    return d[name];
                }
                catch
                {
                    File.Delete(fname);
                    return default(T);
                }
            }
        }

        public string GetTempFilename(string ext)
        {
            var tempName0 = Path.GetTempFileName();
            var tempName = Path.Combine(Path.GetDirectoryName(tempName0), Path.GetFileNameWithoutExtension(tempName0) + ext);
            File.Delete(tempName0);
            return tempName;
        }

        public string GetNameOfMonth(int month)
        {
            string[] mths =
                { "января", "февраля", "марта", "апреля", "мая", "июня", "июля",
                    "августа", "сентября", "октября", "ноября", "декабря" };
            return mths[month - 1];
        }

        public DependencyObject FindInVisualTreeDown(DependencyObject obj, string type)
        {
            if (obj != null)
            {
                if (obj.GetType().Name == type)
                {
                    return obj;
                }

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    DependencyObject childReturn = FindInVisualTreeDown(VisualTreeHelper.GetChild(obj, i), type);
                    if (childReturn != null)
                    {
                        return childReturn;
                    }
                }
            }

            return null;
        }
    }
}
