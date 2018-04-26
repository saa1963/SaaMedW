using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Threading;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MS.Utility;
using MS.Internal.KnownBoxes;
using MS.Internal.PresentationFramework;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;

namespace SaaMedW
{
    public class CloseButton : Button
    {
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
        }
        protected override void OnClick()
        {
            base.OnClick();
        }
    }
}
