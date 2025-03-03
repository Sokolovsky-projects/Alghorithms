using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alghorithms.TransportTask;

namespace TestAlghorithms
{
    public class UnitTestTransportTask
    {
        [Fact]
        public void Transport_Task_Find_Solution_Problem()
        {
            double[,] costMatrix = {
            { 4, 6, 8 },
            { 2, 5, 7 },
            { 3, 4, 9 }
            };

            double[] supply = { 50, 60, 70 };
            double[] demand = { 30, 80, 70 };

            var transportSolver = new TransportTaskSolver(costMatrix, supply, demand);

            //double[,] solutionByNorthWestCornerMethod = transportSolver.NorthWestCornerMethod(); // пока не работает

            double[,] solution = transportSolver.MinimumCostMethod();

            Assert.Equal(930, transportSolver.CalculateTotalCost(solution));
        }
    }
}
