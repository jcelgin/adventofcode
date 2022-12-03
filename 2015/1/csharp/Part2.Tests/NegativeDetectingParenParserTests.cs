namespace Part2.Tests
{
    public class NegativeDetectingParenParserTests
    {
        [Theory]
        [InlineData(")", 1)]
        [InlineData("()())", 5)]
        public void Works(string input, int expected)
        {
            var actual = NegativeDetectingParenParser.Parse(input);
            Assert.Equal(expected, actual);
        }
    }
}