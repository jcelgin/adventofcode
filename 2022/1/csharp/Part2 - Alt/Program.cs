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
var elves = input.Split($"{Environment.NewLine}{Environment.NewLine}");
var result = elves.Select(x => x.Split(Environment.NewLine).Sum(int.Parse)).OrderByDescending(x => x).Take(3).Sum(x => x);
Console.WriteLine($"Result: {result}");
