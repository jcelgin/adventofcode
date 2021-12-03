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

var depths = lines.Select(int.Parse).ToArray();

var inc = 0;

if (depths.Count() < 3)
{
    Console.WriteLine(0);
    return;
}

var mostRecent = depths[0] + depths[1] + depths[2];

for(var i = 3; i < depths.Count(); i++)
{
    int value = mostRecent - depths[i - 3] + depths[i];

    if (value > mostRecent)
    {
        inc++;
    }

    mostRecent = value;
}

Console.WriteLine(inc);