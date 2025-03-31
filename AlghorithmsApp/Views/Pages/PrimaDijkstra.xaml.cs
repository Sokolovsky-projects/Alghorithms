using System;
using System.Collections.Generic;
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
using Alghorithms.MinimumPath;

namespace AlghorithmsApp.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для Prima.xaml
    /// </summary>
    public partial class Prima : Page
    {
        private List<Edge> edges = new List<Edge>();
        private Dictionary<int, int> shortestPath;

        public Prima()
        {
            InitializeComponent();
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Пример ввода: "0,1,2; 1,2,3; 0,3,6; 1,3,8; 1,4,5; 2,4,7; 3,4,9"
                string input = InputTextBox.Text;
                edges = ParseInput(input);

                // Находим количество вершин
                int verticesCount = edges.Select(edge => Math.Max(edge.Vertex1, edge.Vertex2)).Max() + 1;

                OutputTextBlock.Text = "Минимальное остовное дерево:\n";
                Prim(edges, verticesCount);
            }
            catch (Exception ex)
            {
                OutputTextBlock.Text = "Ошибка: " + ex.Message;
            }
        }

        private List<Edge> ParseInput(string input)
        {
            List<Edge> edges = new List<Edge>();
            string[] edgeStrings = input.Split(';');

            foreach (string edgeString in edgeStrings)
            {
                string[] parts = edgeString.Split(',');
                if (parts.Length != 3)
                    throw new FormatException("Неверный формат ввода. Используйте формат: вершина1,вершина2,стоимость;...");

                int vertex1 = int.Parse(parts[0]);
                int vertex2 = int.Parse(parts[1]);
                int cost = int.Parse(parts[2]);

                edges.Add(new Edge { Vertex1 = vertex1, Vertex2 = vertex2, Cost = cost });
            }

            return edges;
        }

        private void Prim(List<Edge> edges, int verticesCount)
        {
            // Создаем список смежности
            List<Edge>[] adjacencyList = new List<Edge>[verticesCount];
            for (int i = 0; i < verticesCount; i++)
                adjacencyList[i] = new List<Edge>();

            // Заполняем список смежности
            foreach (var edge in edges)
            {
                adjacencyList[edge.Vertex1].Add(edge);
                adjacencyList[edge.Vertex2].Add(new Edge { Vertex1 = edge.Vertex2, Vertex2 = edge.Vertex1, Cost = edge.Cost });
            }

            // Алгоритм Прима
            int[] parent = new int[verticesCount];
            int[] key = new int[verticesCount];
            bool[] mstSet = new bool[verticesCount];

            for (int i = 0; i < verticesCount; i++)
            {
                key[i] = int.MaxValue;
                mstSet[i] = false;
            }

            key[0] = 0;
            parent[0] = -1;

            for (int count = 0; count < verticesCount - 1; count++)
            {
                int u = MinKey(key, mstSet, verticesCount);
                mstSet[u] = true;

                foreach (var edge in adjacencyList[u])
                {
                    int v = edge.Vertex2;
                    if (!mstSet[v] && edge.Cost < key[v])
                    {
                        parent[v] = u;
                        key[v] = edge.Cost;
                    }
                }
            }

            PrintMST(parent, key, verticesCount);
        }

        private int MinKey(int[] key, bool[] mstSet, int verticesCount)
        {
            int min = int.MaxValue, minIndex = -1;

            for (int v = 0; v < verticesCount; v++)
            {
                if (!mstSet[v] && key[v] < min)
                {
                    min = key[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }

        private void PrintMST(int[] parent, int[] key, int verticesCount)
        {
            for (int i = 1; i < verticesCount; i++)
            {
                OutputTextBlock.Text += $"{parent[i]} - {i}\t{key[i]}\n";
            }
        }

        private void Button_Dijkstra_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Alghorithms.PrimsAlghorithm.Edge> edgesDijkstra = [];

                foreach (Edge edge in edges)
                {
                    edgesDijkstra.Add(new Alghorithms.PrimsAlghorithm.Edge(edge.Vertex1, edge.Vertex2, edge.Cost));
                }

                Dijkstra dijkstra = new Dijkstra(edgesDijkstra);

                shortestPath = dijkstra.FindShortestPath(int.Parse(DijkstraMinimumPathInput.Text));
                DijkstraMinimumPathOutput.Text = string.Empty;
                foreach (KeyValuePair<int,int> keyValuePair in shortestPath)
                {
                    DijkstraMinimumPathOutput.Text += $"{DijkstraMinimumPathInput.Text} -> {keyValuePair.Key.ToString()} = {keyValuePair.Value.ToString()}\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public class Edge
    {
        public int Vertex1 { get; set; }
        public int Vertex2 { get; set; }
        public int Cost { get; set; }
    }

}
