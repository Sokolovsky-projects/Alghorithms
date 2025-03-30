using AlghorithmsApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlghorithmsApp.Models.TransportTask
{
    class TextBoxDataGroup : BaseViewModel
    {
        private ObservableCollection<TextBoxData> textBoxDatas;
        public ObservableCollection<TextBoxData> TextBoxDatas
        {
            get
            {
                return textBoxDatas;
            }
            set 
            {
                textBoxDatas = value;
                OnPropertyChanged(nameof(TextBoxDatas));
            }
        }

        public TextBoxDataGroup()
        {
            textBoxDatas = new ObservableCollection<TextBoxData>();
        }
    }
}
