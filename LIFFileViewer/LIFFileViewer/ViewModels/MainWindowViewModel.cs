using LIFFileViewer.Data;
using LIFFileViewer.Interfaces;
using LIFFileViewer.LIFTools;
using LIFFileViewer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace LIFFileViewer.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string selectedFile;
        public string SelectedFile
        {
            get
            {
                return selectedFile;
            }
            set
            {
                selectedFile = value;
                OnPropertyChanged(nameof(SelectedFile));
            }
        }

        private string currentDirectory;
        public string CurrentDirectory
        {
            get
            {
                return currentDirectory;
            }
            set
            {
                currentDirectory = value;
                OnPropertyChanged(nameof(CurrentDirectory));
            }
        }

        private ObservableCollection<string> filesInCurrentDirectory;
        public ObservableCollection<string> FilesInCurrentDirectory
        {
            get
            {
                return filesInCurrentDirectory;
            }
            set
            {
                filesInCurrentDirectory = value;
                OnPropertyChanged(nameof(FilesInCurrentDirectory));
            }
        }

        private readonly IDataService data;
        private string LIFFilePath;

        private LIF _lif;
        public LIF lif
        {
            get
            {
                return _lif;
            }
            set
            {
                _lif = value;
                OnPropertyChanged(nameof(lif));
            }
        }

        //Default Constructor
        public MainWindowViewModel() : this(new DefaultDataService()) { }

        public MainWindowViewModel(IDataService data)
        {
            this.data = data;
            FilesInCurrentDirectory = new ObservableCollection<string>();
            //CurrentDirectory
            //SelectedFile
        }

        private SimpleCommand findAndLoadLIFFile;
        public SimpleCommand FindAndLoadLIFFile => findAndLoadLIFFile ?? (findAndLoadLIFFile = new SimpleCommand(
            () => !IsBusy,
            async () =>
            {
                LIFFilePath = await data.FindFileAsync();
                LoadLIFFile.RaiseCanExecuteChanged();
                LoadLIFDirectory.RaiseCanExecuteChanged();
                LoadDirectoryContents();

                if (!IsBusy && data.FileExists(LIFFilePath))
                {
                    IsBusy = true;
                    lif = await data.GetEntriesFromLIFAsync(LIFFilePath);

                    IsBusy = false;
                }
            }
            ));

        private SimpleCommand findLIFFile;
        public SimpleCommand FindLIFFile => findLIFFile ?? (findLIFFile = new SimpleCommand(
            () => !IsBusy, 
            async () =>
            {
                LIFFilePath = await data.FindFileAsync();
                LoadDirectoryContents();
                LoadLIFFile.RaiseCanExecuteChanged();
                LoadLIFDirectory.RaiseCanExecuteChanged();
            }
            ));

        private void LoadDirectoryContents()
        {
            CurrentDirectory = Path.GetDirectoryName(LIFFilePath);
            var ext = new List<string> { ".lif", ".LIF" };
            var files = Directory.GetFiles(CurrentDirectory, "*.*", SearchOption.AllDirectories)
                .Where(s => ext.Contains(Path.GetExtension(s))); //uses LINQ to get the results we want

            FilesInCurrentDirectory.Clear();
            foreach(string file in files)
            {
                FilesInCurrentDirectory.Add(Path.GetFileName(file));

                if(string.Equals(file, LIFFilePath))
                {
                    SelectedFile = FilesInCurrentDirectory.ElementAt<string>(FilesInCurrentDirectory.Count - 1);
                }
            }

        }

        private SimpleCommand loadLIFDirectory;
        public SimpleCommand LoadLIFDirectory => loadLIFDirectory ?? (loadLIFDirectory = new SimpleCommand(
            () => !IsBusy && data.FileExists(LIFFilePath), //sees if we can read in a LIF file
            async () =>
            {
                CurrentDirectory = await data.FindDirectoryAsync();
                LoadDirectoryContents();
                LoadLIFFile.RaiseCanExecuteChanged();
                LoadLIFDirectory.RaiseCanExecuteChanged();
            }
            ));

        private SimpleCommand loadLIFFile;
        public SimpleCommand LoadLIFFile => loadLIFFile ?? (loadLIFFile = new SimpleCommand(
            () => !IsBusy && data.FileExists(LIFFilePath), //sees if we can read in a LIF file
            async () =>
            {
                IsBusy = true;
                LIFFilePath = Path.Combine(CurrentDirectory, SelectedFile);
                lif = await data.GetEntriesFromLIFAsync(LIFFilePath);

                IsBusy = false;
            }
            ));

        private bool isBusy;
        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
                LoadLIFFile.RaiseCanExecuteChanged();
                FindLIFFile.RaiseCanExecuteChanged();
                LoadLIFDirectory.RaiseCanExecuteChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
