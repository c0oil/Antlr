﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Input;

namespace Antlr.ViewModel
{
    public abstract class ViewModelBase : DependencyObject, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        public static ICommand GetDelegateCommand<T>(ref ICommand existingCommand, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            return existingCommand ?? (existingCommand = new DelegateCommand<T>(action) { IsEnabled = true });
        }

        public static bool? ShowDialog<T>(Action<T> additionalInitializationAction) where T : Window, new()
        {
            T newDialog = new T();
            additionalInitializationAction(newDialog);
            return ShowDialog(() => newDialog);
        }

        public static bool? ShowDialog<T>(Func<T> createDelegate) where T : Window
        {
            T newDialog = createDelegate();
            newDialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            newDialog.Owner = UiHelper.FindLastActiveWindow();
            UiHelper.HidePreviousWindow(newDialog, newDialog.Owner);
            return newDialog.ShowDialog();
        }

        #region Close

        public Action<bool?> OnCloseView;

        public void CloseView(bool? result)
        {
            OnCloseView?.Invoke(result);
        }

        #endregion
    }
}
