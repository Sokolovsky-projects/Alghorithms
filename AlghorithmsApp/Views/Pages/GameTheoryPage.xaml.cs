using Alghorithms.GameTheory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace AlghorithmsApp.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для GameTheoryPage.xaml
    /// </summary>
    public partial class GameTheoryPage : Page
    {

        private GameTheorySolver _solver;

        public GameTheoryPage()
        {
            InitializeComponent();
            InitializeMatrices(2, 2); // Default 2x2 matrices
        }

        private void InitializeMatrices(int rows, int cols)
        {
            var matrix1 = new ObservableCollection<MatrixRow>();
            var matrix2 = new ObservableCollection<MatrixRow>();

            for (int i = 0; i < rows; i++)
            {
                var values1 = new ObservableCollection<double>();
                var values2 = new ObservableCollection<double>();

                for (int j = 0; j < cols; j++)
                {
                    values1.Add(0);
                    values2.Add(0);
                }

                matrix1.Add(new MatrixRow { Key = $"Strategy {i + 1}", Values = values1 });
                matrix2.Add(new MatrixRow { Key = $"Strategy {i + 1}", Values = values2 });
            }

            dgPlayer1.ItemsSource = matrix1;
            dgPlayer2.ItemsSource = matrix2;
        }

        private void BtnCreateMatrices_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int rows = int.Parse(txtRows.Text);
                int cols = int.Parse(txtCols.Text);
                InitializeMatrices(rows, cols);
                txtResults.Text = $"Created {rows}x{cols} payoff matrices for both players.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating matrices: {ex.Message}", "Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private GameTheorySolver CreateSolver()
        {
            var matrix1 = ConvertDataGridToMatrix(dgPlayer1);
            var matrix2 = ConvertDataGridToMatrix(dgPlayer2);
            return new GameTheorySolver(matrix1, matrix2);
        }

        private double[,] ConvertDataGridToMatrix(DataGrid dataGrid)
        {
            var rows = ((ObservableCollection<MatrixRow>)dataGrid.ItemsSource).Count;
            var cols = ((ObservableCollection<MatrixRow>)dataGrid.ItemsSource)[0].Values.Count;
            var matrix = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                var row = ((ObservableCollection<MatrixRow>)dataGrid.ItemsSource)[i];
                for (int j = 0; j < cols; j++)
                {
                    matrix[i, j] = row.Values[j];
                }
            }

            return matrix;
        }

        private void BtnFindNash_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _solver = CreateSolver();
                var result = _solver.FindNashEquilibrium();

                if (result.Item1 == -1 && result.Item2 == -1)
                {
                    txtResults.Text = "Nash Equilibrium not found.";
                }
                else
                {
                    txtResults.Text = $"Nash Equilibrium found at:\n" +
                                    $"Player 1 Strategy: {result.Item1 + 1}\n" +
                                    $"Player 2 Strategy: {result.Item2 + 1}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finding Nash Equilibrium: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDominantStrategies_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _solver = CreateSolver();
                var strategies = _solver.FindDominantStrategiesForPlayer1();

                var results = new List<KeyValuePair<string, string>>();
                if (strategies.Length == 0)
                {
                    results.Add(new KeyValuePair<string, string>("Dominant Strategies", "None found"));
                }
                else
                {
                    foreach (var strategy in strategies)
                    {
                        results.Add(new KeyValuePair<string, string>(
                            "Dominant Strategy",
                            $"Strategy {strategy + 1}"));
                    }
                }

                lstResults.ItemsSource = results;
                txtResults.Text = $"Found {strategies.Length} dominant strategies for Player 1.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finding dominant strategies: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnMixedStrategy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _solver = CreateSolver();
                var probabilities = _solver.SolveMixedStrategyForPlayer1();

                var results = new List<KeyValuePair<string, string>>();
                for (int i = 0; i < probabilities.Length; i++)
                {
                    results.Add(new KeyValuePair<string, string>(
                        $"Strategy {i + 1} Probability",
                        probabilities[i].ToString("P2")));
                }

                lstResults.ItemsSource = results;
                txtResults.Text = "Mixed strategy probabilities for Player 1:";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error solving mixed strategy: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnExpectedPayoff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _solver = CreateSolver();
                var probabilities = _solver.SolveMixedStrategyForPlayer1();
                var payoff = _solver.CalculateExpectedPayoffForPlayer1(probabilities);

                lstResults.ItemsSource = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Expected Payoff", payoff.ToString("F2"))
                };

                txtResults.Text = $"Expected payoff for Player 1 with mixed strategy: {payoff:F2}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error calculating expected payoff: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnPrintMatrices_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _solver = CreateSolver();
                _solver.PrintPayoffMatrices();
                txtResults.Text = "Matrices printed to debug output (View → Output in Visual Studio).";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing matrices: {ex.Message}", "Error",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class MatrixRow
    {
        public string Key { get; set; }
        public ObservableCollection<double> Values { get; set; } = new ObservableCollection<double>();
    }

    public class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value as string, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                return result;
            }
            return 0.0;
        }
    }
}


