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

var result = lines.Sum(line =>
{
    return line[(line.IndexOf('|') + 1)..]
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Count(x => x.Length is 2 or 3 or 4 or 7);
});

Console.WriteLine($"Result: {result}");