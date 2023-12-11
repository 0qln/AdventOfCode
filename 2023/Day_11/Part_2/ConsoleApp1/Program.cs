using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using Point = (int X, int Y);

var DATA = File.ReadAllLines(@"../../../input.txt");
var universe = GetUniverse(DATA);
var galaxies = GetGalaxies(universe);
Dictionary<int, bool> emptyCols = new();
Dictionary<int, bool> emptyRows = new();
decimal sum;
sum = GetPairs(galaxies).Sum(x => (decimal)Distance(x, universe, 1, emptyCols, emptyRows));
Console.WriteLine(sum); 
sum = GetPairs(galaxies).Sum(x => (decimal)Distance(x, universe, 10, emptyCols, emptyRows));
Console.WriteLine(sum);
sum = GetPairs(galaxies).Sum(x => (decimal)Distance(x, universe, 100, emptyCols, emptyRows));
Console.WriteLine(sum);
sum = GetPairs(galaxies).Sum(x => (decimal)Distance(x, universe, 1_000_000, emptyCols, emptyRows));
Console.WriteLine(sum);


dynamic Delta(dynamic x, dynamic y) => Math.Abs(x - y);


HashSet<(Point, Point)> GetPairs(List<Point> points)
{
    var ret = new HashSet<(Point, Point)>();

    for (int i = 0; i < points.Count; i++)
    {
        for (int j = 0; j < points.Count; j++)
        {
            if (i == j) continue;

            if (!ret.Contains((points[i], points[j])) &&
                !ret.Contains((points[j], points[i])))
            {
                ret.Add((points[i], points[j]));
            }
        }
    }

    return ret;
}

ulong Distance((Point, Point) points, bool[,] universe, ulong expansionRate, Dictionary<int, bool> emptyCols, Dictionary<int, bool> emptyRows)
{
    Debug.Assert(points.Item1 != points.Item2);

    Point lX = points.Item1.X > points.Item2.X ? points.Item2 : points.Item1;
    Point lY = points.Item1.Y > points.Item2.Y ? points.Item2 : points.Item1;
    Point hX = points.Item1.X < points.Item2.X ? points.Item2 : points.Item1;
    Point hY = points.Item1.Y < points.Item2.Y ? points.Item2 : points.Item1;

    ulong dX = (ulong)(hX.X - lX.X);
    ulong dY = (ulong)(hY.Y - lY.Y);

    for (int x = lX.X; x < hX.X; x++)
    {
        if (IsEmptyCol(universe, x, emptyCols))
        {
            dX += expansionRate - 1;
        }
    }
    for (int y = lY.Y; y < hY.Y; y++)
    {
        if (IsEmptyRow(universe, y, emptyRows))
        {
            dY += expansionRate - 1;
        }
    }

    return dX + dY;
}

List<Point> GetGalaxies(bool[,] universe)
{
    var result = new List<Point>();

    for (int x = 0; x < universe.GetLength(0); x++)
    {
        for (int y = 0; y < universe.GetLength(1); y++)
        {
            if (universe[x, y]) result.Add(new Point(x, y));
        }
    }

    return result;
}

bool[,] GetUniverse(string[] data)
{
    bool[,] ret = new bool[data[0].Length, data.Length];

    for (int i = 0; i < data[0].Length; i++)
    {
        for (int j = 0; j < data.Length; j++)
        {
            ret[i, j] = data[^(j + 1)][i] == '#';
        }
    }

    return ret;
}

bool IsEmptyRow(bool[,] data, int y, Dictionary<int, bool> emptyRows)
{
    if (emptyRows.ContainsKey(y))
    {
        return emptyRows[y];
    }

    for (int x = 0; x < data.GetLength(0); x++)
    {
        if (data[x, y]) return false;
    }

    return true;
}

bool IsEmptyCol(bool[,] data, int x, Dictionary<int, bool> emptyCols)
{
    if (emptyCols.ContainsKey(x))
    {
        return emptyCols[x];
    }

    for (int y = 0; y < data.GetLength(1); y++)
    {
        if (data[x, y]) return false;
    }

    return true;
}

void Print(bool[,] data)
{
    for (int x = data.GetLength(1) - 1; x >= 0; x--)
    {
        for (int y = 0; y < data.GetLength(0); y++)
        {
            var c = data[y, x];
            Console.Write(c ? '#' : '.');
        }

        Console.WriteLine();
    }
}