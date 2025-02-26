using Alghorithms.SimplexMethod;

namespace TestAlghorithms
{
    public class SimplexMethodTest
    {
        [Fact]
        public void Test_Maximization_Simple_Problem()
        {
            // Пример задачи:
            // Максимизация: Z = 3x1 + 2x2
            // Ограничения:
            // x1 + x2 <= 4
            // x1 <= 3
            // x2 <= 2

            double[] c = { 3, 2 };  // Коэффициенты целевой функции
            double[,] A = {
            { 1, 1 },  // Ограничение: x1 + x2 <= 4
            { 1, 0 },  // Ограничение: x1 <= 3
            { 0, 1 }   // Ограничение: x2 <= 2
            };
            double[] b = { 4, 3, 2 };  // Правая часть ограничений

            var constraintsTypes = new List<string> { "<=", "<=", "<=" };

            SimplexSolver solver = new SimplexSolver(c, A, b, constraintsTypes, "max");
            solver.Solve();

            Assert.Equal(3d, solver.GetSolution("x1")); // x1 = 3
            Assert.Equal(1d, solver.GetSolution("x2")); // x2 = 1
            Assert.Equal(11d, solver.OptimalValue); // Оптимальное значение
        }

        [Fact]
        public void Test_Minimization_Simple_Problem()
        {
            // Пример задачи:
            // Минимизация: Z = 3x1 + 2x2
            // Ограничения:
            // x1 + x2 <= 4
            // x1 <= 3
            // x2 <= 2

            double[] c = { 3, 2 };  // Коэффициенты целевой функции
            double[,] A = {
            { 1, 1 },  // Ограничение: x1 + x2 <= 4
            { 1, 0 },  // Ограничение: x1 <= 3
            { 0, 1 }   // Ограничение: x2 <= 2
            };
            double[] b = { 4, 3, 2 };  // Правая часть ограничений

            var constraintsTypes = new List<string> { "<=", "<=", "<=" };

            SimplexSolver solver = new SimplexSolver(c, A, b,constraintsTypes, "min");
            solver.Solve();

            Assert.Equal(0d, solver.GetSolution("x1")); // x1 = 0
            Assert.Equal(0d, solver.GetSolution("x2")); // x2 = 0
            Assert.Equal(0d, solver.OptimalValue); // Оптимальное значение
        }
        
        [Fact]
        public void Test_Simplex_With_Equality_Constraint()
        {
            // Пример задачи:
            // Максимизация: Z = 3x1 + 2x2
            // Ограничения: x + 2y = 4, 3x + y = 5

            double[] c = { 3, 2 }; // Коэффициенты целевой функции
            double[,] A = { { 1, 2 }, { 3, 1 } }; //x1 + 2x2 = 4 , 3x1 + x2 = 5
            double[] b = { 4, 5 };
            var constraintsTypes = new List<string> { "=", "=" };

            // Создаем решатель для максимизации
            var solver = new SimplexSolver(c,A, b, constraintsTypes, "max");

            // Решаем задачу
            solver.Solve();

            // Проверяем результаты
            
            Assert.Equal(1.2d, solver.GetSolution("x1")); // x1 = 1.2
            Assert.Equal(1.4d, solver.GetSolution("x2")); // x2 = 1.4
            Assert.Equal(6.4d, solver.OptimalValue); // Целевая функция = 3*1 + 2*1.5 = 6.4
        }
        
        [Fact]
        public void Test_Simplex_With_Artificial_Variable()
        {
            // Целевая функция: Max x + y
            double[] c = { 1, 1 };
            double[,] A = { { 1, 1 } }; // Ограничение: x + y <= 5
            double[] b = { 5 };
            var constraintsTypes = new List<string> { "<=" };

            // Создаем решатель для максимизации
            var solver = new SimplexSolver(c, A, b, constraintsTypes, "max");

            // Решаем задачу
            solver.Solve();

            // Проверяем результаты
            
            Assert.Equal(5, solver.GetSolution("x1")); // x1 = 5
            Assert.Equal(0, solver.GetSolution("x2")); // x2 = 0
            Assert.Equal(5, solver.OptimalValue); // Целевая функция = 5 + 0 = 5
        }

        [Fact]
        public void Test_Simplex_With_Wrong_Constrain()
        {
            // Пример задачи:
            // Максимизация: Z = 3x1 + 2x2
            // Ограничения: x + 2y = 4, 3x + y = 5

            double[] c = { 3, 2 }; // Коэффициенты целевой функции
            double[,] A = { { 1, 2 }, { 3, 1 } }; //x1 + 2x2 = 4 , 3x1 + x2 = 5
            double[] b = { 4, 5 };
            var constraintsTypes = new List<string> { "=", "-" }; //wrong constrain

            // Проверяем результаты
            Assert.Throws<ArgumentException>(() => new SimplexSolver(c, A, b, constraintsTypes, "max"));
            
        }
    }
}