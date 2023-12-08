
using System.Diagnostics;
using System.Text.RegularExpressions;


var data = File.ReadAllText(@"../../../input.txt").Split("\r\n\r\n");
for (int i = 0; i < 20; i++)
{
    var sw = Stopwatch.StartNew();
    var result = StepsToGoal(data[0], data[1].Split("\n"));
    Console.WriteLine($"Calculated result of {result} in {sw.Elapsed}");
    Debug.Assert(result == 23977527174353);
}


ulong StepsToGoal(string instructionsStr, string[] lines)
{
    // Build Tree
    var names = lines.Select(NodeName);
    (string Left, string Right)[] children = lines.Select(LeftNode).Zip(lines.Select(RightNode)).ToArray();
    var indeces = names.Select((x, i) => new KeyValuePair<string, int>(x, i)).ToDictionary();
    var nodes = children.Select((x, i) => new NodeContent(indeces[children[i].Left], indeces[children[i].Right])).ToArray();

    // Traverse tree
    var currentNodes = names.Where(x => x[^1] == 'A').Select(x => indeces[x]).ToArray();
    var endNodes = names.Where(x => x[^1] == 'Z').Select(x => indeces[x]).ToHashSet();
    var steps = new ulong[currentNodes.Length];
    var instructions = instructionsStr.Select(c => c == 'L').ToArray(); 

    Parallel.For(0, currentNodes.Length, node =>
    {
        int currInstr = -1;

        while (!endNodes.Contains(currentNodes[node]))
        {
            Interlocked.Exchange(ref currentNodes[node], 
                instructions[currInstr = (currInstr + 1) % instructions.Length]
                ? nodes[currentNodes[node]].Left
                : nodes[currentNodes[node]].Right);

            Interlocked.Increment(ref steps[node]);
        }
    });

    // all steps have to end on one round 
    // => take Least Common Multiple of steps
    return Lcm(steps);
}


ulong Lcm(ulong[] numbers) => numbers.Aggregate((x, y) => x * y / Gcd(x, y));
static ulong Gcd(ulong a, ulong b) => b == 0 ? a : Gcd(b, a % b);


const string VALID_NODE = @"[A-Z0-9]";
string NodeName(string node) => Regex.Match(node, $@"^{VALID_NODE}+(?= )").Value;
string LeftNode(string node) => Regex.Match(node, $@"(?<=\(){VALID_NODE}+(?=,)").Value;
string RightNode(string node) => Regex.Match(node, $@"(?<=, ){VALID_NODE}+(?=\))").Value;


record struct NodeContent(int Left, int Right);