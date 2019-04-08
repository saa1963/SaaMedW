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
        private ObservableCollection<VmMKB> m_SearchList = new ObservableCollection<VmMKB>();
        private string m_SearchString;
        private string m_SearchString0;
        public string SearchString
        {
            get => m_SearchString;
            set
            {
                if (value != m_SearchString)
                {
                    m_SearchList.Clear();
                    m_SearchString = value;
                    OnPropertyChanged("SearchString");
                }
            }
        }
        public ObservableCollection<VmMKB> MkbList
        {
            get => m_MkbList;
            set
            {
                m_MkbList = value;
                OnPropertyChanged("MbkList");
            }
        }
        public ObservableCollection<VmMKB> SearchList
        {
            get => m_SearchList;
            set
            {
                m_SearchList = value;
                OnPropertyChanged("SearchList");
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
        public RelayCommand SearchForwardCommand => new RelayCommand(SearchForward);

        private static void SearchForward(object obj)
        {
            var vm = obj as MkbViewModel;
            if (!String.IsNullOrWhiteSpace(vm.SearchString))
                vm.SearchStringForward();
        }

        private void SearchStringForward()
        {
            m_SearchString0 = m_SearchString.ToUpper();
            foreach (var o in m_MkbList)
            {
                SearchStringInNode(o);
            }
        }

        private void SearchStringInNode(VmMKB o)
        {
            if (o.ChildMkb.Count == 0)
            {
                if (o.Name.ToUpper().IndexOf(m_SearchString0) >= 0)
                    SearchList.Add(o);
            }
            else
            {
                foreach(var o1 in o.ChildMkb)
                {
                    SearchStringInNode(o1);
                }
            }
        }

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
