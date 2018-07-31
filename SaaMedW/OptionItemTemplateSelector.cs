using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SaaMedW
{
    public class OptionItemTemplateSelector: DataTemplateSelector
    {
        public DataTemplate TextDataTemplate { get; set; }
        public DataTemplate DateTimeDataTemplate { get; set; }
        public DataTemplate DecimalDataTemplate { get; set; }
        public DataTemplate IntDataTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (element != null && item != null)
            {
                if (item is int)
                    return element.FindResource("IntDataTemplate") as DataTemplate;
                else if (item is decimal)
                    return element.FindResource("DecimalDataTemplate") as DataTemplate;
                else if (item is string)
                    return element.FindResource("TextDataTemplate") as DataTemplate;
                else if (item is DateTime)
                    return element.FindResource("DateTimeDataTemplate") as DataTemplate;
                else
                    return null;
            }
            else
                return null;
        }
    }
}
