namespace Part1.Tests
{
    public class ParenParserTests
    {
        [Theory]
        [InlineData("(())", 0)]
        [InlineData("()()", 0)]
        [InlineData("(((", 3)]
        [InlineData("(()(()(", 3)]
        [InlineData("))(((((", 3)]
        [InlineData("())", -1)]
        [InlineData("))(", -1)]
        [InlineData(")))", -3)]
        [InlineData(")())())", -3)]
        public void Works(string input, int expected)
        {
            var actual = ParenParser.Parse(input);
            Assert.Equal(expected, actual);
        }
    }
}