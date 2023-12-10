
        
using Point = (int X, int Y);

var data = File.ReadAllLines(@"../../../input.txt");

//PrintPlane(plane);
//PrintPlane(Expand(plane));
//PrintPlane(Fix(Expand(plane)));

Console.WriteLine(EnclosedTileCount(GetPlane(data)));

char[,] GetPlane(string[] data)
{
    char[,] plane = new char[data[0].Length, data.Length];

    for (int i = 0; i < data[0].Length; i++)
    {
        for (int j = 0; j < data.Length; j++)
        {
            plane[i, j] = data[^(j + 1)][i];
        }
    }

    return plane;
}

ulong EnclosedTileCount(char[,] plane)
{
    // TODO:
    /* By Expanding the plane we found out, where the animal can escape,
     * but now the plane is too big and there are too many inner tiles.
     * 
     * We have to shrink down the plane again, momoazing, where the animal 
     * can escape (??) 
     * then count the inner tiles...
     */
    var expanded = RepairConnections(Expand(plane));
    PrintPlane(expanded);
    var loop = FindLoop(expanded);
    PrintPlane(loop);
    var outer = FindOuter(loop);
    PrintPlane(outer);
    var inner = FindInner(loop, outer);
    PrintPlane(inner);

    var l = FindLoop(plane);
    var o = FindOuter(l);
    var i = FindInner(l, o);
    PrintPlane(i);

    return Count(inner, c => c == 'i');
}

ulong Count(char[,] plane, Func<char, bool> func)
{
    var result = 0UL;
    for (int x = 0; x < plane.GetLength(0); x++)
        for (int y = 0; y < plane.GetLength(1); y++)
            if (func(plane[x, y]))
                result++;
    return result;
}

char[,] Expand(char[,] plane)
{
    char[,] result = new char[plane.GetLength(0) * 2, plane.GetLength(1) * 2];

    for (int x = 0; x < result.GetLength(0); x++)
        for (int y = 0; y < result.GetLength(1); y++)
            result[x, y] = x % 2 == 0 && y % 2 == 0 ? plane[x / 2, y / 2] : '.';

    return result;
}

char[,] RepairConnections(char[,] plane)
{
    for (int x = 0; x < plane.GetLength(0); x += 2)
    {
        for (int y = 0; y < plane.GetLength(1); y += 2)
        {
            if (x + 2< plane.GetLength(0))
            {
                bool up = plane[x + 2, y] switch { '7' or '-' or 'J' or 'S' => true, _ => false };
                plane[x + 1, y] = up ? plane[x, y] switch { 'F'or '-' or 'L' or 'S' => '-', _ => '.' } : '.';
            }

            if (y + 2 < plane.GetLength(1))
            {
                bool right = plane[x, y + 2] switch { '7' or '|' or 'F' or 'S' => true, _ => false, };
                plane[x, y + 1] = right ? plane[x, y] switch { 'L' or '|' or 'J' or 'S' => '|', _ => '.' } : '.';
            }
        }
    }                

    return plane;
}

char[,] FindLoop(char[,] plane)
{
    Point startingPos = (-1, -1);
    char[,] result = new char[plane.GetLength(0), plane.GetLength(1)];
    for (int x = 0; x < result.GetLength(0); x++)
    {
        for (int y = 0; y < result.GetLength(1); y++)
        {
            result[x, y] = '.';
            if (plane[x, y] == 'S') startingPos = (x, y);
        }
    }

    var prevPos = startingPos;
    var currPos = FirstStep(startingPos, plane);
    result[startingPos.X, startingPos.Y] = 'x';
    result[currPos.X, currPos.Y] = 'x';
    while (((currPos, prevPos) = (NextStep(prevPos, currPos, direction()), currPos)).currPos != startingPos)
        result[currPos.X, currPos.Y] = 'x';

    return result;

    char direction() => plane[currPos.X, currPos.Y];
}

char[,] FindInner(char[,] loop, char[,] outer)
{
    char[,] result = new char[loop.GetLength(0), loop.GetLength(1)];
    for (int x = 0; x < result.GetLength(0); x++)
    {
        for (int y = 0; y < result.GetLength(1); y++)
        {
            if (loop[x, y] == '.' &&
                outer[x, y] == '.')
            {
                result[x,y] = 'i';
            }
            else
            {
                result[x, y] = '.';
            }
        }
    }

    return result;
}

char[,] FindOuter(char[,] loop)
{
    char[,] result = new char[loop.GetLength(0), loop.GetLength(1)];
    for (int x = 0; x < result.GetLength(0); x++)
        for (int y = 0; y < result.GetLength(1); y++)
            result[x, y] = '.';

    // go along edges 
    for (int x = 0; x < result.GetLength(0); x++)
    {
        int y;

        y= 0;
        // expand in all directions
        // leave marks on `result`, where visited
        MarkAdjacendExplode(result, loop, (x, y), c => c == '.', 'o');
        

        y = result.GetLength(1) - 1;
        // expand in all directions
        // leave marks on `result`, where visited
        MarkAdjacendExplode(result, loop, (x, y), c => c == '.', 'o');
    }
    for (int y = 0; y < result.GetLength(1); y++)
    {
        int x;

        x = 0;
        // expand in all directions
        // leave marks on `result`, where visited
        MarkAdjacendExplode(result, loop, (x, y), c => c == '.', 'o');


        x = result.GetLength(0) - 1;
        // expand in all directions
        // leave marks on `result`, where visited
        MarkAdjacendExplode(result, loop, (x, y), c => c == '.', 'o');
    }

    return result;
}

void MarkAdjacendExplode(char[,] writePlane, char[,] readPlane, Point pos, Func<char, bool> condition, char mark)
{
    if (pos.X < 0 || pos.Y < 0 || pos.X >= readPlane.GetLength(0) || pos.Y >= readPlane.GetLength(1))
    {
        return;
    }

    if (condition(readPlane[pos.X, pos.Y]) && condition(writePlane[pos.X, pos.Y]))
    {
        writePlane[pos.X, pos.Y] = mark;
    }
    else
    {
        return;
    }

    MarkAdjacendExplode(writePlane, readPlane, (pos.X + 1, pos.Y), condition, mark);
    MarkAdjacendExplode(writePlane, readPlane, (pos.X - 1, pos.Y), condition, mark);
    MarkAdjacendExplode(writePlane, readPlane, (pos.X, pos.Y + 1), condition, mark);
    MarkAdjacendExplode(writePlane, readPlane, (pos.X, pos.Y - 1), condition, mark);
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

        _ => throw new NotImplementedException()
    };
}
void PrintPlane(char[,] plane)
{
    for (int x = plane.GetLength(1) - 1; x >= 0; x--)
    {
        for (int y = 0; y < plane.GetLength(0); y++)
        {
            Console.Write(plane.GetValue(y, x));
        }

        Console.WriteLine();
    }
}