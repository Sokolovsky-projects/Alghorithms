using AlghorithmsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlghorithmsApp.Models
{
    class TextBoxData : BaseViewModel
    {
        private string text;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value, nameof(Text)); }
        }

        public ObservableCollection<string> ConstraintsTypes { get; } = new ObservableCollection<string>
        {
            "less",
            "more",
            "equal"
        };

        public TextBoxData() 
        {
            Text = string.Empty;

            OnPropertyChanged(nameof(ConstraintsTypes));
        }
    }
}
