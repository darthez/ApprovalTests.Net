using System.Diagnostics;
using System.IO;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using ApprovalUtilities.Utilities;
using NUnit.Framework;

namespace ApprovalTests.Tests.Reporters
{
    [TestFixture]
    public class GenericDiffReporterTest
    {
        public static void StartProcess(string fullCommandLine)
        {
            var splitPosition = fullCommandLine.IndexOf('"', 1);
            var fileName = fullCommandLine.Substring(1, splitPosition - 1);
            var arguments = fullCommandLine.Substring(splitPosition + 1);
            Process.Start(fileName, arguments);
        }

        [Test]
        public void TestGetActualProgramFileEchos()
        {
            string NoneExistingFile = @"C:\ThisDirectoryShouldNotExist\ThisFileShouldNotExist.exe";
            Assert.AreEqual(NoneExistingFile, GenericDiffReporter.GetActualProgramFile(NoneExistingFile));
        }

        [Test]
        public void TestGetCurrentProject()
        {
            var file = PathUtilities.GetAdjacentFile("GenericDiffReporterTest.TestLaunchesBeyondCompareImage.approved.txt");
            string currentProjectFile = Path.GetFileName(VisualStudioProjectFileAdder.GetCurrentProjectFile(file));

            Assert.AreEqual("ApprovalTests.Tests.csproj", currentProjectFile);
        }

        [Test]
        public void TestGetCurrentProjectNotFound()
        {
            var project = VisualStudioProjectFileAdder.GetCurrentProjectFile("C:\\");

            Assert.AreEqual(null, project);
        }

        [Test]
        public void TestMissingDots()
        {
            var e =
                ExceptionUtilities.GetException(() => GenericDiffReporter.RegisterTextFileTypes(".exe", "txt", ".error", "asp"));
            Approvals.Verify(e);
        }

        [Test]
        public void TestProgramsExist()
        {
            Assert.IsFalse(new GenericDiffReporter("this_should_never_exist", "").IsWorkingInThisEnvironment("any.txt"));
        }

        [Test]
        public void TestRegisterWorks()
        {
            var r = new TortoiseDiffReporter();
            GenericDiffReporter.RegisterTextFileTypes(".myCrazyExtension");
            Assert.IsTrue(r.IsWorkingInThisEnvironment("file.myCrazyExtension"));
        }
    }
}