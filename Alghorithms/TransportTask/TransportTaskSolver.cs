using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Alghorithms.TransportTask
{
    public class TransportTaskSolver
    {
        private double[,] costMatrix;
        private double[] supply;
        private double[] demand;
        public double[,] ResultMatrix { get; private set; }

        public TransportTaskSolver(double[,] costMatrix, double[] supply, double[] demand)
        {
            this.costMatrix = (double[,])costMatrix.Clone();
            this.supply = (double[])supply.Clone();
            this.demand = (double[])demand.Clone();

            var initial = MinimumCostMethod();
            ResultMatrix = (double[,])initial.Clone();
            Optimize(ResultMatrix);
        }

        public double[,] MinimumCostMethod()
        {
            int m = supply.Length, n = demand.Length;
            var plan = new double[m, n];
            var costCopy = (double[,])costMatrix.Clone();
            var sCopy = (double[])supply.Clone();
            var dCopy = (double[])demand.Clone();

            while (true)
            {
                double minCost = double.MaxValue;
                int bi = -1, bj = -1;
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                        if (costCopy[i, j] < minCost && sCopy[i] > 0 && dCopy[j] > 0)
                        {
                            minCost = costCopy[i, j];
                            bi = i; bj = j;
                        }
                if (bi < 0) break;

                double q = Math.Min(sCopy[bi], dCopy[bj]);
                plan[bi, bj] = q;
                sCopy[bi] -= q;
                dCopy[bj] -= q;

                if (sCopy[bi] == 0)
                    for (int j = 0; j < n; j++)
                        costCopy[bi, j] = double.MaxValue;
                if (dCopy[bj] == 0)
                    for (int i = 0; i < m; i++)
                        costCopy[i, bj] = double.MaxValue;
            }

            return plan;
        }

        private void Optimize(double[,] sol)
        {
            int m = sol.GetLength(0), n = sol.GetLength(1);
            var isAlloc = new bool[m, n];
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    if (sol[i, j] > 0)
                        isAlloc[i, j] = true;

            FixDegeneracy(isAlloc);

            while (true)
            {
                var u = new double[m]; var v = new double[n];
                var uSet = new bool[m]; var vSet = new bool[n];
                uSet[0] = true;
                bool updated;
                do
                {
                    updated = false;
                    for (int i = 0; i < m; i++)
                        for (int j = 0; j < n; j++)
                            if (isAlloc[i, j])
                            {
                                if (uSet[i] && !vSet[j])
                                {
                                    v[j] = costMatrix[i, j] - u[i];
                                    vSet[j] = true;
                                    updated = true;
                                }
                                if (!uSet[i] && vSet[j])
                                {
                                    u[i] = costMatrix[i, j] - v[j];
                                    uSet[i] = true;
                                    updated = true;
                                }
                            }
                } while (updated);

                double minDelta = 0; int enterI = -1, enterJ = -1;
                for (int i = 0; i < m; i++)
                    for (int j = 0; j < n; j++)
                        if (!isAlloc[i, j])
                        {
                            double delta = costMatrix[i, j] - u[i] - v[j];
                            if (delta < minDelta)
                            {
                                minDelta = delta;
                                enterI = i; enterJ = j;
                            }
                        }

                if (enterI < 0) break;

                var cycle = BuildCycle(enterI, enterJ, isAlloc);
                if (cycle == null) break;

                double theta = double.MaxValue;
                for (int k = 1; k < cycle.Count; k += 2)
                {
                    var (ci, cj) = cycle[k];
                    theta = Math.Min(theta, sol[ci, cj]);
                }

                for (int k = 0; k < cycle.Count; k++)
                {
                    var (ci, cj) = cycle[k];
                    sol[ci, cj] += (k % 2 == 0 ? theta : -theta);
                    if (sol[ci, cj] <= 0 && isAlloc[ci, cj] && k % 2 == 1)
                        isAlloc[ci, cj] = false;
                }
                isAlloc[enterI, enterJ] = true;
            }
        }

        private void FixDegeneracy(bool[,] isAlloc)
        {
            int m = isAlloc.GetLength(0), n = isAlloc.GetLength(1);
            int need = m + n - 1;
            int have = isAlloc.Cast<bool>().Count(x => x);

            for (int i = 0; i < m && have < need; i++)
            {
                for (int j = 0; j < n && have < need; j++)
                {
                    if (!isAlloc[i, j])
                    {
                        int rowCount = Enumerable.Range(0, n).Count(c => isAlloc[i, c]);
                        int colCount = Enumerable.Range(0, m).Count(r => isAlloc[r, j]);
                        if (rowCount < 2 && colCount < 2)
                        {
                            isAlloc[i, j] = true;
                            have++;
                        }
                    }
                }
            }
        }

        private List<(int, int)> BuildCycle(int si, int sj, bool[,] isAlloc)
        {
            int m = isAlloc.GetLength(0), n = isAlloc.GetLength(1);
            var path = new List<(int, int)>();
            var used = new bool[m, n];

            bool Dfs(int i, int j, bool horizontal)
            {
                path.Add((i, j));
                used[i, j] = true;
                if (path.Count >= 4 && i == si && j == sj && path.Count % 2 == 0)
                    return true;

                if (horizontal)
                {
                    for (int col = 0; col < n; col++)
                    {
                        if ((col != j || path.Count == 1)
                            && (isAlloc[i, col] || (i == si && col == sj))
                            && !used[i, col])
                        {
                            if (Dfs(i, col, !horizontal))
                                return true;
                        }
                    }
                }
                else
                {
                    for (int row = 0; row < m; row++)
                    {
                        if ((row != i || path.Count == 1)
                            && (isAlloc[row, j] || (row == si && j == sj))
                            && !used[row, j])
                        {
                            if (Dfs(row, j, !horizontal))
                                return true;
                        }
                    }
                }

                path.RemoveAt(path.Count - 1);
                return false;
            }

            // попытка сначала по горизонтали
            if (Dfs(si, sj, true))
                return path;

            // очистка состояния для повторного поиска по вертикали
            path.Clear();
            for (int x = 0; x < m; x++)
                for (int y = 0; y < n; y++)
                    used[x, y] = false;

            if (Dfs(si, sj, false))
                return path;

            return null;
        }

        public double CalculateTotalCost(double[,] sol)
        {
            double total = 0;
            for (int i = 0; i < sol.GetLength(0); i++)
                for (int j = 0; j < sol.GetLength(1); j++)
                    total += sol[i, j] * costMatrix[i, j];
            return total;
        }

        public void PrintSolution(double[,] sol)
        {
            Console.WriteLine("Решение:");
            for (int i = 0; i < sol.GetLength(0); i++)
            {
                for (int j = 0; j < sol.GetLength(1); j++)
                    Console.Write($"{sol[i, j],6}");
                Console.WriteLine();
            }
            Console.WriteLine($"Общая стоимость: {CalculateTotalCost(sol)}");
        }
    }
}
