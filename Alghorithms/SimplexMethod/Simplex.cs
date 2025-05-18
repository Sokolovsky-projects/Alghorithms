using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Alghorithms.SimplexMethod
{
    public enum SimplexResult { Unbounded, Found, NotYetFound }

    /// <summary>
    /// Класс для решения задач линейного программирования симплекс-методом 
    /// с поддержкой искусственных переменных (M-метод)
    /// </summary>
    public class Simplex
    {
        // Основные параметры задачи
        Function function;          // Целевая функция (в канонической форме)
        double[] functionVariables; // Коэффициенты целевой функции
        double[][] matrix;          // Матрица коэффициентов ограничений
        double[] b;                 // Правые части ограничений
        bool[] m;                   // Флаги искусственных переменных
        double[] M;                 // Вектор M для искусственных переменных
        double[] F;                 // Вектор коэффициентов целевой функции
        int[] C;                    // Индексы базисных переменных
        bool isMDone = false;       // Флаг завершения работы с M-коэффициентами

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="function">Целевая функция</param>
        /// <param name="constraints">Ограничения задачи</param>
        public Simplex(Function function, Constraint[] constraints)
        {
            // Приведение задачи минимизации к максимизации
            if (function.isExtrMax)
            {
                this.function = function;
            }
            else
            {
                this.function = Canonize(function);
            }

            // Построение матрицы ограничений
            getMatrix(constraints);

            // Инициализация коэффициентов целевой функции
            getFunctionArray();

            // Первоначальный расчет M и F векторов
            getMandF();

            // Корректировка F вектора
            for (int i = 0; i < F.Length; i++)
            {
                F[i] = -functionVariables[i];
            }
        }

        /// <summary>
        /// Основной метод для получения решения
        /// </summary>
        /// <returns>Результат решения и история итераций</returns>
        public Tuple<List<SimplexSnap>, SimplexResult> GetResult()
        {
            List<SimplexSnap> snaps = new List<SimplexSnap>();
            snaps.Add(new SimplexSnap(b, matrix, M, F, C, functionVariables, isMDone, m));

            // Итерационный процесс решения
            SimplexIndexResult result = nextStep();
            int i = 0;

            // Ограничение на 100 итераций для предотвращения зацикливания
            while (result.result == SimplexResult.NotYetFound && i < 100)
            {
                // Пересчет таблицы
                calculateSimplexTableau(result.index);
                snaps.Add(new SimplexSnap(b, matrix, M, F, C, functionVariables, isMDone, m));
                result = nextStep();
                i++;
            }

            return new Tuple<List<SimplexSnap>, SimplexResult>(snaps, result.result);
        }

        /// <summary>
        /// Пересчет симплекс-таблицы после выбора разрешающего элемента
        /// </summary>
        /// <param name="Xij">Координаты разрешающего элемента (строка, столбец)</param>
        void calculateSimplexTableau(Tuple<int, int> Xij)
        {
            double[][] newMatrix = new double[matrix.Length][];
            C[Xij.Item2] = Xij.Item1; // Обновление базисных переменных

            // Расчет новой строки для разрешающего столбца
            double[] newJRow = new double[matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
            {
                newJRow[i] = matrix[i][Xij.Item2] / matrix[Xij.Item1][Xij.Item2];
            }

            // Пересчет правой части
            double[] newB = new double[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                newB[i] = (i == Xij.Item2)
                    ? b[i] / matrix[Xij.Item1][Xij.Item2]
                    : b[i] - b[Xij.Item2] / matrix[Xij.Item1][Xij.Item2] * matrix[Xij.Item1][i];
            }
            b = newB;

            // Пересчет всей матрицы коэффициентов
            for (int i = 0; i < matrix.Length; i++)
            {
                newMatrix[i] = new double[C.Length];
                for (int j = 0; j < C.Length; j++)
                {
                    newMatrix[i][j] = (j == Xij.Item2)
                        ? newJRow[i]
                        : matrix[i][j] - newJRow[i] * matrix[Xij.Item1][j];
                }
            }

            matrix = newMatrix;
            getMandF(); // Обновление M и F
        }

        /// <summary>
        /// Расчет векторов M (для искусственных переменных) и F (целевая функция)
        /// </summary>
        void getMandF()
        {
            M = new double[matrix.Length];
            F = new double[matrix.Length];

            for (int i = 0; i < matrix.Length; i++)
            {
                double sumF = 0;
                double sumM = 0;

                // Суммирование коэффициентов
                for (int j = 0; j < matrix.First().Length; j++)
                {
                    if (m[C[j]]) // Для искусственных переменных
                    {
                        sumM -= matrix[i][j];
                    }
                    else // Для обычных переменных
                    {
                        sumF += functionVariables[C[j]] * matrix[i][j];
                    }
                }

                M[i] = m[i] ? sumM + 1 : sumM;
                F[i] = sumF - functionVariables[i];
            }
        }

        /// <summary>
        /// Определение следующего шага алгоритма
        /// </summary>
        /// <returns>Результат с координатами разрешающего элемента и статусом</returns>
        SimplexIndexResult nextStep()
        {
            // работа с искусственными переменными
            int columnM = getIndexOfNegativeElementWithMaxAbsoluteValue(M);

            if (isMDone || columnM == -1)
            {
                isMDone = true;

                // работа с целевой функцией
                int columnF = getIndexOfNegativeElementWithMaxAbsoluteValue(F);

                if (columnF != -1)
                {
                    int row = getIndexOfMinimalRatio(matrix[columnF], b);
                    return (row != -1)
                        ? new SimplexIndexResult(new Tuple<int, int>(columnF, row), SimplexResult.NotYetFound)
                        : new SimplexIndexResult(null, SimplexResult.Unbounded);
                }
                else
                {
                    return new SimplexIndexResult(null, SimplexResult.Found); // Решение найдено
                }
            }
            else
            {
                int row = getIndexOfMinimalRatio(matrix[columnM], b);
                return (row != -1)
                    ? new SimplexIndexResult(new Tuple<int, int>(columnM, row), SimplexResult.NotYetFound)
                    : new SimplexIndexResult(null, SimplexResult.Unbounded);
            }
        }

        // Далее идут вспомогательные методы


        int getIndexOfNegativeElementWithMaxAbsoluteValue(double[] array)
        {
            int index = -1;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] < 0)
                {
                    if (!isMDone || isMDone && !m[i])
                    {
                        if (index == -1)
                        {
                            index = i;
                        }
                        else if (Math.Abs(array[i]) > Math.Abs(array[index]))
                        {
                            index = i;
                        }
                    }

                }
            }
            return index;
        }

        int getIndexOfMinimalRatio(double[] column, double[] b)
        {
            int index = -1;

            for (int i = 0; i < column.Length; i++)
            {
                if (column[i] > 0 && b[i] > 0)
                {
                    if (index == -1)
                    {
                        index = i;
                    }
                    else if (b[i] / column[i] < b[index] / column[index])
                    {
                        index = i;
                    }
                }
            }

            return index;
        }

        public void getFunctionArray()
        {
            double[] funcVars = new double[matrix.Length];
            for (int i = 0; i < matrix.Length; i++)
            {
                funcVars[i] = i < function.variables.Length ? function.variables[i] : 0;
            }
            this.functionVariables = funcVars;
        }

        public Function Canonize(Function function)
        {
            double[] newFuncVars = new double[function.variables.Length];

            for (int i = 0; i < function.variables.Length; i++)
            {
                newFuncVars[i] = -function.variables[i];
            }
            return new Function(newFuncVars, -function.c, true);
        }

        double[][] appendColumn(double[][] matrix, double[] column)
        {
            double[][] newMatrix = new double[matrix.Length + 1][];
            for (int i = 0; i < matrix.Length; i++)
            {
                newMatrix[i] = matrix[i];
            }
            newMatrix[matrix.Length] = column;
            return newMatrix;
        }

        T[] append<T>(T[] array, T element)
        {
            T[] newArray = new T[array.Length + 1];
            for (int i = 0; i < array.Length; i++)
            {
                newArray[i] = array[i];
            }
            newArray[array.Length] = element;
            return newArray;
        }

        double[] getColumn(double value, int place, int length)
        {
            double[] newColumn = new double[length];

            for (int k = 0; k < length; k++)
            {
                newColumn[k] = k == place ? value : 0;
            }

            return newColumn;
        }

        public void getMatrix(Constraint[] constraints)
        {
            for (int i = 0; i < constraints.Length; i++)
            {
                if (constraints[i].b < 0)
                {
                    double[] cVars = new double[constraints[i].variables.Length];

                    for (int j = 0; j < constraints[i].variables.Length; j++)
                    {
                        cVars[j] = -constraints[i].variables[j];
                    }

                    string sign = constraints[i].sign;

                    if (sign == ">=")
                    {
                        sign = "<=";
                    }
                    else if (sign == "<=")
                    {
                        sign = ">=";
                    }

                    Constraint cNew = new Constraint(cVars, -constraints[i].b, sign);
                    constraints[i] = cNew;
                }
            }

            double[][] matrix = new double[constraints.First().variables.Length][];

            for (int i = 0; i < constraints.First().variables.Length; i++)
            {
                matrix[i] = new double[constraints.Length];
                for (int j = 0; j < constraints.Length; j++)
                {
                    matrix[i][j] = constraints[j].variables[i];
                }
            }

            double[][] appendixMatrix = new double[0][];
            double[] Bs = new double[constraints.Length];

            for (int i = 0; i < constraints.Length; i++)
            {
                Constraint current = constraints[i];

                Bs[i] = current.b;

                if (current.sign == ">=")
                {
                    appendixMatrix = appendColumn(appendixMatrix, getColumn(-1, i, constraints.Length));
                }
                else if (current.sign == "<=")
                {
                    appendixMatrix = appendColumn(appendixMatrix, getColumn(1, i, constraints.Length));
                }
            }

            double[][] newMatrix = new double[constraints.First().variables.Length + appendixMatrix.Length][];

            for (int i = 0; i < constraints.First().variables.Length; i++)
            {
                newMatrix[i] = matrix[i];
            }

            for (int i = constraints.First().variables.Length; i < constraints.First().variables.Length + appendixMatrix.Length; i++)
            {
                newMatrix[i] = appendixMatrix[i - constraints.First().variables.Length];
            }

            bool[] hasBasicVar = new bool[constraints.Length];

            for (int i = 0; i < constraints.Length; i++)
            {
                hasBasicVar[i] = false;
            }

            C = new int[constraints.Length];

            int ci = 0;
            for (int i = 0; i < newMatrix.Length; i++)
            {
                bool hasOnlyNulls = true;
                bool hasOne = false;
                Tuple<int, int> onePosition = new Tuple<int, int>(0, 0);
                for (int j = 0; j < constraints.Length; j++)
                {


                    if (newMatrix[i][j] == 1)
                    {
                        if (hasOne)
                        {
                            hasOnlyNulls = false;
                            break;
                        }
                        else
                        {
                            hasOne = true;
                            onePosition = new Tuple<int, int>(i, j);
                        }
                    }
                    else if (newMatrix[i][j] != 0)
                    {
                        hasOnlyNulls = false;
                        break;
                    }


                }

                if (hasOnlyNulls && hasOne)
                {
                    hasBasicVar[onePosition.Item2] = true;
                    C[ci] = onePosition.Item1;
                    ci++;
                }
            }

            m = new bool[newMatrix.Length];

            for (int i = 0; i < newMatrix.Length; i++)
            {
                m[i] = false;
            }

            for (int i = 0; i < constraints.Length; i++)
            {

                if (!hasBasicVar[i])
                {

                    double[] basicColumn = new double[constraints.Length];

                    for (int j = 0; j < constraints.Length; j++)
                    {
                        basicColumn[j] = j == i ? 1 : 0;
                    }

                    newMatrix = appendColumn(newMatrix, basicColumn);
                    m = append(m, true);
                    C[ci] = newMatrix.Length - 1;
                    ci++;
                }

            }

            this.b = Bs;
            this.matrix = newMatrix;
        }
    }
}
