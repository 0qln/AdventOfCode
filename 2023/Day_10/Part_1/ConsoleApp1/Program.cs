

using System.Diagnostics;
using Point = (int X, int Y);

var data = File.ReadAllLines(@"../../../input.txt");

char[,] plane = new char[data[0].Length, data.Length];
Point startingPos = new Point();
for (int i = 0; i < data[0].Length; i++)
{
    for (int j = 0; j < data.Length; j++)
    {
        plane[i, j] = data[^(j + 1)][i];
        if (plane[i, j] == 'S') startingPos = new Point(i, j);
    }
}

var ll = LoopLen(plane, startingPos);
Console.WriteLine(ll / 2 + 1);
Debug.Assert(ll == 13900);


ulong LoopLen(char[,] plane, Point startingPos)
{
    var result = 0UL;

    var prevPos = startingPos;
    var currPos = FirstStep(startingPos, plane);
    while (((currPos, prevPos) = (NextStep(prevPos, currPos, direction()), currPos))
        .currPos != startingPos)
    {
        result++;
    }
    
    return result;

    char direction() => plane[currPos.X, currPos.Y];
    void printInfo() => Console.WriteLine($"curr: {currPos} // prev: {prevPos} // dir: {direction()}");
}

Point FirstStep(Point curr, char[,] plane)
{
    char c;

    // no guaranty that curr is not on the border => bound checks needed

    if (curr.X + 1 < plane.GetLength(0))
    {
        c = plane[curr.X + 1, curr.Y];
        if (c == '-' || c == '7' || c == 'J')
            return (curr.X + 1, curr.Y);        
    }

    if (curr.X > 0)
    {
        c = plane[curr.X - 1, curr.Y];
        if (c == '-' || c == 'F' || c == 'L')
            return (curr.X - 1, curr.Y);
    }

    if (curr.Y + 1 < plane.GetLength(1))
    {
        c = plane[curr.X, curr.Y + 1];
        if (c == '|' || c == 'F' || c == '7')
            return (curr.X, curr.Y + 1);
    }

    if (curr.Y > 0)
    {
        c = plane[curr.X, curr.Y - 1];
        if (c == '|' || c == 'J' || c == 'L') 
            return (curr.X, curr.Y - 1);
    }

    throw new NotSupportedException();
}

Point NextStep(Point prev, Point curr, char direction)
{
    int dY = prev.Y - curr.Y, dX = prev.X - curr.X;
    return direction switch
    {
        '|' => (curr.X, curr.Y - dY),
        '-' => (curr.X - dX, curr.Y),
        'L' or '7' => (curr.X + dY, curr.Y + dX),
        'J' or 'F' => (curr.X - dY, curr.Y - dX),

        _ => (-1, -1)
    };
}


void TestNext()
{
    var directions = "|-LJ7F";
    for (int i = 0; i < directions.Length; i++)
    {
        Console.WriteLine($"Direction '{directions[i]}'");

        try
        {
            Point[] transition = directions[i] switch
            {
                '|' => [(0, 0), (0, 1), (0, 2)],
                '-' => [(0, 0), (1, 0), (2, 0)],
                'L' => [(1, 0), (0, 0), (0, 1)],
                'J' => [(0, 0), (1, 0), (1, 1)],
                '7' => [(0, 1), (1, 1), (1, 0)],
                'F' => [(0, 0), (1, 0), (1, 1)],

                _ => throw new NotImplementedException()
            };

            Point next;

            next = NextStep(transition[0], transition[1], directions[i]);
            Console.WriteLine($"Forward: {next == transition[2]}");

            next = NextStep(transition[2], transition[1], directions[i]);
            Console.WriteLine($"Backward: {next == transition[0]}");

        } catch (NotImplementedException e)
        {
            Console.WriteLine($"Not implemented");
        }

        Console.WriteLine();
    }
}


