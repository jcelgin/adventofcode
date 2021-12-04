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

var bitsPerLine = lines.First().Length;

Tuple<IEnumerable<string>, IEnumerable<string>> DetermineMajorVsMinor(IEnumerable<string> inputLines, int i)
{
    var bitGroups = inputLines.GroupBy(x => x[i]);
    var majorityThreshold = inputLines.Count() / 2;

    var majorityGroups = bitGroups.Where(x => x.Count() > majorityThreshold);

    IGrouping<char, string> majorGroup;
    if (!majorityGroups.Any())
    {
        majorGroup = bitGroups.First(x => x.Key == '1');
    }
    else
    {
        majorGroup = majorityGroups.Single();
    }

    var minorGroup = bitGroups.First(x => x.Key != majorGroup.Key);

    return new Tuple<IEnumerable<string>, IEnumerable<string>>(majorGroup.ToArray(), minorGroup.ToArray());
}

var majorBits = new char[bitsPerLine];
var minorBits = new char[bitsPerLine];

IEnumerable<string> majorLines = lines;
IEnumerable<string> minorLines = lines;

string? majorValueBinaryString = null;
string? minorValueBinaryString = null;

for (var i = 0; i < bitsPerLine; i++)
{
    if (majorValueBinaryString == null)
    {
        (majorLines, _) = DetermineMajorVsMinor(majorLines, i);
        if (majorLines.Count() == 1)
        {
            majorValueBinaryString = majorLines.Single();
        }
    }

    if (minorValueBinaryString == null)
    {
        (_, minorLines) = DetermineMajorVsMinor(minorLines, i);
        if (minorLines.Count() == 1)
        {
            minorValueBinaryString = minorLines.Single();
        }
    }
}

var majorValue = Convert.ToInt32(majorValueBinaryString, 2);
var minorValue = Convert.ToInt32(minorValueBinaryString, 2);

Console.WriteLine($"majorValue: {majorValueBinaryString} ({majorValue}), " +
    $"minorValue: {minorValueBinaryString} ({minorValue}), " +
    $"product: {majorValue * minorValue}");