using Alghorithms.SimplexMethod;

namespace TestAlghorithms
{
    public class UnitTestSimlexMethod
    {
        [Fact]
        public void Test_Maximization_Simple_Problem()
        {
            // Ïðèìåð çàäà÷è:
            // Ìàêñèìèçàöèÿ: Z = 3x1 + 2x2
            // Îãðàíè÷åíèÿ:
            // x1 + x2 <= 4
            // x1 <= 3
            // x2 <= 2

            double[] c = { 3, 2 };  // Êîýôôèöèåíòû öåëåâîé ôóíêöèè
            double[,] A = {
            { 1, 1 },  // Îãðàíè÷åíèå: x1 + x2 <= 4
            { 1, 0 },  // Îãðàíè÷åíèå: x1 <= 3
            { 0, 1 }   // Îãðàíè÷åíèå: x2 <= 2
            };
            double[] b = { 4, 3, 2 };  // Ïðàâàÿ ÷àñòü îãðàíè÷åíèé

            var constraintsTypes = new List<string> { "<=", "<=", "<=" };

            SimplexSolver solver = new SimplexSolver(c, A, b, constraintsTypes, "max");
            solver.Solve();

            Assert.Equal(3d, solver.GetSolution("x1")); // x1 = 3
            Assert.Equal(1d, solver.GetSolution("x2")); // x2 = 1
            Assert.Equal(11d, solver.OptimalValue); // Îïòèìàëüíîå çíà÷åíèå
        }

        [Fact]
        public void Test_Minimization_Simple_Problem()
        {
            // Ïðèìåð çàäà÷è:
            // Ìèíèìèçàöèÿ: Z = 3x1 + 2x2
            // Îãðàíè÷åíèÿ:
            // x1 + x2 <= 4
            // x1 <= 3
            // x2 <= 2

            double[] c = { 3, 2 };  // Êîýôôèöèåíòû öåëåâîé ôóíêöèè
            double[,] A = {
            { 1, 1 },  // Îãðàíè÷åíèå: x1 + x2 <= 4
            { 1, 0 },  // Îãðàíè÷åíèå: x1 <= 3
            { 0, 1 }   // Îãðàíè÷åíèå: x2 <= 2
            };
            double[] b = { 4, 3, 2 };  // Ïðàâàÿ ÷àñòü îãðàíè÷åíèé

            var constraintsTypes = new List<string> { "<=", "<=", "<=" };

            SimplexSolver solver = new SimplexSolver(c, A, b, constraintsTypes, "min");
            solver.Solve();

            Assert.Equal(0d, solver.GetSolution("x1")); // x1 = 0
            Assert.Equal(0d, solver.GetSolution("x2")); // x2 = 0
            Assert.Equal(0d, solver.OptimalValue); // Îïòèìàëüíîå çíà÷åíèå
        }

        [Fact]
        public void Test_Simplex_With_Equality_Constraint()
        {
            // Ïðèìåð çàäà÷è:
            // Ìàêñèìèçàöèÿ: Z = 3x1 + 2x2
            // Îãðàíè÷åíèÿ: x + 2y = 4, 3x + y = 5

            double[] c = { 3, 2 }; // Êîýôôèöèåíòû öåëåâîé ôóíêöèè
            double[,] A = { { 1, 2 }, { 3, 1 } }; //x1 + 2x2 = 4 , 3x1 + x2 = 5
            double[] b = { 4, 5 };
            var constraintsTypes = new List<string> { "=", "=" };

            // Ñîçäàåì ðåøàòåëü äëÿ ìàêñèìèçàöèè
            var solver = new SimplexSolver(c, A, b, constraintsTypes, "max");

            // Ðåøàåì çàäà÷ó
            solver.Solve();

            // Ïðîâåðÿåì ðåçóëüòàòû

            Assert.Equal(1.2d, solver.GetSolution("x1")); // x1 = 1.2
            Assert.Equal(1.4d, solver.GetSolution("x2")); // x2 = 1.4
            Assert.Equal(6.4d, solver.OptimalValue); // Öåëåâàÿ ôóíêöèÿ = 3*1 + 2*1.5 = 6.4
        }

        [Fact]
        public void Test_Simplex_With_Artificial_Variable()
        {
            // Öåëåâàÿ ôóíêöèÿ: Max x + y
            double[] c = { 1, 1 };
            double[,] A = { { 1, 1 } }; // Îãðàíè÷åíèå: x + y <= 5
            double[] b = { 5 };
            var constraintsTypes = new List<string> { "<=" };

            // Ñîçäàåì ðåøàòåëü äëÿ ìàêñèìèçàöèè
            var solver = new SimplexSolver(c, A, b, constraintsTypes, "max");

            // Ðåøàåì çàäà÷ó
            solver.Solve();

            // Ïðîâåðÿåì ðåçóëüòàòû

            Assert.Equal(5, solver.GetSolution("x1")); // x1 = 5
            Assert.Equal(0, solver.GetSolution("x2")); // x2 = 0
            Assert.Equal(5, solver.OptimalValue); // Öåëåâàÿ ôóíêöèÿ = 5 + 0 = 5
        }

        [Fact]
        public void Test_Simplex_With_Wrong_Constrain()
        {
            // Ïðèìåð çàäà÷è:
            // Ìàêñèìèçàöèÿ: Z = 3x1 + 2x2
            // Îãðàíè÷åíèÿ: x + 2y = 4, 3x + y = 5

            double[] c = { 3, 2 }; // Êîýôôèöèåíòû öåëåâîé ôóíêöèè
            double[,] A = { { 1, 2 }, { 3, 1 } }; //x1 + 2x2 = 4 , 3x1 + x2 = 5
            double[] b = { 4, 5 };
            var constraintsTypes = new List<string> { "=", "-" }; //wrong constrain

            // Ïðîâåðÿåì ðåçóëüòàòû
            Assert.Throws<ArgumentException>(() => new SimplexSolver(c, A, b, constraintsTypes, "max"));

        }
    }
}