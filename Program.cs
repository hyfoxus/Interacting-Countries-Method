using System;
using Country_method;

public class MainClass
{
    public static void Main()
    {
        World world = new World(2, 0.00001, 5, 1, 300, 0.4, 0.2,
            8, 1, 5, 15,100, 8, 3);
        
        Console.WriteLine(world.StartTheSimulation());
    }
}