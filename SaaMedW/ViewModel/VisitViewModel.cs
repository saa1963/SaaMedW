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
                OnPropertyChanged("ListVisit");
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
                SelectedVisit.Dt = modelView.Dt.AddHours(modelView.H1).AddMinutes(modelView.M1);
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
            get => new RelayCommand(GenerateInvoice, o => SelectedVisit != null && SelectedVisit.Status == 1);
        }

        private void GenerateInvoice(object obj)
        {
            var modelView = new EditInvoiceViewModel(SelectedVisit);
            var f = new EditInvoiceView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {

            }
        }
    }
}
