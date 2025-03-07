using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alghorithms.DynamicProgramming;

namespace TestAlghorithms
{
    public class UnitTestFibonacciFinder
    {
        [Fact]
        public void Fibonacci_Finder_Problem()
        {
            FibonacciNumbersFinder finder = new FibonacciNumbersFinder();
            
            Assert.Equal(1134903170, finder.GetFibonacci(45));
            Assert.Equal(21, finder.GetFibonacci(8));

            finder.ConsoleLogMemory();
        }
    }
}
