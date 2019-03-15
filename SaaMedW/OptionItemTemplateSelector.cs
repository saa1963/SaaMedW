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
            var nv = item as Options;
            FrameworkElement element = container as FrameworkElement;
            if (element != null && item != null)
            {
                var tp = Options.ВсеВидыПараметров[nv.ParameterType].type;
                if (tp.Equals(typeof(int)))
                    return element.FindResource("IntDataTemplate") as DataTemplate;
                else if (tp.Equals(typeof(decimal)))
                    return element.FindResource("DecimalDataTemplate") as DataTemplate;
                else if (tp.Equals(typeof(string)))
                {
                    var nLine = nv.ParameterValue.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Count();
                    if (nLine <= 1)
                    {
                        return element.FindResource("TextDataTemplate") as DataTemplate;
                    }
                    else
                        return element.FindResource("MultiLineTextDataTemplate") as DataTemplate;
                }
                else if (tp.Equals(typeof(DateTime)))
                    return element.FindResource("DateTimeDataTemplate") as DataTemplate;
                else if (tp.Equals(typeof(System.IO.Path)))
                    return element.FindResource("PathDataTemplate") as DataTemplate;
                else if (tp.IsSubclassOf(typeof(Enum)))
                    return element.FindResource("EnumDataTemplate") as DataTemplate;
                else
                    return null;
            }
            else
                return null;
        }
    }
}
