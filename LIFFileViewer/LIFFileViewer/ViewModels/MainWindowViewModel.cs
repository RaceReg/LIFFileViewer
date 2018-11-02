using LIFFileViewer.Data;
using LIFFileViewer.Interfaces;
using LIFFileViewer.LIFTools;
using LIFFileViewer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace LIFFileViewer.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
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
        }

        private SimpleCommand findAndLoadLIFFile;
        public SimpleCommand FindAndLoadLIFFile => findAndLoadLIFFile ?? (findAndLoadLIFFile = new SimpleCommand(
            () => !IsBusy,
            async () =>
            {
                LIFFilePath = await data.FindFileAsync();
                LoadLIFFile.RaiseCanExecuteChanged();

                if(!IsBusy && data.FileExists(LIFFilePath))
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
                LoadLIFFile.RaiseCanExecuteChanged();
            }
            ));

        private SimpleCommand loadLIFFile;
        public SimpleCommand LoadLIFFile => loadLIFFile ?? (loadLIFFile = new SimpleCommand(
            () => !IsBusy && data.FileExists(LIFFilePath), //sees if we can read in a LIF file
            async () =>
            {
                IsBusy = true;
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
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
