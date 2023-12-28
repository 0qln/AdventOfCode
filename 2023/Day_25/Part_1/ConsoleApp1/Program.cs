using System.Diagnostics;
using System.Text.RegularExpressions;
using Wire = (int, int);

// Extract nodes and their conenctions from the input data.
var input = File.ReadAllLines(@"../../../input.txt");

var leftSides = 
    input.Select(
        line => KeyValuePair.Create(
            Regex.Match(line, @"[a-z]{3}").Value, 
            Regex.Matches(line, @"(?<= )[a-z]{3}").Select(x => x.Value).ToHashSet()))
    .ToDictionary();

var rightSides =
    input.SelectMany(line => Regex.Matches(line, @"(?<= )[a-z]{3}")).Select(x => x.Value)
    .ToHashSet()
    .Where(node => !leftSides.ContainsKey(node))
    .Select(match => KeyValuePair.Create(match, new HashSet<string>()));

var nodes = leftSides.Concat(rightSides).ToDictionary();


// Map string names to indeces.
var keys = nodes.Keys.ToArray();
Func<string, int> ToIndex = name => Array.IndexOf(keys, name);
Func<int, string> ToName = name => keys[name];


// Create connection lookup table, (connections go both ways)
bool[][] connections = new bool[nodes.Count][];

string Neighbors(string node)
{
    string result = "";
    foreach (var neighbor in nodes[node])    
        result += neighbor + " ";    
    return result;
}
string WireToString(Wire wire)
{
    return $"{ToName(wire.Item1)}/{ToName(wire.Item2)}";
}
     
for (int node = 0; node < connections.Length; node++)
{
    connections[node] = new bool[nodes.Count];
}
for (int node = 0; node < connections.Length; node++)
{
    //Console.WriteLine($"{ToName(node)}: {node} - {Neighbors(ToName(node))}");
    foreach (var neighbor in nodes[ToName(node)])
    {
        // Connections go both ways
        connections[node][ToIndex(neighbor)] = true;
        connections[ToIndex(neighbor)][node] = true;
    }

}

int[] connectionCountValues = connections.Select(x => x.Sum(b => b ? 1 : 0)).ToArray();
int[] connectionCountIndeces = new int[nodes.Count].Select((x, i) => i).ToArray();
Array.Sort(connectionCountValues, connectionCountIndeces);


// First remove connections of nodes with few connections.
Wire wire1 = new();
while (TryCutNextEdge(ref wire1))
{
    Wire wire2 = new();
    while (TryCutNextEdge(ref wire2))
    {
        Wire wire3 = new();
        while (TryCutNextEdge(ref wire3))
        {
            var groupInfo = GroupInfo();

            if (groupInfo.Count >= 2)
            {
                foreach (var group in groupInfo)
                {
                    group.ForEach(x => Console.WriteLine(ToName(x)));
                    Console.WriteLine();
                }
            }

            UncutEdge(ref wire3);
        }
        UncutEdge(ref wire2);
    }
    UncutEdge(ref wire1);
}

bool TryCutNextEdge(ref Wire wire)
{
    IncrementWire(ref wire);

    // Increment
    while (wire.Item1 < nodes.Count
        && wire.Item2 < nodes.Count
        && connections[wire.Item1][wire.Item2] == false)
    {
        IncrementWire(ref wire);
    }

    // If iterator == end() return false
    if (wire.Item1 >= nodes.Count ||
        wire.Item2 >= nodes.Count)
    {
        return false;
    }

    // Else cut and return true
    connections[wire.Item1][wire.Item2] = false;
    connections[wire.Item2][wire.Item2] = false;

    return true;
}

void IncrementWire(ref Wire wire)
{
    wire.Item1++;

    if (wire.Item1 == nodes.Count)
    {
        wire.Item1 = 0;
        wire.Item2++;
    }
}

void UncutEdge(ref Wire wire)
{
    connections[wire.Item1][wire.Item2] = true;
    connections[wire.Item2][wire.Item1] = true;
}

List<List<int>> GroupInfo()
{
    var visited = new bool[connections.Length];
    var groups = new List<List<int>>();

    for (int i = 0; i < visited.Length; i++)
    {
        if (visited[i])
            continue;

        visited[i] = true;
        groups.Add(new List<int>());
        Traverse(i);
    }

    return groups;

    int Traverse(int node)
    {
        int result = 1;
        groups[^1].Add(node);
        visited[node] = true;
        for (int i = 0; i < connections.Length; ++i)
        {
            if (visited[i] || connections[node][i] == false)
                continue;

            result += Traverse(i);
        }
        return result;
    }
}





Console.WriteLine();