namespace Day1Tests
{
    public class Day1Tests
    {
        [Fact]
        public void Part1Example()
        {
            var input = Day1.Day1.ParseInput("Example.txt");
            var result = Day1.Day1.Part1(input);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Part1()
        {
            var input = Day1.Day1.ParseInput("Input.txt");
            var result = Day1.Day1.Part1(input);
            Assert.Equal(969, result);
        }

        [Fact]
        public void Part2Example()
        {
            var input = Day1.Day1.ParseInput("Example.txt");
            var result = Day1.Day1.Part2(input);
            Assert.Equal(6, result);
        }

        [Fact]
        public void Part2()
        {
            var input = Day1.Day1.ParseInput("Input.txt");
            var result = Day1.Day1.Part2(input);
            Assert.Equal(5887, result);
        }
    }
}
