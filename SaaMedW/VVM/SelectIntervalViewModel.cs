using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Windows;

namespace SaaMedW
{
    public class SelectIntervalViewModel: NotifyPropertyChanged
    {
        log4net.ILog log;

        public SaaMedEntities ctx { get; set; } = new SaaMedEntities();

        /// <summary>
        /// Список врачей -> Список дат по графику -> Список интервалов
        /// </summary>
        public ObservableCollection<SelectPersonalVisits> PersonalVisits { get; set; }
            = new ObservableCollection<SelectPersonalVisits>();
        /// <summary>
        /// Набор услуг
        /// </summary>
        public List<Benefit> Benefits { get; set; }
        /// <summary>
        /// Продолжительность суммы услуг
        /// </summary>
        public int Duration { get; set; }
        public SelectVisitTimeInterval ReturnInterval { get; set; }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="benefits">Набор услуг по одному направлению</param>
        public SelectIntervalViewModel(List<Benefit> benefits, int duration)
        {
            log = log4net.LogManager.GetLogger(this.GetType());
            Duration = duration;
            Benefits = benefits;
            foreach (var o in ctx.PersonalSpecialty.Include(s => s.Personal)
                .Where(ПроверкаНаСпециальность)
                .Select(s => new SelectPersonalVisits()
                { PersonalId = s.PersonalId, PersonalFio = s.Personal.Fio }))
            {
                o.Parent = this;
                o.Fill();
                if (o.DateIntervals.Count > 0)
                    PersonalVisits.Add(o);
            }
        }

        private bool ПроверкаНаСпециальность(PersonalSpecialty arg)
        {
            // это направление к которой подвязана 1-ая услуга
            Specialty specialty = ctx.Specialty.Find(Benefits[0].SpecialtyId);
            while (arg.SpecialtyId != specialty.Id && specialty.ParentId != null)
            {
                specialty = ctx.Specialty.Find(specialty.ParentId);
            }
            if (arg.SpecialtyId == specialty.Id) return true;
            else return false;
        }

        public RelayCommand SelectIntervalCommand
        {
            get { return new RelayCommand(SelectIntervalProc); }
        }

        private void SelectIntervalProc(object obj)
        {
            var ti = obj as SelectVisitTimeInterval;
            if (ti.IsVisit) return;
            ReturnInterval = ti;
            DialogResult = true;
        }

        private bool? m_DialogResult;
        public bool? DialogResult
        {
            get => m_DialogResult;
            set
            {
                m_DialogResult = value;
                OnPropertyChanged("DialogResult");
            }
        }
    }
}
