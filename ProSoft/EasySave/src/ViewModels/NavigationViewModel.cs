﻿
/// <summary>
/// ViewModel - [ "The Connector" ]
/// ViewModel exposes data contained in the Model objects to the View. The ViewModel performs 
/// all modifications made to the Model data.
/// </summary>

using EasySave.src.Models;
using EasySave.src.Render;
using EasySave.src.Render.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace EasySave.src.ViewModels
{
    class NavigationViewModel : INotifyPropertyChanged
    {
        // CollectionViewSource enables XAML code to set the commonly used CollectionView properties,
        // passing these settings to the underlying view.
        private CollectionViewSource MenuItemsCollection;

        // ICollectionView enables collections to have the functionalities of current record management,
        // custom sorting, filtering, and grouping.
        public ICollectionView SourceCollection => MenuItemsCollection.View;

        public NavigationViewModel()
        {
            // ObservableCollection represents a dynamic data collection that provides notifications when items
            // get added, removed, or when the whole list is refreshed.
            ObservableCollection<MenuItems> menuItems = new ObservableCollection<MenuItems>
            {
                new MenuItems { MenuName = "Home", MenuImage = @"Assets/Home_Icon.png" },
            };

            MenuItemsCollection = new CollectionViewSource { Source = menuItems };

            // Set Startup Page
            SelectedViewModel = new HomeViewModel();
        }

        // Implement interface member for INotifyPropertyChanged.
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        // Select ViewModel
        private object _selectedViewModel;
        public object SelectedViewModel
        {
            get => _selectedViewModel;
            set { _selectedViewModel = value; OnPropertyChanged("SelectedViewModel"); }
        }

        // Switch Views
        public void SwitchViews(object parameter)
        {
            switch (parameter)
            {
                case "Home":
                    SelectedViewModel = new HomeViewModel();
                    break;
            }
        }

        // Menu Button Command
        private ICommand _menucommand;
        public ICommand MenuCommand
        {
            get
            {
                if (_menucommand == null)
                {
                    _menucommand = new RelayCommand(param => SwitchViews(param));
                }
                return _menucommand;
            }
        }

        // Show Home View
        private void ShowHome()
        {
            SelectedViewModel = new HomeViewModel();
        }

        // Back button Command
        private ICommand _backHomeCommand;
        public ICommand BackHomeCommand
        {
            get
            {
                if (_backHomeCommand == null)
                {
                    _backHomeCommand = new RelayCommand(p => ShowHome());
                }
                return _backHomeCommand;
            }
        }

        // Close App
        public void CloseApp(object obj)
        {
            MainWindow win = obj as MainWindow;
            win.Close();
        }

        // Close App Command
        private ICommand _closeCommand;
        public ICommand CloseAppCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(p => CloseApp(p));
                }
                return _closeCommand;
            }
        }

    }
}
