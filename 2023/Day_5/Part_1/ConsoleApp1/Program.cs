using System.Diagnostics;
using System.Text.RegularExpressions;
using Map = (string Name, System.Collections.Generic.Dictionary<int, int>[] ValueMappings);


string data = File.ReadAllText("../../../input.txt");

List<Map> maps = GetMaps(data).ToList();
List<int> seeds = GetSeeds(data).ToList();

foreach (var seed in seeds)
{
    RunThroughMaps(seed, maps);
}


int RunThroughMaps(int seed, List<Map> maps)
{
    Console.Write(seed);
    foreach (var map in maps)
    {
        foreach (var mapping in map.ValueMappings)
        {
            seed = mapping.ContainsKey(seed) ? mapping[seed] : seed;
        }
        Console.Write($" -[{map.Name}]-> {seed}");
    }
    Console.WriteLine();
    return seed;
}

IEnumerable<Map> GetMaps(string data)
{
    var maps = Regex.Matches(data, "(?<=\\r\\n)[0-9 \\r\\n]+(?=\\r\\n\\r\\n|$)");
    var names = Regex.Matches(data, "(?<=\\r\\n\\r\\n)[\\S]+");
    Debug.Assert(maps.Count == names.Count);

    for (int i = 0; i < maps.Count; i++)
    {
        var mappings = new List<Dictionary<int, int>>();
        foreach (var line in maps[i].Value.Split('\n'))
        {
            var vals = Regex.Matches(line, @"\d+").Select(x => int.Parse(x.Value)).ToArray();
            var newMap = new Dictionary<int, int>();
            for (int r = 0; r < vals[2]; r++)
                newMap.Add(r + vals[1], r + vals[0]);
            
            mappings.Add(newMap);
        }

        yield return (names[i].Value, mappings.ToArray());
    }
}

IEnumerable<int> GetSeeds(string data)
{
    return new Regex("(?<=seeds: ).+(?=\\n)").Match(data).Value.Split(' ').Select(x => int.Parse(x));
}