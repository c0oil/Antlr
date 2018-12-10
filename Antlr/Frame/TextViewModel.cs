using System;
using System.IO;
using System.Windows.Input;
using Antlr.ViewModel;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;

namespace Antlr.Frame
{
    public class TextViewModel : ViewModelBase
    {
        private ICommand parseCommand;
        public ICommand ParseCommand => GetDelegateCommand<object>(ref parseCommand, OnParse);

        private string inText;
        public string InText
        {
            get { return inText; }
            set
            {
                inText = value;
                OnPropertyChanged(nameof(InText));
            }
        }

        private static void OnParse(object obj)
        {
            try
            {
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
    
}