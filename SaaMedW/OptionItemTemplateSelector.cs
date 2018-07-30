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
            if (item is int)
                return IntDataTemplate;
            else if (item is decimal)
                return DateTimeDataTemplate;
            else if (item is string)
                return TextDataTemplate;
            else if (item is DateTime)
                return DateTimeDataTemplate;
            else
                return null;
        }
    }
}
