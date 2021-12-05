internal record Coorindate
{
    public int X { get; }
    public int Y { get; }

    public Coorindate(string input)
    {
        var tokens = input.Split(',', StringSplitOptions.RemoveEmptyEntries);
        X = int.Parse(tokens[0]);
        Y = int.Parse(tokens[1]);
    }

    public Coorindate(int x, int y)
    {
        X = x;
        Y = y;
    }
}

