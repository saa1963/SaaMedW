using SaaMedW.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SaaMedW.ViewModel
{
    public enum TypeTimeInterval
    {
        Visit, Graphic
    }
    public class VisitTimeInterval : TimeInterval
    {
        public DateIntervalsViewModel Parent { get; set; }
        public int PersonalId { get; set; }
        public DateTime Dt{ get; set; }
        public TypeTimeInterval typeTimeInterval { get; set; }
        public bool IsVisit { get => typeTimeInterval == TypeTimeInterval.Visit; }
        public int VisitId { get; set; }

        public VisitTimeInterval():base() { }
        public VisitTimeInterval(TimeInterval timeInterval):base(timeInterval)
        {
        }
        public VisitTimeInterval(DateTime dt, int h1, int m1, int h2, int m2): 
            base(dt, h1, m1, h2, m2) { }
        public RelayCommand AddVisitCommand
        {
            get { return new RelayCommand(AddVisitProc); }
        }
        private void AddVisitProc(object obj)
        {
            var root = this.Parent.Parent.Parent;
            var visit = new Visit()
            {
                Dt = this.Begin,
                Duration = this.Interval.Minutes,
                PersonId = root.SelectedPersonId,
                PersonalId = this.PersonalId,
                Status = 0
            };
            var benefit = root.ctx.Benefit.Find(root.BenefitSel.Id);
            visit.VisitBenefit.Add(new VisitBenefit() { Benefit = benefit, Kol = 1, Status = 0 });
            root.ctx.Visit.Add(visit);
            root.ctx.SaveChanges();
            this.VisitId = visit.Id;
            this.typeTimeInterval = TypeTimeInterval.Visit;
            OnPropertyChanged("typeTimeInterval");
            OnPropertyChanged("IsVisit");
        }
        public RelayCommand EditVisitCommand
        {
            get { return new RelayCommand(EditVisitProc); }
        }

        private void EditVisitProc(object obj)
        {
            var root = this.Parent.Parent.Parent;
            var visit = root.ctx.Visit.Find(VisitId);
            var modelView = new EditOneVisitViewModel(visit);
            var f = new EditOneVisitView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                visit.Dt = modelView.Dt.AddHours(modelView.H1).AddMinutes(modelView.M1);
                visit.Duration = modelView.Duration;
                visit.PersonalId = modelView.PersonalId;
                visit.PersonId = modelView.PersonId;
                visit.Status = modelView.Status;
                root.ctx.VisitBenefit.RemoveRange(visit.VisitBenefit);
                visit.VisitBenefit.Clear();
                foreach (var o in modelView.VisitBenefit)
                {
                    visit.VisitBenefit.Add(new VisitBenefit()
                        { BenefitId = o.BenefitId,
                            Kol = o.Kol, Status = o.Status });
                }
                root.ctx.SaveChanges();
            }
        }

        public RelayCommand DelVisitCommand
        {
            get { return new RelayCommand(DelVisitProc); }
        }
        private void DelVisitProc(object obj)
        {
            var root = this.Parent.Parent.Parent;
            var visit = root.ctx.Visit.Find(VisitId);
            root.ctx.VisitBenefit.RemoveRange(visit.VisitBenefit);
            visit.VisitBenefit.Clear();
            root.ctx.Visit.Remove(visit);
            root.ctx.SaveChanges();
            this.VisitId = 0;
            this.typeTimeInterval = TypeTimeInterval.Graphic;
            OnPropertyChanged("typeTimeInterval");
        }
    }
}
