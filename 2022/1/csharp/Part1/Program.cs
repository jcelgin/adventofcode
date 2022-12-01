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

var max = 0;
var runningTotal = 0;

foreach (var line in lines)
{
    if (line == string.Empty)
    {
        if (runningTotal > max)
        {
            max = runningTotal;
        }

        runningTotal = 0;
        continue;
    }

    runningTotal += int.Parse(line);
}

Console.WriteLine($"Result: {max}");