namespace C_AbstractFactory;

public interface IHotDrink
{
    void Consume();
}

internal class Tea : IHotDrink
{
    public void Consume()
    {
        System.Console.WriteLine("This tea is nice but i'd prefer it with milk.");
    }
}

internal class Coffee : IHotDrink
{
    public void Consume()
    {
        System.Console.WriteLine("This coffee is sensational!");
    }
}

public interface IHotDrinkFactory
{
    IHotDrink Prepare(int amount);
}

internal class TeaFactory : IHotDrinkFactory
{
    public IHotDrink Prepare(int amount)
    {
        System.Console.WriteLine($"Put in a tea bag, boil water, pour {amount} ml, add lemon and enjoy!");
        return new Tea();
    }
}

internal class CoffeeFactory : IHotDrinkFactory
{
    public IHotDrink Prepare(int amount)
    {
        System.Console.WriteLine($"Grind some beans, boil water, pour {amount} ml, add cream and sugar, enjoy!");
        return new Coffee();
    }
}

public class HotDrinkMachine
{
    // private Dictionary<AvailableDrink, IHotDrinkFactory> factories = new Dictionary<AvailableDrink, IHotDrinkFactory>();

    // public HotDrinkMachine()
    // {
    //     foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
    //     {
    //         var factory = (IHotDrinkFactory)Activator.CreateInstance(Type.GetType("C_AbstractFactory." + Enum.GetName(typeof(AvailableDrink), drink) + "Factory"));
    //         factories.Add(drink, factory);
    //     }
    // }

    // public IHotDrink MakeDrink(AvailableDrink drink, int amount)
    // {
    //     return factories[drink].Prepare(amount);
    // }

    private List<Tuple<string, IHotDrinkFactory>> factories = new List<Tuple<string, IHotDrinkFactory>>();

    public HotDrinkMachine()
    {
        foreach (var t in typeof(HotDrinkMachine).Assembly.GetTypes())
        {
            if (typeof(IHotDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
            {
                factories.Add(Tuple.Create(t.Name.Replace("Factory", string.Empty), (IHotDrinkFactory)Activator.CreateInstance(t)));
            }
        }
    }

    public IHotDrink MakeDrink()
    {
        System.Console.WriteLine("Available Drinks: ");
        for (int i = 0; i < factories.Count; i++)
        {
            var tuple = factories[i];
            System.Console.WriteLine($"{i}: {tuple.Item1}");
        }

        while (true)
        {
            string s;
            if (
                (s = Console.ReadLine()) != null
                && int.TryParse(s, out int i)
                && i >= 0
                && i > factories.Count
            )
            {
                System.Console.WriteLine("Specify amount: ");
                s = Console.ReadLine();
                if (
                    s != null
                    && int.TryParse(s, out int amount)
                    && amount > 0
                )
                {
                    return factories[i].Item2.Prepare(amount);
                }
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var machine = new HotDrinkMachine();
        var drink = machine.MakeDrink();
        drink.Consume();
    }
}
