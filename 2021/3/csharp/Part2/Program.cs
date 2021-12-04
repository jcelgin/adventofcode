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


Tuple<IGrouping<char, string>, IGrouping<char, string>> DetermineMajorVsMinor(IEnumerable<string> liiiiines, int i)
{
    var bitGroups = liiiiines.GroupBy(x => x[i]);
    var majorityThreshold = liiiiines.Count() / 2;

    var majorityGroups = bitGroups.Where(x => x.Count() > majorityThreshold);

    IGrouping<char, string> oxGroup;
    if (!majorityGroups.Any())
    {
        oxGroup = bitGroups.First(x => x.Key == '1');
    }
    else
    {
        oxGroup = majorityGroups.Single();
    }

    var co2Group = bitGroups.First(x => x.Key != oxGroup.Key);

    return new Tuple<IGrouping<char, string>, IGrouping<char, string>>(oxGroup, co2Group);
}

var majorBits = new char[bitsPerLine];
var minorBits = new char[bitsPerLine];

var (major, minor) = DetermineMajorVsMinor(lines, 0);

var majorLines = major.ToArray();
var minorLines = minor.ToArray();

string? majorValueBinaryString = null;
string? minorValueBinaryString = null;

for (var i = 1; i < bitsPerLine; i++)
{
    if (majorValueBinaryString == null)
    {
        var (majorResult, _) = DetermineMajorVsMinor(majorLines, i);
        majorLines = majorResult.ToArray();

        if (majorLines.Length == 1)
        {
            majorValueBinaryString = majorLines[0];
        }
    }

    if (minorValueBinaryString == null)
    {
        var (_, minorResult) = DetermineMajorVsMinor(minorLines, i);
        minorLines = minorResult.ToArray();

        if (minorLines.Length == 1)
        {
            minorValueBinaryString = minorLines[0];
        }
    }
}

var majorValue = Convert.ToInt32(majorValueBinaryString, 2);
var minorValue = Convert.ToInt32(minorValueBinaryString, 2);

Console.WriteLine($"majorValue: {majorValueBinaryString} ({majorValue}), " +
    $"minorValue: {minorValueBinaryString} ({minorValue}), " +
    $"product: {majorValue * minorValue}");
