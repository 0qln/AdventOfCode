using Map = int[][];


Map map = File.ReadAllLines(@"../../../input.txt").Select(x => x.Select(x => x - '0').ToArray()).ToArray();
Map dykstra = new int[map.Length][];

List<List<Neighbor>> nodeConnections = new();
// build node connections
for (int i = 0; i < map.Length; i++)
{
    for (int j = 0; j < map[0].Length; j++)
    {
        nodeConnections.Add(new List<Neighbor?>
        {
            j > 0 ? new Neighbor(i * map.Length + j - 1, map[i][j - 1], Direction.Left) : null,
            j < map[0].Length - 1 ? new Neighbor(i * map.Length + j + 1, map[i][j + 1], Direction.Right) : null,
            i > 0 ? new Neighbor(i * map.Length + j - map.Length, map[i - 1][j], Direction.Up) : null,
            i < map.Length - 1 ? new Neighbor(i * map.Length + j + map.Length, map[i + 1][j], Direction.Down) : null,
        }.Where(x => x is not null).Cast<Neighbor>().ToList());
    }
}

var minDist = ComputeShortestPathsByDijkstra(0, nodeConnections);
Console.WriteLine();



(int[] MinimumDistances, int[] PreviousVertices) ComputeShortestPathsByDijkstra(int startIndex, List<List<Neighbor>> adjacencyList)
{
    const int MAX_WEIGHT = 999999;
    var numVertices = adjacencyList.Count;
    var minimumDistances = new int[numVertices].Select(x => MAX_WEIGHT).ToArray();
    var previousVertices = new int[numVertices].Select(x => -1).ToArray();
    var vertexQueue = new SortedSet<Tuple<int, int>>(Comparer<Tuple<int, int>>.Create((x, y) => x.Item1.CompareTo(y.Item1))) { Tuple.Create(0, startIndex) };
    minimumDistances[startIndex] = 0;
    
    while (vertexQueue.Any())
    {
        var distance = vertexQueue.First().Item1;
        var index = vertexQueue.First().Item2;
        vertexQueue.Remove(vertexQueue.First());

        foreach (var neighborIterator in adjacencyList[index])
        {
            var targetIndex = neighborIterator.targetIndex;
            var weight = neighborIterator.weight;
            var currentDistance = distance + weight;

            if (currentDistance < minimumDistances[targetIndex])
            {
                vertexQueue.Remove(Tuple.Create(minimumDistances[targetIndex], targetIndex));
                minimumDistances[targetIndex] = currentDistance;
                previousVertices[targetIndex] = index;
                vertexQueue.Add(Tuple.Create(minimumDistances[targetIndex], targetIndex));
            }
        }
    }

    return (minimumDistances, previousVertices);
}



record struct Neighbor(int targetIndex, int weight, Direction direction);

enum Direction { Left, Up, Right, Down }