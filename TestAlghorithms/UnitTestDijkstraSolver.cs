using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alghorithms.MinimumPath;
using Alghorithms.PrimsAlghorithm;

namespace TestAlghorithms
{
    public class UnitTestDijkstraSolver
    {
        [Fact]
        public void Minimum_Path_Dijkstra_Problem()
        {
            List<Edge> edges = new List<Edge>
            {
                new Edge(0,1,6),
                new Edge(0,2,1),
                new Edge(0,3,5),
                new Edge(1,2,5),
                new Edge(1,4,3),
                new Edge(2,4,6),
                new Edge(2,5,4)
            };

            Dijkstra dijkstra = new Dijkstra(edges);

            var result = dijkstra.FindShortestPath(0);

            Assert.Equal(6, result[1]);
            Assert.Equal(1, result[2]);
            Assert.Equal(5, result[3]);
            Assert.Equal(7, result[4]);
            Assert.Equal(5, result[5]);
        }
    }
}
