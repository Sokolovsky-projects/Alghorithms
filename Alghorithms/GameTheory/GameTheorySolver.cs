using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alghorithms.GameTheory
{
    public class GameTheorySolver
    {
        // Матрицы выигрыша для двух игроков (Игрок 1 и Игрок 2)
        private double[,] payoffMatrixPlayer1;
        private double[,] payoffMatrixPlayer2;

        // Конструктор
        public GameTheorySolver(double[,] matrix1, double[,] matrix2)
        {
            if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1))
                throw new ArgumentException("Размерности матриц должны совпадать.");

            payoffMatrixPlayer1 = matrix1;
            payoffMatrixPlayer2 = matrix2;
        }

        // Функция для нахождения равновесия Нэша (в простейшем виде)
        public (int, int) FindNashEquilibrium()
        {
            int rows = payoffMatrixPlayer1.GetLength(0);
            int cols = payoffMatrixPlayer1.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // Проверка для игрока 1
                    bool bestForPlayer1 = true;
                    for (int k = 0; k < rows; k++)
                    {
                        if (payoffMatrixPlayer1[k, j] > payoffMatrixPlayer1[i, j])
                        {
                            bestForPlayer1 = false;
                            break;
                        }
                    }

                    // Проверка для игрока 2
                    bool bestForPlayer2 = true;
                    for (int l = 0; l < cols; l++)
                    {
                        if (payoffMatrixPlayer2[i, l] > payoffMatrixPlayer2[i, j])
                        {
                            bestForPlayer2 = false;
                            break;
                        }
                    }

                    if (bestForPlayer1 && bestForPlayer2)
                    {
                        return (i, j); // Возвращаем индексы равновесия Нэша
                    }
                }
            }

            return (-1, -1); // Равновесие Нэша не найдено
        }

        // Вывод матриц для проверки
        public void PrintPayoffMatrices()
        {
            Console.WriteLine("Матрица выигрышей Игрока 1:");
            PrintMatrix(payoffMatrixPlayer1);

            Console.WriteLine("Матрица выигрышей Игрока 2:");
            PrintMatrix(payoffMatrixPlayer2);
        }

        private void PrintMatrix(double[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        // Метод для поиска доминирующих стратегий для Игрока 1
        public int[] FindDominantStrategiesForPlayer1()
        {
            int rows = payoffMatrixPlayer1.GetLength(0);
            int cols = payoffMatrixPlayer1.GetLength(1);
            var dominantStrategies = new List<int>();

            for (int i = 0; i < rows; i++)
            {
                bool isDominant = true;

                // Проверяем, что стратегия i доминирует над всеми другими
                for (int j = 0; j < rows; j++)
                {
                    if (i == j) continue;

                    bool dominatesAllColumns = true;
                    for (int k = 0; k < cols; k++)
                    {
                        if (payoffMatrixPlayer1[i, k] < payoffMatrixPlayer1[j, k])
                        {
                            dominatesAllColumns = false;
                            break;
                        }
                    }

                    if (!dominatesAllColumns)
                    {
                        isDominant = false;
                        break;
                    }
                }

                if (isDominant)
                {
                    dominantStrategies.Add(i);
                }
            }

            return dominantStrategies.ToArray();
        }

        public double[] SolveMixedStrategyForPlayer1()
        {
            int rows = payoffMatrixPlayer1.GetLength(0);
            int cols = payoffMatrixPlayer1.GetLength(1);

            double[] probabilities = new double[rows];

            // Для простоты допустим, что игра состоит из двух стратегий у игрока 2 (простейший случай)
            double p1 = 1.0 / (payoffMatrixPlayer1[0, 0] - payoffMatrixPlayer1[1, 0] - payoffMatrixPlayer1[0, 1]);
            double p2 = 1 - p1;

            return new double[] { p1, p2 }; // возвращаем вероятности смешанной стратегии для игрока 1
        }

        public double CalculateExpectedPayoffForPlayer1(double[] player1MixedStrategy, double[] player2MixedStrategy = null)
        {
            int rows = payoffMatrixPlayer1.GetLength(0);
            int cols = payoffMatrixPlayer1.GetLength(1);

            // Если стратегия второго игрока не указана, считаем равномерное распределение
            if (player2MixedStrategy == null)
            {
                player2MixedStrategy = new double[cols];
                for (int j = 0; j < cols; j++)
                    player2MixedStrategy[j] = 1.0 / cols;
            }

            double expectedPayoff = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    expectedPayoff += payoffMatrixPlayer1[i, j] * player1MixedStrategy[i] * player2MixedStrategy[j];
                }
            }

            return expectedPayoff;
        }
    }
}
