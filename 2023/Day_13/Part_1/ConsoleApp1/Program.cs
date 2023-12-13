

string[][] fields = File.ReadAllText(@"../../../input.txt").Split("\r\n\r\n").Select(x => x.Split("\r\n")).ToArray();

Console.WriteLine(fields.Sum(GenerateNotes));

int GenerateNotes(string[] field)
{
    var colIdx = FindPerfectReflectionCol(field);
    var rowIdx = FindPerfectReflectionRow(field);

    if (colIdx == -1 && rowIdx == -1)
    {
        throw new ArgumentException("Now perfect reflection found");
    }

    var result = colIdx == -1 ? rowIdx * 100: colIdx;

    return result;
}

int FindPerfectReflectionRow(string[] field)
{
    for (int row = 1; row < field.Length; row++)
    {
        if (IsPerfectReflectionRow(field, row)) return row;
    }

    return -1;
}
int FindPerfectReflectionCol(string[] field)
{
    for (int col = 1; col < field[0].Length; col++)
    {
        if (IsPerfectReflectionCol(field, col)) return col;
    }

    return -1;
}

bool IsPerfectReflectionRow(string[] field, int row)
{
    int len = field.Length;
    int d = row-1, u = row;

    while (d >= 0 && u < len)
    {
        var down = GetRow(field, d--);
        var up = GetRow(field, u++);

        if (!down.SequenceEqual(up))
        {
            return false;
        }
    }

    return true;
}
bool IsPerfectReflectionCol(string[] field, int col)
{
    int len = field[0].Length;
    int l = col - 1, r = col;

    while (l >= 0 && r < len)
    {
        var left = GetCol(field, l--);
        var right = GetCol(field, r++);

        if (!left.SequenceEqual(right))
        {
            return false;
        }
    }

    return true;

}

char[] GetCol(string[] field, int index)
{
    char[] column = new char[field.Length];
    for (int i = 0; i < column.Length; i++)
        column[i] = field[i][index];   
    return column;
}
char[] GetRow(string[] field, int index)
{
    return field[index].ToCharArray();
}
