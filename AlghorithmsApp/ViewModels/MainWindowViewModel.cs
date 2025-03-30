using AlghorithmsApp.Views.Pages;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AlghorithmsApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel() { }

        private Page _currentPage;
        public Page CurrentPage
        {
            get { return _currentPage; }
            set 
            {
                SetProperty(ref _currentPage, value, nameof(CurrentPage));
            }
        }

        private ActionCommand simplexPageCommand;
        public ActionCommand SimplexPageCommand
        {
            get
            {
                if (simplexPageCommand == null)
                {
                    simplexPageCommand = new ActionCommand(SimplexPage);
                }

                return simplexPageCommand;
            }
        }

        private void SimplexPage()
        {
            CurrentPage = new SimplexMethod();
        }
        
    }
}
