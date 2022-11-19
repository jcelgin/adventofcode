using System.Diagnostics;

Dictionary<char, long> CombineDicts(Dictionary<char, long> d1, Dictionary<char, long> d2)
{
    foreach (var entry in d1)
    {
        if (d2.ContainsKey(entry.Key))
        {
            d2[entry.Key] += entry.Value;
        }
        else
        {
            d2.Add(entry.Key, entry.Value);
        }
    }

    return d2;
}

Dictionary<char, long> RecursiveNightmare(int numSteps, char c0, char c1, IReadOnlyDictionary<string, char> mutations)
{
    var interstitialChar = mutations[new string(new ReadOnlySpan<char>(new[] { c0, c1 }))];

    if (numSteps == 1)
    {
        return interstitialChar == c0
            ? new Dictionary<char, long> { { c0, 2 } }
            : new Dictionary<char, long> { { c0, 1 }, { interstitialChar, 1 } };
    }

//    Console.WriteLine($"{c0}{c1} =>  {interstitialChar}");

    var child0 = RecursiveNightmare(numSteps - 1, c0, interstitialChar, mutations);
    var child1 = RecursiveNightmare(numSteps - 1, interstitialChar, c1, mutations);

    return CombineDicts(child0, child1);
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

var mutations = new Dictionary<string, char>();
for (var i = 2; i < lines.Length; i++)
{
    var tokens = lines[i].Split(" -> ");
    mutations.Add(tokens[0], tokens[1][0]);
}

const int numTransformations = 25;

var result = new Dictionary<char, long>();

for (var i = 1; i < pattern.Length; i++)
{
    var sw = Stopwatch.StartNew();
    var c0 = pattern[i - 1];
    var c1 = pattern[i];
    var m = RecursiveNightmare(numTransformations, c0, c1, mutations);
    result = CombineDicts(result, m);
    sw.Stop();
    Console.WriteLine($"Chunk {i}: {sw.ElapsedMilliseconds} ms");
    Console.Beep();
}

result[pattern.Last()]++;

var orderedResult = result.OrderBy(x => x.Value).ToArray();

foreach (var m in orderedResult)
{  
    Console.WriteLine($"{m.Key}: {m.Value}");
}

var diff = orderedResult.Last().Value - orderedResult.First().Value;

Console.WriteLine($"Diff: {diff}");
