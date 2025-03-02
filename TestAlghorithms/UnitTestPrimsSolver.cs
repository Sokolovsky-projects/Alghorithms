using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alghorithms.PrimsAlghorithm;

namespace TestAlghorithms
{
    public class UnitTestPrimsSolver
    {
        [Fact]
        public void Prims_Solver_MinimumPath_Problem()
        {
            PrimsSolver solver = new PrimsSolver();

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

            int solution = solver.FindMinimumSpanningTree(edges, 6);

            Assert.Equal(18, solution);
        }
    }
}
