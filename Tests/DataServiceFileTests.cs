using LIFFileViewer.Interfaces;
using LIFFileViewer.LIFTools;
using LIFFileViewer.ViewModels;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class DataServiceFileTests
    {
        [Test]
        public void FindDirectoryTest()
        {
            var dataService = new TestDataServce();
            var testVM = new MainWindowViewModel(dataService);

            testVM.LoadLIFDirectory.Execute(this);

            //HOW TO WAIT FOR TASK TO FINISH?

            Assert.AreEqual("D:\\", testVM.CurrentDirectory);
        }
    }

    public class TestDataServce : IDataService
    {
        public bool FileExists(string lifFILE)
        {
            if(string.Equals(lifFILE, "CorrectFile.lif"))
            {
                return true;
            }
            return false;
        }

        public Task<string> FindDirectoryAsync()
        {
            return Task.Factory.StartNew(() => { return "C:\\Users"; });
        }

        public Task<string> FindFileAsync()
        {
            throw new NotImplementedException();
        }

        public Task<LIF> GetEntriesFromLIFAsync(string lifFILE)
        {
            throw new NotImplementedException();
        }
    }
}
