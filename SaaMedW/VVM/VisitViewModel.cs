using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SaaMedW.View;
using System.Windows.Data;
using System.Timers;

namespace SaaMedW
{
    public class VisitViewModel: NotifyPropertyChanged, IDisposable
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private DateTime _selectedDate;
        private Timer timer;

        public ObservableCollection<IdName> ListStatus { get; set; } = new ObservableCollection<IdName>()
        {
            new IdName(){ Id = 0, Name = "Все"},
            new IdName(){ Id = 1, Name = "Предварительные"},
            new IdName(){ Id = 2, Name = "Завершенные"}
        };

        private int m_SelectedStatus;
        public int SelectedStatus
        {
            get => m_SelectedStatus;
            set
            {
                m_SelectedStatus = value;
                OnPropertyChanged("SelectedStatus");
                RefreshData();
            }
        }

        private DateTime SelectedDateNext { get => SelectedDate.AddDays(1); }
        public DateTime SelectedDate
        {
            get => _selectedDate; set
            {
                _selectedDate = value;
                //OnPropertyChanged("ListVisit");
                RefreshData();
            }
        }

        public VmVisit SelectedVisit { get; set; }
        public ObservableCollection<VmVisit> ListVisit { get; set; }
            = new ObservableCollection<VmVisit>();
        public VisitViewModel() : base()
        {
            SetTimer();
            SelectedDate = DateTime.Today;
            RefreshData();
        }
        private void SetTimer()
        {
            // Create a timer with a two second interval.
            timer = new System.Timers.Timer(60000);
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            bool closestSet = false;
            ListVisit.Clear();
            IQueryable<Visit> q = ctx.Visit.Include(s => s.Person).Include(s => s.Personal)
                    .Include(s => s.VisitBenefit.Select(o => o.Benefit)).Include(s => s.Invoice)
                    .Where(s => s.Dt >= SelectedDate && s.Dt < SelectedDateNext)
                    .OrderBy(s => s.Dt);
            if (SelectedStatus == 1)
            {
                q = q.Where(s => !s.Status);
            }
            else if (SelectedStatus == 2)
            {
                q = q.Where(s => s.Status);
            }
            foreach (var o in q)
            {
                var v = new VmVisit(o);
                if (o.Dt > DateTime.Now && !closestSet)
                {
                    v.Closest = true;
                    closestSet = true;
                }
                ListVisit.Add(v);
            }
        }

        public RelayCommand DelVisitCommand
        {
            get => new RelayCommand(DelVisit, o => o != null 
                && !((VmVisit)o).Status);
        }

        private void DelVisit(object obj)
        {
            var selectedVisit = (VmVisit)obj;
            var visit = selectedVisit.Obj;
            ctx.VisitBenefit.RemoveRange(visit.VisitBenefit);
            ctx.Visit.Remove(visit);
            ctx.SaveChanges();
            ListVisit.Remove(selectedVisit);
        }

        public RelayCommand GenerateInvoiceCommand
        {
            get => new RelayCommand(GenerateInvoice, 
                o => o != null && ((VmVisit)o).Status);
        }

        private void GenerateInvoice(object obj)
        {
            var selectedVisit = (VmVisit)obj;
            bool isDoit = true; ;
            Invoice invoice = selectedVisit.Obj.Invoice.SingleOrDefault(s => true);
            if (invoice != null)
            {
                if (invoice.Status != enumStatusInvoice.Неоплачен)
                {
                    System.Windows.MessageBox
                        .Show("Счет уже существует и оплачивался. Переформировать нельзя.");
                    isDoit = false;
                }
                else
                {
                    if (System.Windows.MessageBox
                        .Show("Счет уже существует. Переформировать?", "", 
                        System.Windows.MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
                    {
                        ctx.InvoiceDetail.RemoveRange(invoice.InvoiceDetail);
                        ctx.Invoice.Remove(invoice);
                    }
                    else
                    {
                        isDoit = false;
                    }
                }
            }
            if (isDoit)
            {
                var newInvoice = new Invoice()
                {
                    Dt = DateTime.Now,
                    Person = selectedVisit.Person,
                    Status = enumStatusInvoice.Неоплачен,
                    Visit = selectedVisit.Obj,
                    Sm = selectedVisit.VisitBenefit.Sum(s => s.Kol * s.Benefit.Price),
                };
                foreach (var o in selectedVisit.VisitBenefit)
                {
                    newInvoice.InvoiceDetail.Add(new InvoiceDetail()
                    {
                        BenefitName = o.Benefit.Name,
                        Kol = o.Kol,
                        Price = o.Benefit.Price,
                        Sm = o.Kol * o.Benefit.Price
                    });
                }
                ctx.Invoice.Add(newInvoice);
                ctx.SaveChanges();
                System.Windows.MessageBox
                        .Show($"Сформирован счет № {newInvoice.Id}");
            }
        }

        public RelayCommand MoveVisitCommand
        {
            get => new RelayCommand(MoveVisit, o => o != null && !(o as VmVisit).Status);
        }

        private void MoveVisit(object obj)
        {
            var selectedVisit = obj as VmVisit;
            var modelView
                = new SelectIntervalViewModel(selectedVisit.VisitBenefit.Select(s => s.Benefit).ToList(), selectedVisit.Duration);
            var f = new SelectInterval() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                var selectedPersonalId = modelView.ReturnInterval.Parent.Parent.PersonalId;
                selectedVisit.Personal = ctx.Personal.Find(selectedPersonalId);
                selectedVisit.Dt = modelView.ReturnInterval.Begin;
                ctx.SaveChanges();
                RefreshData();
            }
        }

        public RelayCommand ChangeStatusCommand
        {
            get => new RelayCommand(ChangeStatus, o => o != null 
                && !ctx.Invoice.Any(s => s.VisitId == ((VmVisit)o).Id));
        }

        private void ChangeStatus(object obj)
        {
            ctx.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                ctx.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
