using System.Text;

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

var result = lines.Sum(line =>
{
    var div = line.IndexOf('|');
    var leftTokens = line[..div].Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var rightTokens = line[(div+1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries);

    var key = Decode(leftTokens);
    var translated = Translate(rightTokens, key);

    return int.Parse(translated);
});

Console.WriteLine($"Result: {result}");

Dictionary<string, char> Decode(string[] tokens)
{
    var d1 = tokens.Single(x => x.Length == 2).ToCharArray();
    var d4 = tokens.Single(x => x.Length == 4).ToCharArray();
    var d7 = tokens.Single(x => x.Length == 3).ToCharArray();
    var d8 = tokens.Single(x => x.Length == 7).ToCharArray();

    var charGroups = tokens.SelectMany(x => x.ToCharArray()).GroupBy(x => x);

    var freqChar = new Dictionary<int, HashSet<char>>();
    foreach (var group in charGroups)
    {
        if (freqChar.TryGetValue(group.Count(), out var value))
        {
            value.Add(group.Key);
        }
        else
        {
            freqChar.Add(group.Count(), new HashSet<char> { group.Key });
        }
    }

    var leftTop = freqChar[6].Single();
    var leftBottom = freqChar[4].Single();

    var rightBottom = freqChar[9].Single();

    var middle = d4.Except(d1).Single(x => x != leftTop);
    var rightTop = d1.Single(x => x != rightBottom);

    // unused but here they are
    // var top = d7.Except(d1).Single();
    // var bottom = d8.Except(d7).Except(d4).Single(x => x != leftBottom);

    var d = new Dictionary<IEnumerable<char>, char>
    {
        { d8.Except(new[] { middle }), '0' },
        { d1, '1' },
        { d8.Except(new[] { leftTop, rightBottom }), '2' },
        { d8.Except(new[] { leftTop, leftBottom }), '3' },
        { d4, '4' },
        { d8.Except(new[] { rightTop, leftBottom }), '5' },
        { d8.Except(new[] { rightTop }), '6' },
        { d7, '7' },
        { d8, '8' },
        { d8.Except(new[] { leftBottom }), '9' }
    };

    return d.ToDictionary(x => new string(x.Key.OrderBy(y => y).ToArray()), x => x.Value);
}

string Translate(IReadOnlyCollection<string> tokens, Dictionary<string, char> lookup)
{
    var sb = new StringBuilder(tokens.Count);
    foreach (var token in tokens)
    {
        var ordered = new string(token.ToCharArray().OrderBy(x => x).ToArray());
        if (lookup.TryGetValue(ordered, out var value))
        {
            sb.Append(value);
        }
        else
        {
            throw new Exception("!!!");
        }
    }
    return sb.ToString();
}