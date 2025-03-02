using Alghorithms.PrimsAlghorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alghorithms.KruskalsAlghorithm
{
    public class KruskalSolver
    {
        public List<Edge> minEdges = new List<Edge>();

        public int FindMinimumSpanningTree(List<Edge> edges, int vertexCount)
        {
            // Сортируем рёбра по весу
            edges.Sort((a, b) => a.Weight.CompareTo(b.Weight));

            // Инициализация DSU (Система непересекающихся множеств)
            int[] parent = new int[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                parent[i] = i;
            }

            int mstWeight = 0;
            int edgesAdded = 0;

            // Проходим по всем рёбрам
            foreach (var edge in edges)
            {
                int rootFrom = Find(parent, edge.From);
                int rootTo = Find(parent, edge.To);

                // Если рёбра не образуют цикл, добавляем их в остов
                if (rootFrom != rootTo)
                {
                    mstWeight += edge.Weight;
                    minEdges.Add(edge);
                    Union(parent, rootFrom, rootTo);
                    edgesAdded++;

                    // Если добавлено (vertexCount - 1) рёбер, остов построен
                    if (edgesAdded == vertexCount - 1)
                    {
                        break;
                    }
                }
            }

            return mstWeight;
        }

        // Метод для поиска корня множества
        private int Find(int[] parent, int vertex)
        {
            if (parent[vertex] != vertex)
            {
                parent[vertex] = Find(parent, parent[vertex]);
            }
            return parent[vertex];
        }

        // Метод для объединения двух множеств
        private void Union(int[] parent, int x, int y)
        {
            int rootX = Find(parent, x);
            int rootY = Find(parent, y);
            parent[rootX] = rootY;
        }
    }
}
