using System.Collections.ObjectModel;
using System.Dynamic;
using System.Runtime.InteropServices;

namespace Alghorithms.SimplexMethod
{
    /**
      <summary>
       Класс <see cref="SimplexSolver"/> предназначен для решения задач линейного программирования с помощью симплекс метода
      </summary>
    **/
    public class SimplexSolver
    {
        private double[,] A; // Матрица коэффициентов ограничений
        private double[] b;  // Вектор правых частей ограничений
        private double[] c;  // Вектор коэффициентов целевой функции
        private string optimizationType; // Максимизация или минимизация
        private List<string> constraintsTypes; // Список типов ограничений: "<=", ">=", "="

        private int numVariables; // Количество переменных
        private int numConstraints; // Количество ограничений

        // Поле для хранения решения в виде словаря
        private Dictionary<string, double> Solution { get; set; }

        private double optimalValue;
        public double OptimalValue 
        {
            get
            {
                return Math.Round(optimalValue, 2);
            }
            private set 
            {
                optimalValue = value;
            } 
        }

        public SimplexSolver(double[] c, double[,] A, double[] b, List<string> constraintsTypes, string optimizationType = "max")
        {
            this.c = c;
            this.A = A;
            this.b = b;
            this.constraintsTypes = constraintsTypes;
            this.optimizationType = optimizationType;

            this.numVariables = c.Length;
            this.numConstraints = b.Length;

            if (!ValidateInput())
            {
                throw new ArgumentException("Ввод некоректных данных");
            }

            if (optimizationType.ToLower() == "min")
            {
                // Если задача на минимизацию, меняем знак коэффициентов целевой функции
                for (int i = 0; i < c.Length; i++)
                {
                    c[i] = -c[i];
                }
            }

            // Инициализируем словарь для хранения решения
            Solution = new Dictionary<string, double>();
        }

        private bool ValidateInput()
        {
            bool valid = true;
            
            List<string> constraintsValidTypes = new List<string> 
            {
                "<=",
                ">=",
                "="
            };

            // проверка на верно введенные ограничения
            foreach (string constraintType in constraintsTypes)
            {
                if (!constraintsValidTypes.Contains(constraintType)) 
                {
                    valid = false;
                    break;
                }
            }

            return valid;
        }

        public void Solve()
        {
            // Преобразуем задачу в каноническую форму
            double[,] tableau = CreateTableau();
            int[] basicVariables = new int[numConstraints];

            for (int i = 0; i < numConstraints; i++)
            {
                basicVariables[i] = numVariables + i;
            }

            while (true)
            {
                // Печатаем текущую таблицу
                PrintTableau(tableau);

                // Шаг 1: Найдем столбец с минимальным значением в последней строке
                int pivotColumn = FindPivotColumn(tableau);
                if (pivotColumn == -1)
                    break;

                // Шаг 2: Найдем строку с минимальным значением отношения b_i / a_ij
                int pivotRow = FindPivotRow(tableau, pivotColumn);
                if (pivotRow == -1)
                    throw new Exception("Задача не имеет решения!");

                // Шаг 3: Пересчитываем симплекс-таблицу
                PerformPivoting(ref tableau, pivotRow, pivotColumn);

                // Обновляем базис
                basicVariables[pivotRow] = pivotColumn;
            }

            // Извлекаем решение в виде словаря
            for (int i = 0; i < numConstraints; i++)
            {
                if (basicVariables[i] < numVariables)
                {
                    string variableName = "x" + (basicVariables[i] + 1);
                    Solution[variableName] = tableau[i, numVariables + numConstraints];
                }
            }

            // Вычисление оптимального значения целевой функции
            OptimalValue = 0;
            for (int i = 0; i < numVariables; i++)
            {
                OptimalValue += c[i] * Solution.GetValueOrDefault("x" + (i + 1), 0);
            }
        }

        private double[,] CreateTableau()
        {
            // Количество дополнительных переменных зависит от типа ограничений
            int additionalVars = 0;
            for (int i = 0; i < numConstraints; i++)
            {
                if (constraintsTypes[i] == ">=")
                    additionalVars++;
            }

            double[,] tableau = new double[numConstraints + 1, numVariables + numConstraints + additionalVars + 1];

            // Заполнение таблицы с коэффициентами целевой функции
            for (int i = 0; i < numVariables; i++)
            {
                tableau[numConstraints, i] = -c[i];
            }

            // Заполнение таблицы с коэффициентами ограничений
            int artVarOffset = 0; // Смещение для искусственных переменных
            for (int i = 0; i < numConstraints; i++)
            {
                for (int j = 0; j < numVariables; j++)
                {
                    tableau[i, j] = A[i, j];
                }
                tableau[i, numVariables + numConstraints] = b[i];

                if (constraintsTypes[i] == ">=")
                {
                    // Преобразуем ограничение x1 + x2 >= 4 в x1 + x2 - s1 = 4
                    // Добавляем искусственную переменную (в данном случае - s1)
                    tableau[i, numVariables + numConstraints + artVarOffset] = -1;
                    artVarOffset++;
                }
                else if (constraintsTypes[i] == "=")
                {
                    // Для ограничений типа "=" не нужно добавлять искусственные переменные
                }
            }

            // Добавление искусственных переменных в целевую функцию
            for (int i = 0; i < numConstraints; i++)
            {
                if (constraintsTypes[i] == ">=")
                {
                    // Для ограничений типа >= добавляем искусственную переменную в целевую функцию
                    tableau[numConstraints, numVariables + numConstraints + i] = -1;
                }
            }

            return tableau;
        }

        private int FindPivotColumn(double[,] tableau)
        {
            int pivotColumn = -1;
            double minValue = 0;

            for (int i = 0; i < numVariables + numConstraints; i++)
            {
                if (tableau[numConstraints, i] < minValue)
                {
                    minValue = tableau[numConstraints, i];
                    pivotColumn = i;
                }
            }

            return pivotColumn;
        }

        private int FindPivotRow(double[,] tableau, int pivotColumn)
        {
            int pivotRow = -1;
            double minRatio = double.MaxValue;

            for (int i = 0; i < numConstraints; i++)
            {
                if (tableau[i, pivotColumn] > 0)
                {
                    double ratio = tableau[i, numVariables + numConstraints] / tableau[i, pivotColumn];
                    if (ratio < minRatio)
                    {
                        minRatio = ratio;
                        pivotRow = i;
                    }
                }
            }

            return pivotRow;
        }

        private void PerformPivoting(ref double[,] tableau, int pivotRow, int pivotColumn)
        {
            double pivotElement = tableau[pivotRow, pivotColumn];
            for (int i = 0; i < numVariables + numConstraints + 1; i++)
            {
                tableau[pivotRow, i] /= pivotElement;
            }

            for (int i = 0; i < numConstraints + 1; i++)
            {
                if (i != pivotRow)
                {
                    double factor = tableau[i, pivotColumn];
                    for (int j = 0; j < numVariables + numConstraints + 1; j++)
                    {
                        tableau[i, j] -= factor * tableau[pivotRow, j];
                    }
                }
            }
        }

        private void PrintTableau(double[,] tableau)
        {
            for (int i = 0; i < numConstraints + 1; i++)
            {
                for (int j = 0; j < numVariables + numConstraints + 1; j++)
                {
                    Console.Write(tableau[i, j].ToString("F2") + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public double GetSolution(string argument)
        {
            return Math.Round(Solution.GetValueOrDefault(argument, 0), 2); //округленный до 2 знаков ответ
        }
    }
}