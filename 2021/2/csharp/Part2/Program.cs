if (args.Length != 1)
{
    throw new ArgumentException($"Expected 1 argument (file path), got {args.Length}");
}

string filePath = args[0];

if (!File.Exists(filePath))
{
    throw new FileNotFoundException(filePath);
}

var lines = await File.ReadAllLinesAsync(args[0]);

var x = 0;
var y = 0;
var aim = 0;

foreach (var line in lines)
{
    var tokens = line.Split(' ');
    if (tokens.Length != 2)
    {
        throw new Exception($"Expected two tokens, received {tokens.Length} for input `{line}`");
    }

    var direction = tokens[0]; 

    if (!int.TryParse(tokens[1], out var magnitude))
    {
        throw new Exception($"Unable to parse `{tokens[1]}` to int.");
    }

    switch (direction.ToLower())
    {
        case "forward":
            x += magnitude;
            y += aim * magnitude;
            break;
        case "up":
            aim -= magnitude;
            break;
        case "down":
            aim += magnitude;
            break;
        default:
            throw new ArgumentOutOfRangeException($"Unexpected direction `{direction}`");
    }
}

Console.WriteLine($"{x}, {y}: {x*y}");
