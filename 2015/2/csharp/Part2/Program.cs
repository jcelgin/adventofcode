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

var total = 0;
foreach (var line in lines)
{
    var tokens = line.Split('x').Select(int.Parse).OrderBy(x => x).ToArray();

    if (tokens.Length != 3)
    {
        throw new ApplicationException("We only deal in three dimensions, sir.");
    }

    // ReSharper disable once ArrangeRedundantParentheses
    total += (tokens[0] + tokens[1]) * 2 + (tokens[0] * tokens[1] * tokens[2]);
}

Console.WriteLine($"Result: {total}");