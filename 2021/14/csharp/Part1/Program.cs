using System.Text;

string Transform(string s, IReadOnlyDictionary<string, string> mutations)
{
    var sb = new StringBuilder();

    for (var i = 1; i < s.Length; i++)
    {
        var pattern = $"{s[i - 1]}{s[i]}";

        if (!mutations.TryGetValue(pattern, out var result))
        {
            throw new Exception($"Pattern {pattern} not found in mutations.");
        }

        sb.Append(result);
    }

    sb.Append(s.Last());

    return sb.ToString();
}

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

var pattern = lines[0];

var mutations = new Dictionary<string, string>();
for (var i = 2; i < lines.Length; i++)
{
    var tokens = lines[i].Split(" -> ");
    mutations.Add(tokens[0], $"{tokens[0][0]}{tokens[1]}");
}

const int numTransformations = 10;

var result = pattern;
for (var i = 0; i < numTransformations; i++)
{
    result = Transform(result, mutations);
//    Console.WriteLine($"{i}: {result}");
}

var k = result.GroupBy(x => x).Select(x => new { x.Key, Count = x.Count() }).OrderBy(x=>x.Count).ToArray();

var diff = k.Last().Count - k.First().Count;

Console.WriteLine($"Diff: {diff}");
