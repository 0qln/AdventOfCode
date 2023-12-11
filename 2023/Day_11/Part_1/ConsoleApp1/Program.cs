using System.Numerics;
using Point = (int X, int Y);

var DATA = File.ReadAllLines(@"../../../input.txt");
var galaxies = Galaxies(ExpandUniverse(GetUniverse(DATA)));
Console.WriteLine(Pairs(galaxies).Sum(Distance));


HashSet<(Point, Point)> Pairs(List<Point> points)
{
    var ret = new HashSet<(Point, Point)>();

    for (int i = 0; i < points.Count; i++)
    {
        for (int j =  0; j < points.Count; j++)
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


int Distance((Point, Point) points)
{
    return Math.Abs(points.Item1.X - points.Item2.X) + Math.Abs(points.Item1.Y - points.Item2.Y);
}

List<Point> Galaxies(bool[,] universe)
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


bool[,] ExpandUniverse(bool[,] data, int rate = 1)
{
    HashSet<int> emptyRows = new(), emptyCols = new();

    for (int x = 0; x < data.GetLength(0); x++) if (IsEmptyCol(data, x)) emptyCols.Add(x);
    for (int y = 0; y < data.GetLength(1); y++) if (IsEmptyRow(data, y)) emptyRows.Add(y);
    
    var ret = new bool[data.GetLength(0) + emptyCols.Count * rate, data.GetLength(1) + emptyRows.Count * rate];

    for (int rx = 0, dx = 0; dx < data.GetLength(0); rx++, dx++)
    {
        if (emptyCols.Contains(dx)) rx += rate;

        for (int ry = 0, dy = 0; dy < data.GetLength(1); ry++, dy++)
        {
            if (emptyRows.Contains(dy)) ry += rate;

            ret[rx, ry] = data[dx, dy];
        }
    }

    return ret;
}

bool IsEmptyRow(bool[,] data, int y)
{
    for (int x = 0; x < data.GetLength(0); x++)
    {
        if (data[x, y]) return false;
    }

    return true;
}

bool IsEmptyCol(bool[,] data, int x)
{
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