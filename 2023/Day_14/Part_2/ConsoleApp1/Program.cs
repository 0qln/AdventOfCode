

using System;
using System.Diagnostics.CodeAnalysis;

public class Program
{
    public static int Main(string[] args)
    {
        var data = File.ReadAllLines(@"../../../input.txt");
        var platform = new Platform(data);
        platform.SpinCycle(1000000000);
        Console.WriteLine(platform.TotalLoad(Direction.North));

        return 0;
    }
}





public class Platform
{
    private char[][] _values;


    public Platform(string[] strings)
    {
        this._values = strings.Select(x => x.ToCharArray()).ToArray();
    }

    public unsafe void SpinCycle(int cycles)
    {
        var comparer = new ValueEqualityComparer();
        Dictionary<char[][], int> history = new(comparer); // <platform, index>

        for (int i = 0; i < cycles;  i++)
        {
            if (history.ContainsKey(_values))
            {
                /* Repitition detected */

                int cycleLen = i - history[_values];
                int index = history[_values] + (cycles - history[_values]) % cycleLen;
                _values = 

                break;
            }

            history.Add(CloneValues(), i);

            Tilt(Direction.North);
            Tilt(Direction.West);
            Tilt(Direction.South);
            Tilt(Direction.East);
        }
    }


    public class ValueEqualityComparer : IEqualityComparer<char[][]>
    {
        public bool Equals(char[][]? x, char[][]? y)
        {
            Span<char[]> thisSpan = x;
            Span<char[]> otherSpan = y;

            for (int col = 0; col < thisSpan[0].Length; col++)
            {
                for (int row = 0; row < thisSpan.Length; row++)
                {
                    if (thisSpan[row][col] != otherSpan[row][col]) return false;
                }
            }

            return true;
        }

        public int GetHashCode([DisallowNull] char[][] obj)
        {
            int result = 0;

            for (int col = 0; col < obj[0].Length; col++)
            {
                for (int row = 0; row < obj.Length; row++)
                {
                    result ^= (obj[row][col] == 'O' ? 1 : 0) << (col + row * obj.Length);
                }
            }

            return result;

            //System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            //for (int col = 0; col < obj[0].Length; col++)
            //{
            //    for (int row = 0; row < obj.Length; row++)
            //    {
            //        stringBuilder.Append(obj[col][row]);
            //    }
            //}
            //return stringBuilder.ToString().GetHashCode();
        }
    }

    public char[][] CloneValues() => (char[][])_values.Select(x => (char[])x.Clone()).ToArray();

    public void Tilt(Direction direction)
    {
        Span<char[]> span = _values;

        switch (direction)
        {
            case Direction.North:
                for (int col = 0; col < span[0].Length; col++)
                {
                    for (int row = 0; row < span.Length; row++)
                    {
                        int begin = row, roundeds = 0;

                        while (row < span.Length && span[row][col] != '#')
                        {
                            if (span[row][col] == 'O') roundeds++;
                            row++;
                        }

                        while (row != begin)
                        {
                            span[begin++][col] = roundeds-- > 0 ? 'O' : '.';
                        }
                    }
                }
                break;

            case Direction.South:
                for (int col = 0; col < span[0].Length; col++)
                {
                    for (int row = span.Length-1; row >= 0; row--)
                    {
                        int begin = row, roundeds = 0;

                        while (row >= 0 && span[row][col] != '#')
                        {
                            if (span[row][col] == 'O') roundeds++;
                            row--;
                        }

                        while (row != begin)
                        {
                            span[begin--][col] = roundeds-- > 0 ? 'O' : '.';
                        }
                    }
                }
                break;

            case Direction.West:
                for (int row = 0; row < span.Length; row++)
                {
                    for (int col = 0; col < span[0].Length; col++)
                    {
                        int begin = col, roundeds = 0;

                        while (col < span[0].Length && span[row][col] != '#')
                        {
                            if (span[row][col] == 'O') roundeds++;
                            col++;
                        }

                        while (col != begin)
                        {
                            span[row][begin++] = roundeds-- > 0 ? 'O' : '.';
                        }
                    }
                }
                break;

            case Direction.East:
                for (int row = 0; row < span.Length; row++)
                {
                    for (int col = span[0].Length - 1; col >= 0; col--)
                    {
                        int begin = col, roundeds = 0;

                        while (col >= 0 && span[row][col] != '#')
                        {
                            if (span[row][col] == 'O') roundeds++;
                            col--;
                        }

                        while (col != begin)
                        {
                            span[row][begin--] = roundeds-- > 0 ? 'O' : '.';
                        }
                    }
                }
                break;
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
        _values[row][col] = c;
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


