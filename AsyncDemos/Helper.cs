using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemos
{
    static class Helper
    {
        public static int Fibonacci(int n)
        {
            // Simulate a long CPU operation by sleeping
            Thread.Sleep(100);

            if (n == 1 || n == 2) return 1;

            int a = 1;
            int b = 1;
            while (true)
            {
                var temp = a;
                a = b;
                b = a + temp;
                if (n-- == 2) return b;
            }
        }

        public static User GetUserFromDatabase(int id)
        {
            // Simulate a long CPU operation by sleeping
            Thread.Sleep(1000);

            return new User
            {
                Name = $"NAME_{(id * 10)}_NAME",
                Age = id * 10,
            };
        }

        public static async Task<User> GetUserFromDatabaseAsync(int id)
        {
            // DON'T ACTUALLY DO THIS, THIS IS JUST FOR THE DEMO
            await Task.Yield();
            return GetUserFromDatabase(id);
        }

        static Stopwatch _sw;

        public static void OutputHeader()
        {
            Console.WriteLine("  TIME   THREAD_ID  OUTPUT");
            Console.WriteLine("---------------------------------------------------");
            _sw = Stopwatch.StartNew();
        }

        public static void Output(string str)
        {
            Console.WriteLine($"{_sw.ElapsedMilliseconds,6:D}   {Thread.CurrentThread.ManagedThreadId,8:D}   {str}");
        }
    }

    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
