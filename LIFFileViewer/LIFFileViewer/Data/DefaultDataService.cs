using Avalonia.Controls;
using LIFFileViewer.Interfaces;
using LIFFileViewer.LIFTools;
using LIFFileViewer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LIFFileViewer.Data
{
    class DefaultDataService : IDataService
    {
        public bool FileExists(string lifFILE)
        {
            return File.Exists(lifFILE);
        }

        public Task<string> FindDirectoryAsync()
        {
            throw new NotImplementedException(); /////////////////////////////////
        }

        public async Task<string> FindFileAsync()
        {
            var openFileDialog = new OpenFileDialog()
            {
                AllowMultiple = false,
                Title = "Select FinishLynx LIF results file:",
                //Filters = { "*.lif" };
            };

            var pathArray = await openFileDialog.ShowAsync();

            if((pathArray?.Length ?? 0) > 0)
            {
                return pathArray[0];
            }
            return null;
        }

        public Task<LIF> GetEntriesFromLIFAsync(string lifFILE)
        {
            return Task.Run(() =>
            {
                var reader = new LIFReader(lifFILE);

                return reader.GetLIFObject();
            });
        }
    }
}
