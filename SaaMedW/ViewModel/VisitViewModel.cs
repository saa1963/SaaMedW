using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SaaMedW.View;

namespace SaaMedW.ViewModel
{
    public class VisitViewModel : ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private DateTime _selectedDate;

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
            SelectedDate = DateTime.Today;
            RefreshData();
        }

        private void RefreshData()
        {
            ListVisit.Clear();
            var q = ctx.Visit.Include(s => s.Person).Include(s => s.Personal)
                .Include(s => s.VisitBenefit.Select(o => o.Benefit)).Include(s => s.Invoice)
                .Where(s => s.Dt >= SelectedDate && s.Dt < SelectedDateNext)
                .OrderBy(s => s.Dt);
            foreach (var o in q)
            {
                ListVisit.Add(new VmVisit(o));
            }
        }

        public RelayCommand EditVisitCommand
        {
            get => new RelayCommand(EditVisit, o => SelectedVisit != null);
        }

        private void EditVisit(object obj)
        {
            var visit = SelectedVisit.Obj;
            var modelView = new EditOneVisitViewModel(visit);
            var f = new EditOneVisitView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                SelectedVisit.Dt = modelView.Dt.Date.AddHours(modelView.H1).AddMinutes(modelView.M1);
                SelectedVisit.Duration = modelView.Duration;
                SelectedVisit.PersonalId = modelView.PersonalId;
                SelectedVisit.PersonId = modelView.PersonId;
                SelectedVisit.Status = modelView.Status;
                ctx.VisitBenefit.RemoveRange(visit.VisitBenefit);
                SelectedVisit.VisitBenefit.Clear();
                foreach (var o in modelView.VisitBenefit)
                {
                    SelectedVisit.VisitBenefit.Add(new VisitBenefit()
                    {
                        BenefitId = o.BenefitId,
                        Kol = o.Kol,
                        Status = o.Status
                    });
                }
                ctx.SaveChanges();
            }
        }

        public RelayCommand DelVisitCommand
        {
            get => new RelayCommand(DelVisit, o => SelectedVisit != null);
        }

        private void DelVisit(object obj)
        {
            var visit = SelectedVisit.Obj;
            ctx.VisitBenefit.RemoveRange(visit.VisitBenefit);
            ctx.Visit.Remove(visit);
            ctx.SaveChanges();
            ListVisit.Remove(SelectedVisit);
        }

        public RelayCommand GenerateInvoiceCommand
        {
            get => new RelayCommand(GenerateInvoice, o => SelectedVisit != null && SelectedVisit.Status == enVisitStatus.Завершен);
        }

        private void GenerateInvoice(object obj)
        {
            bool isDoit = true; ;
            Invoice invoice = SelectedVisit.Obj.Invoice.SingleOrDefault(s => true);
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
                    Person = SelectedVisit.Person,
                    Status = enumStatusInvoice.Неоплачен,
                    Visit = SelectedVisit.Obj,
                    Sm = SelectedVisit.VisitBenefit.Sum(s => s.Kol * s.Benefit.Price),
                };
                foreach (var o in SelectedVisit.VisitBenefit)
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
    }
}
