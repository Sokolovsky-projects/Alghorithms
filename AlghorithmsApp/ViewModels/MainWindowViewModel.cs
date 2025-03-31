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

        private ActionCommand transportTaskPageCommand;
        public ActionCommand TransportTaskPageCommand
        {
            get 
            {
                if(transportTaskPageCommand == null)
                {
                    transportTaskPageCommand = new ActionCommand(TransportPage);
                }

                return transportTaskPageCommand;
            }
        }

        private void TransportPage()
        {
            CurrentPage = new TransportTaskPage();
        }

        private ActionCommand primaCommand;
        public ActionCommand PrimaCommand
        {
            get 
            {
                if (primaCommand == null)
                {
                    primaCommand = new ActionCommand(Prima);
                }
                return primaCommand;
            }
        }

        private void Prima()
        {
            CurrentPage = new Prima();
        }

        private ActionCommand dynamicCommand;
        public ActionCommand DynamicCommand
        {
            get
            {
                if(dynamicCommand == null)
                {
                    dynamicCommand = new ActionCommand(Dynamic);
                }

                return dynamicCommand;
            }
        }

        private void Dynamic() 
        {
            CurrentPage = new DynamicPage();
        }

        private ActionCommand gameTheoryCommand;
        public ActionCommand GameTheoryCommand
        {
            get 
            {
                if(gameTheoryCommand == null)
                {
                    gameTheoryCommand = new ActionCommand(GameTheory);
                }

                return gameTheoryCommand;
            }
        }

        private void GameTheory()
        {
            CurrentPage = new GameTheoryPage();
        }
    }
}
