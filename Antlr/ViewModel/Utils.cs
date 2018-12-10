using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Antlr.ViewModel
{
    public static class UiHelper
    {
        public static Window FindLastActiveWindow()
        {
            return Application.Current.Windows.Cast<Window>().LastOrDefault(window => window.IsActive);
        }

        public static void HidePreviousWindow(Window formToShow, Window formToHide){
            if (formToHide != null && formToHide != formToShow)
            {
                formToShow.Loaded += (s, e) => formToHide.Visibility = Visibility.Collapsed;
                formToShow.Closed += (s, e) => formToHide.Visibility = Visibility.Visible;
            }
        }
    }
}
