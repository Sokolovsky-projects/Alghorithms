using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alghorithms.DynamicProgramming
{
    public class FibonacciNumbersFinder
    {
        // основная суть динамического программирования - сохранение результатов для оптимизации вычислений
        public readonly Dictionary<int, long> memory = new Dictionary<int, long>();

        public FibonacciNumbersFinder() 
        {
            // запись в память первых значений
            memory[0] = 0;
            memory[1] = 1;
        }

        public long GetFibonacci(int n)
        {
            // если в памяти уже есть решение для числа то возвращаем его
            if (memory.ContainsKey(n))
            {
                return memory[n];
            }

            // находим число ряда Фибоначчи, суммма двух предыдущих чисел
            memory[n] = GetFibonacci(n - 1) + GetFibonacci(n - 2);

            return memory[n];
        }

        public void ConsoleLogMemory()
        {
            foreach (var item in memory) 
            {
                Console.WriteLine($"{item.Key} -> {item.Value}");
            }
        }

        public Dictionary<int, long> GetMemory()
        {
            return memory;
        }
    }
}
