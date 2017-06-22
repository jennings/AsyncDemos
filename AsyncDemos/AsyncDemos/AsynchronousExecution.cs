using System.Threading.Tasks;
using static AsyncDemos.Helper;

namespace AsyncDemos
{
    class AsynchronousExecution
    {
        public async Task<string> GetGreetingThreeTimesAsync()
        {
            try
            {
                Output($">> {GetType().Name}.GetGreetingThreeTimesAsync()");

                Task<string> task1 = GetGreetingAsync(1);
                Task<string> task2 = GetGreetingAsync(2);
                Task<string> task3 = GetGreetingAsync(3);

                // Does not block; we return a Task immediately and continue when Task.WhenAll() completes
                await Task.WhenAll(task1, task2, task3);

                // Since the tasks are already complete, these awaits don't pause/resume the method
                return await task1 + await task2 + await task3;
            }
            finally
            {
                Output($"<< {GetType().Name}.GetGreetingThreeTimesAsync()");
            }
        }

        async Task<string> GetGreetingAsync(int id)
        {
            Output($"---- (ID = {id}) GetUserFromDatabase({id})");
            var user = await GetUserFromDatabaseAsync(id);

            Output($"---- (ID = {id}) Fibonacci({user.Age})");
            var magicNumber = Fibonacci(user.Age);

            Output($"---- (ID = {id}) Finished");
            return $"Your magic number is {magicNumber}!";
        }
    }
}
