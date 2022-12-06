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

const int numChars = 14;
for (var i = 0; i < input.Length; i++)
{
    var end = i + numChars;
    var charCount = input[i..end].ToHashSet().Count;
    if (charCount == numChars)
    {
        Console.WriteLine($"Result: {end}");
        return;
    }
}