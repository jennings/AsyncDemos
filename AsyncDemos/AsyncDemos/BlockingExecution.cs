using static AsyncDemos.Helper;

namespace AsyncDemos
{
    class BlockingExecution
    {
        public string GetGreetingThreeTimes()
        {
            try
            {
                Output($">> {GetType().Name}.GetGreetingThreeTimes()");
                return GetGreeting(1) + GetGreeting(2) + GetGreeting(3);
            }
            finally
            {
                Output($"<< {GetType().Name}.GetGreetingThreeTimes()");
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
