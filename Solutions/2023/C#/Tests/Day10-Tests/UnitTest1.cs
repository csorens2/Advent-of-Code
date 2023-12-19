using Day10;

namespace Day10_Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Part1_1_Example_Test()
        {
            var input = Program.ParseInput("ExamplePart1-1.txt");
            Assert.Equal(4, Program.Part1(input));
        }

        [Fact]
        public void Part1_2_Example_Test()
        {
            var input = Program.ParseInput("ExamplePart1-2.txt");
            Assert.Equal(8, Program.Part1(input));
        }
    }
}