namespace Viagogo;

public class Event
{
    public string Name { get; set; }
    public string City { get; set; }
}

public class Customer
{
    public string Name { get; set; }
    public string City { get; set; }
}

public class EventDistance
{
    public Event Event { get; set; }
    public int Distance { get; set; }
}

public class Solution
{
    private static readonly Dictionary<string, int> Dictionary = new Dictionary<string, int>();

    private static void Main(string[] args)
    {
        var customer = new Customer { Name = "John", City = "New York" };

        /*
         * We want you to send an email to this customer with all events in their city
         * Just call AddToEmail(customer, event) for each event you think they should get
         */

        //Lets get 5 'closest events' for our customer using GetDistance()

        //Get top 5 'best' events - best = EventPrice + (GetDistance * 0.5)

        //int case you needed to filter with other fields
        var closerEvents = GetEvents(x => x.City == customer.City);

        //to add only closer ones based on city match
        //closerEvents.ForEach( closeEvent => AddToEmail(customer, closeEvent));

        var distanceFromEvents = new List<EventDistance>();

        foreach (var e in closerEvents)
        {
            var distance = GetDistanceBetweenCities(customer.City, e.City);
            distanceFromEvents.Add(new EventDistance { Event = e, Distance = distance });
        }
        distanceFromEvents.Sort((x, y) => x.Distance.CompareTo(y.Distance));
        var limit = distanceFromEvents.Count < 5 ? distanceFromEvents.Count : 5;
        for (int i = 0; i < limit; i++)
        {
            var price = GetPrice(distanceFromEvents[i].Event);
            AddToEmail(customer, distanceFromEvents[i].Event, price);
        }
    }

    private static int GetDistanceBetweenCities(string from, string to)
    {
        if (from == to) return 0;

        //To ensure uniqueness
        var dictKey = $"{from}:{to}";

        if (Dictionary.ContainsKey(dictKey)) return Dictionary[dictKey];

        Dictionary.Add(dictKey, GetDistance(from, to));

        return Dictionary[dictKey];
    }

    // You do not need to know how these methods work

    private static void AddToEmail(Customer c, Event e, int? price = null)
    {
        var distance = GetDistance(c.City, e.City);
        Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
    }

    private static IEnumerable<Event> GetEvents(Func<Event, bool> predicate)
    {
        return GetAllEvents().Where(predicate);
    }

    private static IEnumerable<Event> GetAllEvents()
    {
        return new List<Event>{
            new Event{ Name = "Phantom of the Opera", City = "New York"},
            new Event{ Name = "Metallica", City = "Los Angeles"},
            new Event{ Name = "Metallica", City = "New York"},
            new Event{ Name = "Metallica", City = "Boston"},
            new Event{ Name = "LadyGaGa", City = "New York"},
            new Event{ Name = "LadyGaGa", City = "Boston"},
            new Event{ Name = "LadyGaGa", City = "Chicago"},
            new Event{ Name = "LadyGaGa", City = "San Francisco"},
            new Event{ Name = "LadyGaGa", City = "Washington"}
        };
    }

    private static int GetPrice(Event e)
    {
        return (AlphebiticalDistance(e.City, "") + AlphebiticalDistance(e.Name, "")) / 10;
    }

    private static int GetDistance(string fromCity, string toCity)
    {
        return AlphebiticalDistance(fromCity, toCity);
    }

    private static int AlphebiticalDistance(string s, string t)
    {
        var result = 0;
        var i = 0;

        for (i = 0; i < Math.Min(s.Length, t.Length); i++)
        {
            // Console.Out.WriteLine($"loop 1 i={i} {s.Length} {t.Length}");
            result += Math.Abs(s[i] - t[i]);
        }

        for (; i < Math.Max(s.Length, t.Length); i++)
        {
            // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
            result += s.Length > t.Length ? s[i] : t[i];
        }

        return result;
    }
}

/*
            var customers = new List<Customer>{
                new Customer{ Name = "Nathan", City = "New York"},
                new Customer{ Name = "Bob", City = "Boston"},
                new Customer{ Name = "Cindy", City = "Chicago"},
                new Customer{ Name = "Lisa", City = "Los Angeles"}
            };
*/