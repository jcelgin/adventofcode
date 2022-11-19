using System.Diagnostics;

Dictionary<char, long> CombineDictionaries(IReadOnlyDictionary<char, long> d1, IReadOnlyDictionary<char, long> d2)
{
    var combined = new Dictionary<char, long>(d2);

    foreach (var entry in d1)
    {
        if (!combined.TryAdd(entry.Key, entry.Value))
        {
            combined[entry.Key] += entry.Value;
        }
    }

    return combined;
}

var stepCache = new Dictionary<string, Dictionary<char, long>>();

Dictionary<char, long> RecursiveNightmare(int numSteps, ReadOnlySpan<char> pattern,
    IReadOnlyDictionary<string, char> mutations)
{
    var cacheKey = $"{numSteps}{pattern}";

    if (stepCache.TryGetValue(cacheKey, out var cacheHit))
    {
        return cacheHit;
    }

    var interstitialChar = mutations[new string(pattern)];

    if (numSteps == 1)
    {
        return interstitialChar == pattern[0]
            ? new Dictionary<char, long> { { interstitialChar, 2 } }
            : new Dictionary<char, long> { { pattern[0], 1 }, { interstitialChar, 1 } };
    }

//    Console.WriteLine($"{c0}{c1} =>  {interstitialChar}");

    var child0 = RecursiveNightmare(numSteps - 1, new[] { pattern[0], interstitialChar }, mutations);
    var child1 = RecursiveNightmare(numSteps - 1, new[] { interstitialChar, pattern[1] }, mutations);

    var stepResult = CombineDictionaries(child0, child1);

    stepCache.TryAdd(cacheKey, stepResult);

    return stepResult;
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

var inputPattern = lines[0];

var mutations = new Dictionary<string, char>();
for (var i = 2; i < lines.Length; i++)
{
    var tokens = lines[i].Split(" -> ");
    mutations.Add(tokens[0], tokens[1][0]);
}

const int numTransformations = 40;

var result = new Dictionary<char, long>();

for (var i = 0; i < inputPattern.Length-1; i++)
{
    var sw = Stopwatch.StartNew();
    var m = RecursiveNightmare(numTransformations, inputPattern.Substring(i,2), mutations);
    result = CombineDictionaries(result, m);
    sw.Stop();
    Console.WriteLine($"Chunk {i}: {sw.ElapsedMilliseconds} ms");
}

result[inputPattern.Last()]++;

var orderedResult = result.OrderBy(x => x.Value).ToArray();

foreach (var m in orderedResult)
{  
    Console.WriteLine($"{m.Key}: {m.Value}");
}

var diff = orderedResult.Last().Value - orderedResult.First().Value;

Console.WriteLine($"Diff: {diff}");
