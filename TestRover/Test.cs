using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestRover
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void Do_the_two_rovers_in_the_problem_statement_move_as_expected()
        {
            var input = Resources.TestInput;
            var output = Trim(MarsRover.Program.Run(input));
            var testOutput = Trim(Resources.TestOutput);
            Assert.AreEqual(output, testOutput);
        }

        /// <summary>
        /// Removes extra blank lines or whitespace.
        /// </summary>
        private static string Trim(string input)
        {
            return input.Trim('\r', '\n', ' ');
        }
    }
}
