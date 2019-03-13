using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using OfficeOpenXml;

namespace SaaMedW
{
    public class MkbViewModel: NotifyPropertyChanged
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private List<MKB> lst;
        private ObservableCollection<VmMKB> m_MkbList = new ObservableCollection<VmMKB>();
        public ObservableCollection<VmMKB> MkbList
        {
            get => m_MkbList;
            set
            {
                m_MkbList = value;
                OnPropertyChanged("MbkList");
            }
        }
        public MkbViewModel()
        {
            RefreshData();
        }

        private void RefreshData()
        {
            lst = ctx.MKB.ToList();
            m_MkbList.Clear();
            var q = lst.Where(s => String.IsNullOrWhiteSpace(s.Parent) || s.Parent == "NULL").OrderBy(s => s.Kod);
            foreach(var o in q)
            {
                m_MkbList.Add(new VmMKB(lst, o));
            }
        }
        public RelayCommand LoadCommand { get; set; } = new RelayCommand(Load);

        private static void Load(object obj)
        {
            string fname;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Файл МКБ-10|mkb10.xlsx";
            if (ofd.ShowDialog() == true)
            {
                fname = ofd.FileName;
            }
            else
            {
                return;
            }
            using (var pack = new ExcelPackage(new FileInfo(fname)))
            {
                int row = 5;
                var wsh = pack.Workbook.Worksheets[1];
                string kod, name, parent;
                using (var ctx = new SaaMedEntities())
                {
                    ctx.Database.ExecuteSqlCommand("DELETE FROM MKB");
                    while (!String.IsNullOrWhiteSpace(kod = wsh.Cells[row, 1].Text))
                    {
                        name = wsh.Cells[row, 2].Text;
                        parent = wsh.Cells[row, 3].Text;
                        ctx.Database.ExecuteSqlCommand("INSERT INTO MKB (Kod, Name, Parent) VALUES (@kod, @name, @parent)", 
                            new SqlParameter("@kod", kod),
                            new SqlParameter("@name", name),
                            new SqlParameter("@parent", parent));
                        row++;
                    }
                }

                MessageBox.Show("Загрузка завершена");
            }
        }
    }
}
