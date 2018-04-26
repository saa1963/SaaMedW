using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SaaMedW
{
    public static class ClickCommand
    {
        public static readonly DependencyProperty CommandProperty =

        DependencyProperty.RegisterAttached("Command",
        typeof(ICommand),
        typeof(ClickCommand),
        new FrameworkPropertyMetadata(OnCommandChanged));

        public static readonly DependencyProperty CommandParameterProperty =
        DependencyProperty.RegisterAttached(
        "CommandParameter",
        typeof(object),
        typeof(ClickCommand));

        public static readonly DependencyProperty IsEnabledProperty =
        DependencyProperty.RegisterAttached(
        "IsEnabled",
        typeof(bool),
        typeof(ClickCommand));

        public static void SetCommand(DependencyObject element, ICommand value)
        {
            element.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject element)
        {
            return (ICommand)element.GetValue(CommandProperty);
        }

        public static void SetCommandParameter(DependencyObject element, object value)
        {
            element.SetValue(CommandParameterProperty, value);
        }

        public static object GetCommandParameter(DependencyObject element)
        {
            return element.GetValue(CommandParameterProperty);
        }

        public static void SetIsEnabled(DependencyObject element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }

        public static bool GetIsEnabled(DependencyObject element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }

        private static void OnCommandChanged
        (DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var frameworkElement = obj as FrameworkElement;
            frameworkElement.PreviewMouseLeftButtonUp -= PreviewMouseLeftButtonUp;
            frameworkElement.PreviewMouseLeftButtonUp += PreviewMouseLeftButtonUp;

            var command = GetCommand(frameworkElement);

            SetIsEnabled(frameworkElement, command.CanExecute(null));

            command.CanExecuteChanged += (s, e) =>
            {
                SetIsEnabled(frameworkElement, command.CanExecute(null));
            };
        }

        private static void PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var obj = sender as DependencyObject;

            var command = GetCommand(obj);

            if (command != null)
            {
                command.Execute(GetCommandParameter(obj));

                e.Handled = false;
            }
        }
    }
}
