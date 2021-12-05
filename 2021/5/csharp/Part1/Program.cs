using System.Text.RegularExpressions;

if (args.Length != 1)
{
    throw new ArgumentException($"Expected 1 argument (file path), got {args.Length}");
}

string filePath = args[0];

if (!File.Exists(filePath))
{
    throw new FileNotFoundException(filePath);
}

var lines = await File.ReadAllLinesAsync(filePath);

var r = new Regex("(\\d*,\\d*)", RegexOptions.Compiled);

var map = new Dictionary<Coorindate, int>();

foreach(var line in lines)
{
    var m = r.Matches(line);
    if (m.Count != 2)
    {
        throw new Exception($"Unexpected input `{line}`");
    }

    var c0 = new Coorindate(m[0].Value);
    var c1 = new Coorindate(m[1].Value);

    Coorindate[] markedCoords;

    if (c0.X == c1.X)
    {
        markedCoords = GetNumbersInRangeInclusive(c0.Y, c1.Y)
            .Select(y => new Coorindate(c0.X, y))
            .ToArray();
    }
    else if (c0.Y == c1.Y)
    {
        markedCoords = GetNumbersInRangeInclusive(c0.X, c1.X)
            .Select(x => new Coorindate(x, c0.Y))
            .ToArray();
    }
    else
    {
        Console.WriteLine($"Skipping diagonal `{line}`");
        continue;
    }

    foreach (var c in markedCoords)
    {
        if (!map.ContainsKey(c))
        {
            map.Add(c, 1);
        }
        else
        {
            map[c]++;
        }
    }
}

var result = map.Aggregate(0, (sum, entry) => entry.Value > 1 ? sum + 1 : sum);

Console.WriteLine($"Dangerous positions: {result}");

IEnumerable<int> GetNumbersInRangeInclusive(int start, int end)
{
    if (start > end)
    {
        return GetNumbersInRangeInclusive(end, start).Reverse();
    }

    return Enumerable.Range(start, end - start + 1);
}