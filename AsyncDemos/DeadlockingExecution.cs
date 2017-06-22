using System.Threading;
using System.Threading.Tasks;
using static AsyncDemos.Helper;

namespace AsyncDemos
{
    class DeadlockingExecution
    {
        public string GetGreetingThreeTimesDEADLOCK()
        {
            // This method is doing the gruntwork of creating a SynchronizationContext that simulates the
            // WindowsFormsSynchronizationContext (which gets you back onto the UI thread).

            // By setting this SynchronizationContext, "await task" will complete on the same thread that it started on.

            var context = new SimulationSynchronizationContext();
            try
            {
                SynchronizationContext.SetSynchronizationContext(context);
                var tcs = new TaskCompletionSource<string>();
                context.Post(_ => tcs.SetResult(GetGreetingThreeTimes()), null);
                return tcs.Task.Result; // This is bad too, but we never get here anyway.
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(null);
                context.Dispose();
            }
        }

        string GetGreetingThreeTimes()
        {
            // This is a synchronous method that's trying to call asynchronous methods.
            // Something tells me this is a bad idea...

            try
            {
                Output($">> {GetType().Name}.GetGreetingThreeTimes()");

                Task<string> task1 = GetGreetingAsync(1);
                Task<string> task2 = GetGreetingAsync(2);
                Task<string> task3 = GetGreetingAsync(3);

                Output($"-- Task.WaitAll(task1, task2, task3)");

                // HERE IS THE DEADLOCK.
                // We're synchronously waiting for three tasks to complete, but all three of them
                // have a SynchronizationContext that requires getting back onto this blocked thread.
                Task.WaitAll(task1, task2, task3);

                return task1.Result + task2.Result + task3.Result;
            }
            finally
            {
                Output($"<< {GetType().Name}.GetGreetingThreeTimes()");
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
