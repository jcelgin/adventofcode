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

var dict = new Dictionary<string, List<string>>();

const string startPoint = "start";
const string endPoint = "end";

void AddToDict(string key, string value)
{
    if (value == startPoint)
    {
        return;
    }

    if (key == endPoint)
    {
        return;
    }

    if (dict.TryGetValue(key, out var list))
    {
        list.Add(value);
    }
    else
    {
        dict.Add(key, new List<string> { value });
    }
}

List<List<string>> Traverse(string from, List<string> visited)
{
    visited.Add(from);

    if (from == endPoint)
    {
        return new List<List<string>> { visited };
    }

    // should we add this premise to some hashset to avoid reprocessing/infinite loop?

    if (!dict.TryGetValue(from, out var children))
    {
        throw new ApplicationException("Start value not found.");
    }

    var results = new List<List<string>>();

    foreach (var child in children)
    {
        if (child.ToLower() == child)
        {
            if (visited.Contains(child))
            {
                continue;
            }
        }

        results.AddRange(Traverse(child, new List<string>(visited)));
    }

    return results;
}


foreach (var line in lines)
{
    var tokens = line.Split('-');

    if (tokens.Length != 2)
    {
        throw new ApplicationException("Expected two nodes per line.");
    }

    AddToDict(tokens[0], tokens[1]);
    AddToDict(tokens[1], tokens[0]);
}

if (!dict.TryGetValue(startPoint, out var startNodes))
{
    throw new ApplicationException("Start value not found.");
}

var result = Traverse(startPoint, new List<string>());

Console.WriteLine($"{result.Count} paths:");

foreach (var r in result)
{
    Console.WriteLine($"\t{string.Join("-", r)}");
}

