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

var depths = lines.Select(x => int.Parse(x));

var mostRecent = int.MaxValue;

var inc = 0;

foreach (var value in depths)
{
    if (value > mostRecent)
    {
        inc++;
    }
    mostRecent = value;
}

Console.WriteLine(inc);



