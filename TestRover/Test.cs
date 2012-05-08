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
            var output = Normalize(MarsRover.Program.Run(input));
            var testOutput = Normalize(Resources.TestOutput);
            Assert.AreEqual(output, testOutput);
        }

        /// <summary>
        /// Removes extra blank lines or whitespace.
        /// </summary>
        private static string Trim(string input)
        {
            return input.Trim('\r', '\n', ' ');
        }

        /// <summary>
        /// Files added to git can have their line endings altered.
        /// Standardize on one line ending for the tests, working around git.
        /// <see cref="http://help.github.com/line-endings/"/>
        /// </summary>
        private static string Normalize(string s)
        {
            return Trim(s.Replace("\r\n", "\n"));
        }
    }
}
