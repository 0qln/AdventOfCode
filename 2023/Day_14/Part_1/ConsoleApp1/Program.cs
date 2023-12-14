

public class Program
{
    public static int Main(string[] args)
    {
        var data = File.ReadAllLines(@"../../../input.txt");
        var platform = new Platform(data);
        platform.Print();
        platform.Tilt(Direction.North);
        platform.Print();
        Console.WriteLine(platform.TotalLoad(Direction.North));

        return 0;
    }
}





public class Platform
{
    //private char[,] _values;
    private string[] _values;

    public Platform(string[] strings)
    {
        this._values = strings;
    }

    public void Tilt(Direction direction)
    {
        if (direction != Direction.North)
        {
            throw new NotImplementedException();
        }

        for (int col = 0; col < _values[0].Length; col++)
        {
            for (int row = 0; row < _values.Length; row++)
            {
                int begin = row, roundeds = 0;

                while (row < _values.Length && _values[row][col] != '#')
                {
                    if (_values[row][col] == 'O') roundeds++;
                    row++;
                }

                while (row != begin)
                {
                    Replace(begin++, col, roundeds-- > 0 ? 'O' : '.');
                }
            }
        }
    }

    public void Print()
    {
        foreach (var line in _values)
        {
            Console.WriteLine(line);
        }
        Console.WriteLine();
    }

    private void Replace(int row, int col, char c)
    {
        System.Text.StringBuilder strBuilder = new System.Text.StringBuilder(_values[row]);
        strBuilder[col] = c;
        _values[row] = strBuilder.ToString();
    }

    public int TotalLoad(Direction direction = Direction.North)
    {
        if (direction != Direction.North)
        {
            throw new NotImplementedException();
        }

        int load = 0;

        for (int row = 0; row < _values.Length; row++)
        {
            load += _values[row].Count(c => c == 'O') * (_values.Length - row);
        }

        return load;
    }
}


public enum Direction { North, East, South, West }


