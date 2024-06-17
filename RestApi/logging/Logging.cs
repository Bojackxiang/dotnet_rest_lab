namespace RestApi.logging;

public class Logging : ILogging
{
    public void Log(string message, string type)
    {
        if (type == "Error")
        {
            Console.WriteLine("Error - {0}", message);
        }
        else
        {
            Console.WriteLine("Other - {0}", message);
        }
    }
}