using LIFFileViewer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LIFFileViewer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Hello World!";

        private ObservableCollection<Entry> entries;
        public ObservableCollection<Entry> Entries
        {
            get
            {
                return entries;
            }
            set
            {
                entries = value;
            }
        }

        public MainWindowViewModel()
        {
            entries = new ObservableCollection<Entry>();

            var tempEntry = new Entry();
            tempEntry.Place = 1;
            tempEntry.RacerId = 0001;
            tempEntry.Lane = 1;
            tempEntry.LastName = "Porter";
            tempEntry.FirstName = "Jackson";
            tempEntry.Affiliation = "SC";
            tempEntry.Time = DateTime.Now;

            entries.Add(tempEntry);
        }
    }
}
