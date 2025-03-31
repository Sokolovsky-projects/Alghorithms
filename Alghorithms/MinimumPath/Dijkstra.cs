using Alghorithms.PrimsAlghorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alghorithms.MinimumPath
{
    public class Dijkstra
    {
        private List<Edge> graph;
        private Dictionary<int, List<Edge>> adjList;

        public Dijkstra(List<Edge> edges)
        {
            graph = edges;
            adjList = new Dictionary<int, List<Edge>>();

            // Строим список смежности
            foreach (var edge in edges)
            {
                if (!adjList.ContainsKey(edge.From))
                    adjList[edge.From] = new List<Edge>();
                if (!adjList.ContainsKey(edge.To))
                    adjList[edge.To] = new List<Edge>();

                adjList[edge.From].Add(edge);
            }
        }

        public Dictionary<int, int> FindShortestPath(int start)
        {
            var distances = new Dictionary<int, int>();
            var priorityQueue = new SortedSet<(int distance, int vertex)>();

            // Инициализация расстояний
            foreach (var vertex in adjList.Keys)
            {
                distances[vertex] = int.MaxValue;
            }
            distances[start] = 0;

            priorityQueue.Add((0, start));

            while (priorityQueue.Count > 0)
            {
                var (currentDist, currentVertex) = priorityQueue.Min;
                priorityQueue.Remove(priorityQueue.Min);

                if (currentDist > distances[currentVertex])
                    continue;

                // Обрабатываем все смежные вершины (исходящие ребра)
                foreach (var edge in adjList[currentVertex])
                {
                    int neighbor = edge.To;
                    int newDist = currentDist + edge.Weight;

                    if (newDist < distances[neighbor])
                    {
                        // Удаляем старое значение из очереди, если оно существует
                        priorityQueue.Remove((distances[neighbor], neighbor));
                        distances[neighbor] = newDist;
                        priorityQueue.Add((newDist, neighbor));
                    }
                }

                // Для ненаправленного графа: обрабатываем входящие ребра
                foreach (var vertex in adjList.Keys)
                {
                    foreach (var edge in adjList[vertex])
                    {
                        if (edge.To == currentVertex)
                        {
                            int neighbor = vertex; // Это вершина, из которой идет ребро к currentVertex
                            int newDist = currentDist + edge.Weight;

                            if (newDist < distances[neighbor])
                            {
                                priorityQueue.Remove((distances[neighbor], neighbor));
                                distances[neighbor] = newDist;
                                priorityQueue.Add((newDist, neighbor));
                            }
                        }
                    }
                }
            }

            return distances;
        }
    }
}
