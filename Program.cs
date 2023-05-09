using System;
using Country_method;

public class MainClass
{
    public static void Main()
    {
        World world = new World(2, 0.00001, 5, 1, 600, 0.4, 0.2,
            8, 1, 1, 100,100, 8, 3, 4);
        
        Console.WriteLine(world.StartTheSimulation());
        
        Console.WriteLine(world.funcInvoked);
    }
}