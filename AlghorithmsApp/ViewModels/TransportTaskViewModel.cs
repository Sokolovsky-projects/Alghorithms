using AlghorithmsApp.Models.TransportTask;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alghorithms.TransportTask;
using AlghorithmsApp.Models;
using System.Windows;

namespace AlghorithmsApp.ViewModels
{
    class TransportTaskViewModel : BaseViewModel
    {
        public TransportTaskViewModel()
        {
            textBoxDataTable = new ObservableCollection<TextBoxDataGroup>();
            textBoxDataTableAnswer = new ObservableCollection<TextBoxDataGroup>();

            providersGroup = new ObservableCollection<Models.TextBoxData>();
            consumersGroup = new ObservableCollection<Models.TextBoxData>();

            providersCount = 3;
            consumersCount = 3;

            UpdateView();
        }

        private int providersCount;
        public int ProviderCount
        {
            get
            {
                return providersCount;
            }
            set
            {
                providersCount = value;
                OnPropertyChanged(nameof(ProviderCount));
            }
        }

        private ObservableCollection<Models.TextBoxData> providersGroup;
        public ObservableCollection<Models.TextBoxData> ProvidersGroup
        {
            get
            {
                return providersGroup;
            }
            set
            {
                providersGroup = value;
                OnPropertyChanged(nameof(ProvidersGroup));
            }
        }


        private int consumersCount;
        public int ConsumersCount
        {
            get
            {
                return consumersCount;
            }
            set
            {
                consumersCount = value;
                OnPropertyChanged(nameof(ConsumersCount));
            }
        }

        private ObservableCollection<Models.TextBoxData> consumersGroup;
        public ObservableCollection<Models.TextBoxData> ConsumersGroup
        {
            get
            {
                return consumersGroup;
            }
            set
            {
                consumersGroup = value;
                OnPropertyChanged(nameof(ConsumersGroup));
            }
        }

        private ObservableCollection<TextBoxDataGroup> textBoxDataTable;
        public ObservableCollection<TextBoxDataGroup> TextBoxDataTable
        {
            get
            {
                return textBoxDataTable;
            }
            set
            {
                textBoxDataTable = value;
                OnPropertyChanged(nameof(TextBoxDataTable));
            }
        }

        private ObservableCollection<TextBoxDataGroup> textBoxDataTableAnswer;
        public ObservableCollection<TextBoxDataGroup> TextBoxDataTableAnswer
        {
            get
            {
                return textBoxDataTableAnswer;
            }
            set
            {
                textBoxDataTableAnswer = value;
                OnPropertyChanged(nameof(TextBoxDataTableAnswer));
            }
        }

        private ActionCommand updateViewCommand;
        public ActionCommand UpdateViewCommand
        {
            get
            {
                if (updateViewCommand == null)
                {
                    updateViewCommand = new ActionCommand(UpdateView);
                }

                return updateViewCommand;
            }
            set
            {
                updateViewCommand = value;
                OnPropertyChanged(nameof(updateViewCommand));
            }
        }

        private void UpdateView()
        {
            textBoxDataTable.Clear();

            for (int i = 0; i < providersCount; i++)
            {
                TextBoxDataGroup group = new TextBoxDataGroup();

                for (int j = 0; j < consumersCount; j++)
                {
                    group.TextBoxDatas.Add(new Models.TextBoxData());
                }

                TextBoxDataTable.Add(group);
            }

            OnPropertyChanged(nameof(TextBoxDataTable));

            //providers

            providersGroup.Clear();

            for (int i = 0; i < providersCount; i++)
            {
                ProvidersGroup.Add(new Models.TextBoxData());
            }
            OnPropertyChanged(nameof(ProvidersGroup));

            //consumers

            consumersGroup.Clear();

            for(int i = 0; i < consumersCount; i++)
            {
                ConsumersGroup.Add(new Models.TextBoxData());
            }
            OnPropertyChanged(nameof(ProvidersGroup));

        }

        private ActionCommand solveCommand;
        public ActionCommand SolveCommand
        {
            get
            {
                if (solveCommand == null)
                {
                    solveCommand = new ActionCommand(Solve);
                }

                return solveCommand;
            }
            set
            {
                solveCommand = value;
                OnPropertyChanged(nameof(solveCommand));
            }
        }

        private void Solve()
        {
            try
            {
                double[,] costMatrix = new double[providersCount, consumersCount];

                for (int i = 0; i < providersCount; i++)
                {
                    for (int j = 0; j < consumersCount; j++)
                    {
                        costMatrix[i, j] = double.Parse(TextBoxDataTable[i].TextBoxDatas[j].Text);
                    }
                }

                double[] supply = new double[providersCount];
                int k = 0;
                foreach (TextBoxData data in providersGroup)
                {
                    supply[k] = double.Parse(data.Text);
                    k++;
                }

                double[] demand = new double[consumersCount];
                k = 0;
                foreach (TextBoxData data in consumersGroup)
                {
                    demand[k] = double.Parse(data.Text);
                    k++;
                }

                TransportTaskSolver solver = new TransportTaskSolver(costMatrix, supply, demand);

                double[,] answer = solver.MinimumCostMethod();

                TextBoxDataTableAnswer.Clear();
                for (int i = 0; i < answer.GetLength(0); i++)
                {
                    TextBoxDataGroup data = new TextBoxDataGroup();
                    for (int j = 0; j < answer.GetLength(1); j++)
                    {
                        TextBoxData text = new TextBoxData();
                        text.Text = answer[i, j].ToString();
                        data.TextBoxDatas.Add(text);
                    }
                    TextBoxDataTableAnswer.Add(data);
                }
                OnPropertyChanged(nameof(TextBoxDataTableAnswer));
                TotalCostText = "Итоговая стоимость = " + solver.CalculateTotalCost(answer).ToString();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string total;
        public string TotalCostText
        {
            get { return total; }
            set
            {
                total = value;
                OnPropertyChanged(nameof(TotalCostText));
            }
        }
    }
}
