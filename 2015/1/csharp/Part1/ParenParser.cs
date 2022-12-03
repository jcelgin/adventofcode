namespace Part1;

public static class ParenParser
{
    public static int Parse(string input)
    {
        var result = 0;
        foreach (var c in input)
        {
            switch (c)
            {
                case '(':
                    result++;
                    break;
                case ')':
                    result--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected character {c}");
            }
        }

        return result;
    }
}