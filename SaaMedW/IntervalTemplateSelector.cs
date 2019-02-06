using SaaMedW.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SaaMedW
{
    public class IntervalTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GraphicDataTemplate { get; set; }
        public DataTemplate VisitDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            VisitTimeInterval ti = (VisitTimeInterval)item;

            if (ti.TypeTimeInterv == TypeTimeInterval.Graphic)
                return GraphicDataTemplate;
            else
                return VisitDataTemplate;
        }
    }
}
