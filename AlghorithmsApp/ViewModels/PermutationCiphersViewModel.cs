using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using AlghorithmsApp.Models.Cipher;
using Ciphers;
using Microsoft.Xaml.Behaviors.Core;

namespace AlghorithmsApp.ViewModels
{
    class PermutationCiphersViewModel : BaseViewModel
    {
        private ObservableCollection<string> cipherDatas;
        public ObservableCollection<string> CipherDatas
        {
            get 
            {
                if (cipherDatas == null)
                {
                    cipherDatas = new ObservableCollection<string>();
                }

                return cipherDatas;
            }
            set
            {
                cipherDatas = value;
                OnPropertyChanged(nameof(CipherDatas));
            }
        }

        public PermutationCiphersViewModel()
        {
            CipherDatas = new ObservableCollection<string>()
            {
                "Блочный",
                "Трисемуса"
            };
        }

        private string currentCipher;
        public string CurrentCipher
        {
            get { return currentCipher; }
            set
            {
                currentCipher = value;
                OnPropertyChanged(nameof(CurrentCipher));
            }
        }

        private ActionCommand solveCipherCommand;
        public ActionCommand SolveCipherCommand
        {
            get
            {
                if(solveCipherCommand == null)
                {
                    solveCipherCommand = new ActionCommand(Solve);
                }

                return solveCipherCommand;
            }
        }

        private string key;
        public string Key
        {
            get
            {
                return key;
            }
            set 
            {
                key = value;
                OnPropertyChanged(nameof(Key));
            }
        }

        private string message;
        public string Message
        {
            get
            {
                return message;
            }
            set 
            {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private string solution;
        public string Solution
        {
            get
            {
                return solution;
            }
            set 
            {
                solution = value;
                OnPropertyChanged(nameof(Solution));
            }
        }

        private void Solve()
        {
            switch (CurrentCipher)
            {
                case "Блочный":
                    BlockCipher cipher = new BlockCipher(Message, Key);
                    Solution = cipher.EncryptedMessage;
                    break;
                case "Трисемуса":
                    TrisemusCipher trisemus = new TrisemusCipher(Message, Key);
                    Solution = trisemus.EncryptedMessage;
                    break;
            }
        }
    }
}
