using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alghorithms.TransportTask
{
    public class TransportTaskSolver
    {
        private double[,] costMatrix; // Матрица стоимостей
        private double[] supply; // Массив поставок
        private double[] demand; // Массив спроса

        public TransportTaskSolver(double[,] costMatrix, double[] supply, double[] demand)
        {
            this.costMatrix = costMatrix;
            this.supply = supply;
            this.demand = demand;
        }

        // Метод северо-западного угла
        // На данный момент не работает
        //public double[,] NorthWestCornerMethod()
        //{
        //    int rows = supply.Length;
        //    int cols = demand.Length;
        //    double[,] result = new double[rows, cols];
        //    int i = 0, j = 0;

        //    while (i < rows && j < cols)
        //    {
        //        double allocation = Math.Min(supply[i], demand[j]);
        //        result[i, j] = allocation;
        //        supply[i] -= allocation;
        //        demand[j] -= allocation;

        //        if (supply[i] == 0) i++;
        //        if (demand[j] == 0) j++;
        //    }

        //    return result;
        //}

        // Метод минимальной стоимости
        public double[,] MinimumCostMethod()
        {
            int rows = supply.Length;
            int cols = demand.Length;
            double[,] result = new double[rows, cols];

            // Создаём копию матрицы стоимостей
            double[,] costCopy = (double[,])costMatrix.Clone();

            int i = 0, j = 0;

            while (i < rows && j < cols)
            {
                double minCost = double.MaxValue;
                int minRow = -1, minCol = -1;

                // Ищем минимальную стоимость в матрице
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        if (costCopy[r, c] < minCost && supply[r] > 0 && demand[c] > 0)
                        {
                            minCost = costCopy[r, c];
                            minRow = r;
                            minCol = c;
                        }
                    }
                }

                if (minRow == -1 || minCol == -1) break;

                double allocation = Math.Min(supply[minRow], demand[minCol]);
                result[minRow, minCol] = allocation;
                supply[minRow] -= allocation;
                demand[minCol] -= allocation;

                if (supply[minRow] == 0) costCopy[minRow, minCol] = double.MaxValue; // Закрываем строку
                if (demand[minCol] == 0) costCopy[minRow, minCol] = double.MaxValue; // Закрываем колонку
            }

            return result;
        }

        // Подсчёт стоимости для текущего распределения
        public double CalculateTotalCost(double[,] solution)
        {
            double totalCost = 0;
            int rows = supply.Length;
            int cols = demand.Length;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    totalCost += solution[i, j] * costMatrix[i, j];
                }
            }

            return totalCost;
        }

        // Вывод решения
        public void PrintSolution(double[,] solution)
        {
            int rows = supply.Length;
            int cols = demand.Length;

            Console.WriteLine("Решение транспортной задачи:");
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"{solution[i, j]} \t");
                }
                Console.WriteLine();
            }
        }
    }
}
