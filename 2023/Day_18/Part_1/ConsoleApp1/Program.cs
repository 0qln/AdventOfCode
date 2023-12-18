

var data = File.ReadAllLines("../../../input.txt");

int minX = 0, maxX = 0, minY = 0, maxY  = 0, currX = 0, currY = 0;

foreach (var line in data)
{
    // execute

    switch (line[0])
    {
        case 'U': currY += int.Parse(line[2].ToString()); break;
        case 'R': currX += int.Parse(line[2].ToString()); break;
        case 'D': currY -= int.Parse(line[2].ToString()); break;
        case 'L': currX -= int.Parse(line[2].ToString()); break;
    }

    // update 

    minX = Math.Min(minX, currX);
    minY = Math.Min(minY, currY);
    maxX = Math.Max(maxX, currX);
    maxY = Math.Max(maxY, currY);
}

int width = Math.Abs(maxX) + Math.Abs(minX) + 1;
int height = Math.Abs(maxY) + Math.Abs(minY) + 1;

var map = new bool[width, height];

currX = Math.Abs(minX);
currY = Math.Abs(minY);

foreach (var line in data)
{
    int t, d = int.Parse(line[2].ToString());
    switch (line[0])
    {
        case 'U':
            t = currY;
            while (++currY < t + d)
                map[currX, currY] = true;
            break;
        case 'R':
            t = currX;
            while (++currX < t + d)
                map[currX, currY] = true;
            break;
        case 'D':
            t = currY;
            while (--currY > t - d)
                map[currX, currY] = true;
            break;
        case 'L':
            t = currX;
            while (--currX > t - d)
                map[currX, currY] = true;
            break;
    }
    map[currX, currY] = true;
}


ulong result = 0;
for (int y = height-1; y >= 0; y--)
{
    var left = new bool[width];
    var right = new bool[width];

    int ctr = 0;

    bool onBorder = false;

    for (int x = 0; x < width; x++)
    {
        if (map[x, y] && !onBorder) onBorder = true;
        if (!map[x, y]) onBorder = false;
        bool onBorderChanged = map[x, y] != onBorder;

        if (map[x, y])
            ctr++;

        left[x] = ctr % 2 == 0;
        right[x] = map[x, y];
    }

    //PrintFUNC(left, right, (a, b) => a | b);

    var negative = ConvertFUNC(left, right, (a, b) => a | b);
    PrintRAW(negative);
    PrintFUNC(negative, negative.Select(x => true).ToArray(), (a, b) => a ^ b);

    void PrintRAW(bool[] a)
    {
        for (int x = 0; x < width; x++)
            PrintBool(a[x]);
        
        Console.Write("     ");
    }

    bool[] ConvertFUNC(bool[] a, bool[] b, Func<bool, bool, bool> func)
    {
        var ret = new bool[width];
        for (int x = 0; x < width; x++)
            ret[x] = (func(a[x], b[x]));
        return ret;
    }

    void PrintFUNC(bool[] a, bool[] b, Func<bool, bool, bool> func)
    {
        PrintRAW(a);
        PrintRAW(b);
        PrintRAW(ConvertFUNC(a, b, func));
        
        Console.Write("     ");
    }

    Console.WriteLine();
}

Console.WriteLine(result);

Console.WriteLine();
Print();

void PrintBool(bool value) => Console.Write(value ? '#' : '.');


void Print()
{
    for (int y = height-1; y >= 0; y--)
    {
        for(int x = 0; x < width; x++)
        {
            PrintBool(map[x, y]);
        }

        Console.WriteLine();
    }
}