if (args.Length != 1)
{
    throw new ArgumentException($"Expected 1 argument (file path), got {args.Length}");
}

var filePath = args[0];

if (!File.Exists(filePath))
{
    throw new FileNotFoundException(filePath);
}

var lines = await File.ReadAllLinesAsync(filePath);

var illegalChars = new List<char>();
foreach (var line in lines)
{
    Console.WriteLine(line);
    var stack = new Stack<char>();
    var corrupted = false;
    foreach (var token in line.ToCharArray())
    {
        switch (token)
        {
            case '(':
                stack.Push(')');
                break;
            case '[':
                stack.Push(']');
                break;
            case '{':
                stack.Push('}');
                break;
            case '<':
                stack.Push('>');
                break;
            case ')':
            case ']':
            case '}':
            case '>':
                var p = stack.Pop();
                if (p != token)
                {
                    illegalChars.Add(token);
                    Console.WriteLine($"\tExpected {p}, but found {token} instead");
                    corrupted = true;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException("adsf");
        }

        if (corrupted)
        {
            break;
        }
    }
    if (!corrupted)
    {
        Console.WriteLine($"\tStack height: {stack.Count}");
    }
}

var result = 0;

foreach (var ic in illegalChars.GroupBy(x => x))
{
    var multiplier = 0;
    multiplier = ic.Key switch
    {
        ')' => 3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => throw new ArgumentOutOfRangeException("adsf"),
    };
    result += ic.Count() * multiplier;
}

Console.WriteLine($"Result: {result}");