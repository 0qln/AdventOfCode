
using System.Diagnostics;
using System.Text.RegularExpressions;

var data = File.ReadAllText(@"../../../input.txt").Split("\r\n\r\n");
var sw = Stopwatch.StartNew();
Console.WriteLine(AllInOneBcChallangeGoofy(data[0], data[1].Split("\n")));
sw.Stop();
Console.WriteLine($"Completed in {sw.Elapsed}");

ulong AllInOneBcChallangeGoofy(string instructions, string[] lines)
{
    // Build Tree
    Dictionary<string, (string left, string right)> nodes = new();
    foreach (var line in lines)
        nodes.Add(NodeName(line), (LeftNode(line), RightNode(line)));

    // select all nodes that end with an 'A'
    NodeContent[] fastNodes = new NodeContent[nodes.Count];
    var keys = nodes.Keys.ToArray();
    var values = nodes.Values.ToArray();
    Dictionary<string, int> indeces = new Dictionary<string, int>();
    for (int i = 0; i < fastNodes.Length; i++)
        indeces.Add(keys[i], i);

    for (int i = 0; i < fastNodes.Length; i++)
        fastNodes[i] = new NodeContent(indeces[values[i].left], indeces[values[i].right]);

    Console.WriteLine("Converted");

    int[] startNodes = nodes.Where(kvp => kvp.Key[^1] == 'A').Select(x => indeces[x.Key]).ToArray();
    int[] currentNodes = startNodes;
    HashSet<int> endNodes = new(nodes.Where(kvp => kvp.Key[^1] == 'Z').Select(x => indeces[x.Key]));
    ulong[] steps = new ulong[startNodes.Length];

    Console.WriteLine("Initiated");

    Parallel.For(0, startNodes.Length, node =>
    {
        int currentInstruction = 0;

        while (!endNodes.Contains(currentNodes[node]))
        {
            var left = instructions[currentInstruction] == 'L';

            // Use Interlocked to update currentNodes[node] in a thread-safe manner
            int updatedNode = left ? fastNodes[currentNodes[node]].Left : fastNodes[currentNodes[node]].Right;
            Interlocked.Exchange(ref currentNodes[node], updatedNode);

            Interlocked.Increment(ref steps[node]);

            currentInstruction++;
            currentInstruction %= instructions.Length;
        }
    });

    Console.WriteLine("Computed steps");

    return Lcm(steps);
}

ulong Lcm(ulong[] numbers) => numbers.Aggregate((x, y) => x * y / Gcd(x, y));
static ulong Gcd(ulong a, ulong b) => b == 0 ? a : Gcd(b, a % b);

const string VALID_NODE = @"[A-Z0-9]";
string NodeName(string node) => Regex.Match(node, @"^"+ VALID_NODE + @"+(?= )").Value;
string LeftNode(string node) => Regex.Match(node, @"(?<=\()"+ VALID_NODE + @"+(?=,)").Value;
string RightNode(string node) => Regex.Match(node, @"(?<=, )"+ VALID_NODE + @"+(?=\))").Value;


record struct NodeContent(int Left, int Right);