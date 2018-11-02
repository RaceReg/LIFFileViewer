using LIFFileViewer.Interfaces;
using LIFFileViewer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIFFileViewer.Data
{
    class DefaultDataService : IDataService
    {
        public bool FileExists(string lifFILE)
        {
            throw new NotImplementedException();
        }

        public Task<string> FindFileAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Entry>> GetEntriesFromLIFAsync(string lifFILE)
        {
            throw new NotImplementedException();
        }
    }
}
