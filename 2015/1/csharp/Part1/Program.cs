using Part1;

if (args.Length != 1)
{
    throw new ArgumentException($"Expected 1 argument (file path), got {args.Length}");
}

var filePath = args[0];

if (!File.Exists(filePath))
{
    throw new FileNotFoundException(filePath);
}

var input = await File.ReadAllTextAsync(filePath);

Console.WriteLine($"Result: {ParenParser.Parse(input)}");