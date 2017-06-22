using System;
using System.Threading;
using System.Threading.Tasks;
using static AsyncDemos.Helper;

namespace AsyncDemos
{
    class Program
    {
        static readonly ManualResetEventSlim _quit = new ManualResetEventSlim(false);

        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                do
                {
                    await ExecuteAsync();
                    Console.WriteLine();
                    Console.WriteLine("PRESS ESC TO EXIT");
                    Console.WriteLine("PRESS ANY OTHER KEY TO REPEAT");
                }
                while (Console.ReadKey().Key != ConsoleKey.Escape);
                _quit.Set();
                return 1; // Ensure not async-void
            });
            _quit.Wait();
        }

        static async Task ExecuteAsync()
        {
                await Task.Yield();

                OutputHeader();
                Output("START");


                // BLOCKING EXECUTION
                var execution = new BlockingExecution();
                var result = execution.GetGreetingThreeTimes();

                // MULTI-THREADED EXECUTION
                //var execution = new MultiThreadingExecution();
                //var result = execution.GetGreetingThreeTimes();

                // MULTI-THREADED EXECUTION (WITH ASYNC/AWAIT)
                //var execution = new MultiThreadingExecution();
                //var result = await execution.GetGreetingThreeTimesAsync();

                // ASYNCHRONOUS EXECUTION
                //var execution = new AsynchronousExecution();
                //var result = await execution.GetGreetingThreeTimesAsync();

                // DEADLOCKING BY BLOCKING ON ASYNC EXECUTION
                //var execution = new DeadlockingExecution();
                //var result = execution.GetGreetingThreeTimesDEADLOCK();


                // var result = await execution.GetGreetingThreeTimesAsync();

                Output($"Final result: {result}");

                Output("DONE");
            }
    }
}
