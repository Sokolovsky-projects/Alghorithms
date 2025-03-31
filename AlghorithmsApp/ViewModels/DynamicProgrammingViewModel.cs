using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alghorithms.DynamicProgramming;
using Microsoft.Xaml.Behaviors.Core;

namespace AlghorithmsApp.ViewModels
{
    public class DynamicProgrammingViewModel : BaseViewModel
    {
        public DynamicProgrammingViewModel() 
        {

        }

        private int fibonacciNumber;
        public int FibonacciNumber
        {
            get
            {
                return fibonacciNumber;
            }
            set
            {
                fibonacciNumber = value;
                OnPropertyChanged(nameof(FibonacciNumber));
            }
        }

        private long fibonacciAnswer;
        public long FibonacciAnswer
        {
            get
            {
                return fibonacciAnswer;
            }
            set
            {
                fibonacciAnswer = value;
                OnPropertyChanged(nameof(FibonacciAnswer));
            }
        }

        private ActionCommand solveFibonacci;
        public ActionCommand SolveFibonacci
        {
            get
            {
                if (solveFibonacci == null)
                {
                    solveFibonacci = new ActionCommand(Solve);
                }

                return solveFibonacci;
            }
        }

        private void Solve()
        {
            FibonacciNumbersFinder fibonacciNumbersFinder = new FibonacciNumbersFinder();

            FibonacciAnswer = fibonacciNumbersFinder.GetFibonacci(FibonacciNumber);
        }
    }
}
