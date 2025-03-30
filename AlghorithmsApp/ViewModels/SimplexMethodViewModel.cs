using AlghorithmsApp.Models;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Alghorithms.SimplexMethod;

namespace AlghorithmsApp.ViewModels
{
    class SimplexMethodViewModel : BaseViewModel
    {
        private List<double> c = new List<double>(); //коэфициенты целевой функции
        private double[,] A = new double[100, 100]; // Матрица коэффициентов ограничений
        private List<double> b = new List<double>();  // Вектор правых частей ограничений
        private ListBox optimizationType; // Максимизация или минимизация
        private List<string> constraintsTypes = new List<string>(); // Список типов ограничений: "<=", ">=", "="

        public ObservableCollection<string> ConstraintsTypes { get; } = new ObservableCollection<string>
        {
            "less",
            "more",
            "equal"
        };

        public ObservableCollection<string> OptimizationTypes { get; } = new ObservableCollection<string>
        {
            "max",
            "min"
        };

        public SimplexMethodViewModel()
        {
            functionBoxes = new ObservableCollection<TextBoxData>();
            constraints = new ObservableCollection<ConstraintData>();

            ConstraintCount = 3;
            VariablesCount = 3;

            UpdateView();
            OnPropertyChanged(nameof(ConstraintsTypes));
            OnPropertyChanged(nameof(OptimizationTypes));
        }

        private string answer;
        public string Answer
        {
            get { return answer; }
            set 
            {
                answer = value;
                OnPropertyChanged(nameof(Answer));
            }
        }

        private string fDirection;
        public string FDirection
        {
            get { return fDirection; }
            set
            {
                fDirection = value;
                OnPropertyChanged(nameof(FDirection));
            }
        }

        private int constraintCount;
        public int ConstraintCount
        {
            get { return constraintCount; }
            set
            {
                constraintCount = value;
                OnPropertyChanged(nameof(ConstraintCount));
            }
        }

        private int variablesCount;
        public int VariablesCount
        {
            get
            {
                return variablesCount;
            }
            set
            {
                variablesCount = value;
                OnPropertyChanged(nameof(VariablesCount));
            }
        }

        private ActionCommand updateCommand;
        public ActionCommand UpdateCommand
        {
            get
            {
                if (updateCommand == null)
                {
                    updateCommand = new ActionCommand(UpdateView);
                }
                return updateCommand;
            }
        }

        private ObservableCollection<TextBoxData> functionBoxes;
        public ObservableCollection<TextBoxData> FunctionBoxes
        {
            get
            {
                return functionBoxes;
            }
            set
            {
                functionBoxes = value;
                OnPropertyChanged(nameof(FunctionBoxes));
            }
        }

        private ObservableCollection<ConstraintData> constraints;
        public ObservableCollection<ConstraintData> Constraints
        {
            get
            {
                return constraints;
            }
            set
            {
                constraints = value;
                OnPropertyChanged(nameof(Constraints));
            }
        }


        private void UpdateView()
        {
            // обновление целевой функции
            functionBoxes.Clear();

            OnPropertyChanged(nameof(VariablesCount));

            for (int i = 0; i < VariablesCount; i++)
            {
                functionBoxes.Add(new TextBoxData());
            }

            OnPropertyChanged(nameof(FunctionBoxes));

            // обновление ограничений
            constraints.Clear();

            for (int i = 0; i < ConstraintCount; i++)
            {
                ConstraintData constraint = new ConstraintData();

                for (int j = 0; j < VariablesCount; j++)
                {
                    constraint.Arguments.Add(new TextBoxData());
                }

                Constraints.Add(constraint);
            }

            OnPropertyChanged(nameof(Constraints));
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
                OnPropertyChanged(nameof(SolveCommand));
            }
        }

        private void Solve()
        {
            try
            {
                foreach (TextBoxData item in functionBoxes)
                {
                    c.Add(double.Parse(item.Text));
                }

                for (int i = 0; i < ConstraintCount; i++)
                {
                    b.Add(Constraints[i].ConstraintNumber);

                    switch (Constraints[i].EqualityType)
                    {
                        case "more":
                            constraintsTypes.Add(">=");
                            break;
                        case "less":
                            constraintsTypes.Add("<=");
                            break;
                        case "equal":
                            constraintsTypes.Add("=");
                            break;
                    }

                    for (int j = 0; j < VariablesCount; j++)
                    {
                        A[i, j] = double.Parse(Constraints[i].Arguments[j].Text);
                    }
                }

                SimplexSolver solver = new SimplexSolver(c.ToArray(), A, b.ToArray(), constraintsTypes, FDirection);

                solver.Solve();

                Dictionary<string, double> solution = solver.GetAllSolution();

                Answer = string.Empty;
                foreach (KeyValuePair<string, double> item in solution)
                {
                    Answer += $"{item.Key}={Math.Round(item.Value, 2)} ";
                }

                Answer += "F = " + solver.OptimalValue;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
