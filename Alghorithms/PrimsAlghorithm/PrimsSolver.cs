using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alghorithms.PrimsAlghorithm
{
    public class PrimsSolver
    {
        public List<Edge> minEdges = new List<Edge>();

        public int FindMinimumSpanningTree(List<Edge> edges, int vertexCount)
        {
            // Создаём список смежности
            var adj = new List<List<Edge>>();
            for (int i = 0; i < vertexCount; i++)
            {
                adj.Add(new List<Edge>());
            }

            // Заполняем список смежности
            foreach (var edge in edges)
            {
                adj[edge.From].Add(new Edge(edge.From, edge.To, edge.Weight));
                adj[edge.To].Add(new Edge(edge.To, edge.From, edge.Weight));
            }

            // Приоритетная очередь для выбора рёбер с минимальным весом
            var pq = new SortedSet<Edge>(Comparer<Edge>.Create((a, b) => a.Weight == b.Weight ? a.From.CompareTo(b.From) : a.Weight.CompareTo(b.Weight)));
            var used = new bool[vertexCount];
            int mstWeight = 0;

            // Начинаем с вершины 0
            pq.Add(new Edge(-1, 0, 0));

            while (pq.Count > 0)
            {
                var minEdge = pq.Min;
                pq.Remove(minEdge);

                if (used[minEdge.To]) continue;

                used[minEdge.To] = true;
                mstWeight += minEdge.Weight;
                minEdges.Add(minEdge);

                // Добавляем все смежные рёбра
                foreach (var edge in adj[minEdge.To])
                {
                    if (!used[edge.To])
                    {
                        pq.Add(edge);
                    }
                }
            }

            return mstWeight;
        }
    }
}
