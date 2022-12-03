namespace Part2;

public static class NegativeDetectingParenParser
{
    public static int Parse(string input)
    {
        var result = 0;
        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i];
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
            if (result < 0)
            {
                return i + 1;
            }
        }

        return int.MaxValue;
    }
}