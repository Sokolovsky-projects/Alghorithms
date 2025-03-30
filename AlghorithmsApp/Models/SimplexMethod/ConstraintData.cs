using AlghorithmsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlghorithmsApp.Models
{
    class ConstraintData : BaseViewModel
    {
        private ObservableCollection<TextBoxData> arguments;
        public ObservableCollection<TextBoxData> Arguments
        {
            get { return arguments; }
            set
            {
                arguments = value;
                OnPropertyChanged(nameof(Arguments));
            }
        }

        public ObservableCollection<string> ConstraintsTypes { get; } = new ObservableCollection<string>
        {
            "less",
            "more",
            "equal"
        };

        private string equalityType;
        public string EqualityType
        {
            get { return equalityType; }
            set 
            {
                equalityType = value;
                OnPropertyChanged(nameof(EqualityType));
            }
        }

        private double constraintNumber;
        public double ConstraintNumber
        {
            get { return constraintNumber; }
            set 
            {
                constraintNumber = value; 
                OnPropertyChanged(nameof(ConstraintNumber));
            }
        }

        public ConstraintData() 
        {
            arguments = new ObservableCollection<TextBoxData>();

            OnPropertyChanged(nameof(ConstraintsTypes));
        }
    }
}
