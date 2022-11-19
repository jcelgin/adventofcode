using System.Diagnostics;
using System.Text;

string Transform(string s, IReadOnlyDictionary<string, char[]> mutations)
{
    var sb = new StringBuilder();

    for (var i = 0; i < s.Length-1; i++)
    {
        var pattern = s.Substring(i, 2);
        var result = mutations[pattern];
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

var mutations = new Dictionary<string, char[]>();
for (var i = 2; i < lines.Length; i++)
{
    var tokens = lines[i].Split(" -> ");
    var value = new[] { tokens[0][0], tokens[1][0] };
    mutations.Add(tokens[0], value);
}

const int numTransformations = 10;

var result = pattern;

for (var i = 0; i < numTransformations; i++)
{
    var sw = Stopwatch.StartNew();
    result = Transform(result, mutations);
    sw.Stop();
    Console.WriteLine($"Completed step {i} in {sw.ElapsedMilliseconds}ms");
    //Console.WriteLine($"{i}: {result}");
}

var k = result.GroupBy(x => x).Select(x => new { x.Key, Count = x.Count() }).OrderBy(x => x.Count).ToArray();

var diff = k.Last().Count - k.First().Count;

Console.WriteLine($"Diff: {diff}");
