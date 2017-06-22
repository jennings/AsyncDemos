using System.Threading.Tasks;
using static AsyncDemos.Helper;

namespace AsyncDemos
{
    class MultiThreadingExecution
    {
        public string GetGreetingThreeTimes()
        {
            try
            {
                Output($">> {GetType().Name}.GetGreetingThreeTimes()");

                Task<string> task1 = Task.Run(() => GetGreeting(1));
                Task<string> task2 = Task.Run(() => GetGreeting(2));
                Task<string> task3 = Task.Run(() => GetGreeting(3));

                // BLOCKS
                Task.WaitAll(task1, task2, task3);

                return task1.Result + task2.Result + task3.Result;
            }
            finally
            {
                Output($"<< {GetType().Name}.GetGreetingThreeTimes()");
            }
        }

        public async Task<string> GetGreetingThreeTimesAsync()
        {
            try
            {
                Output($">> {GetType().Name}.GetGreetingThreeTimesAsync()");

                Task<string> task1 = Task.Run(() => GetGreeting(1));
                Task<string> task2 = Task.Run(() => GetGreeting(2));
                Task<string> task3 = Task.Run(() => GetGreeting(3));

                // BLOCKS
                await Task.WhenAll(task1, task2, task3);

                return task1.Result + task2.Result + task3.Result;
            }
            finally
            {
                Output($"<< {GetType().Name}.GetGreetingThreeTimesAsync()");
            }
        }

        string GetGreeting(int id)
        {
            Output($"---- (ID = {id}) GetUserFromDatabase({id})");
            var user = GetUserFromDatabase(id);

            Output($"---- (ID = {id}) Fibonacci({user.Age})");
            var magicNumber = Fibonacci(user.Age);

            Output($"---- (ID = {id}) Finished");
            return $"Your magic number is {magicNumber}!";
        }
    }
}
