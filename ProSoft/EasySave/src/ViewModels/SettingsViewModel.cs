using EasySave.Properties;
using EasySave.src.Models;
using EasySave.src.Render;
using EasySave.src.Render.Views;
using EasySave.src.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace EasySave.src.ViewModels
{
    public class SettingsViewModel
    {
        private readonly CollectionViewSource _cryptoSoftSettingsItemsCollection, _langItemsCollection, _logItemsCollection;
        public ICollectionView CryptoSoftSourceCollection => _cryptoSoftSettingsItemsCollection.View;
        public ICollectionView LangSourceCollection => _langItemsCollection.View;
        public ICollectionView LogsSourceCollection => _logItemsCollection.View;

        public SettingsViewModel()
        {
            ObservableCollection<SettingsItem> cryptoSoftItems = new ObservableCollection<SettingsItem>
            {
                new SettingsItem { SettingsName = $"{Resource.Settings_Secret}", SettingsValue = DirectoryUtils.GetSecret(), SettingsCommand = new RelayCommand(ChangeKey) },
                new SettingsItem { SettingsName = $"{Resource.Settings_Extensions}", SettingsValue = DirectoryUtils.GetExtensions(), SettingsCommand = new RelayCommand(ChangeExtensions) },
            };
            ObservableCollection<SettingsItem> langItems = new ObservableCollection<SettingsItem>
            {
                new SettingsItem { SettingsName = $"{Resource.Lang_fr_FR}", SettingsImage = @"Assets/fr_FR.png", SettingsCommand = new RelayCommand(ChangeLanguage) },
                new SettingsItem { SettingsName = $"{Resource.Lang_en_US}", SettingsImage = @"Assets/en_US.png", SettingsCommand = new RelayCommand(ChangeLanguage) },
                new SettingsItem { SettingsName = $"{Resource.Lang_it_IT}", SettingsImage = @"Assets/it_IT.png", SettingsCommand = new RelayCommand(ChangeLanguage) },
                new SettingsItem { SettingsName = $"{Resource.Lang_ru_RU}", SettingsImage = @"Assets/ru_RU.png", SettingsCommand = new RelayCommand(ChangeLanguage) },
            };
            ObservableCollection<SettingsItem> logItems = new ObservableCollection<SettingsItem>
            {
                new SettingsItem { SettingsName = "JSON", SettingsImage = @"Assets/json.png", SettingsCommand = new RelayCommand(ChangeLogsFormat) },
                new SettingsItem { SettingsName = "XML", SettingsImage = @"Assets/xml.png", SettingsCommand = new RelayCommand(ChangeLogsFormat) },
            };
            _cryptoSoftSettingsItemsCollection = new CollectionViewSource { Source = cryptoSoftItems };
            _langItemsCollection = new CollectionViewSource { Source = langItems };
            _logItemsCollection = new CollectionViewSource { Source = logItems };
        }

        private static void ChangeLanguage(object culture)
        {
            var cultureData = "";
            switch ((string)culture)
            {
                case var _ when (string)culture == Resource.Lang_fr_FR:
                    cultureData = "fr-FR";
                    break;
                case var _ when (string)culture == Resource.Lang_en_US:
                    cultureData = "en-US";
                    break;
                case var _ when (string)culture == Resource.Lang_it_IT:
                    cultureData = "it-IT";
                    break;
                case var _ when (string)culture == Resource.Lang_ru_RU:
                    cultureData = "ru-RU";
                    break;
                    
            }
            CultureInfo cultureInfo = new CultureInfo(cultureData.ToString());
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            var windows = Application.Current.MainWindow;
            Application.Current.MainWindow = new MainWindow();
            windows.Close();
            Application.Current.MainWindow.Show();
        }

        private static void ChangeLogsFormat(object format)
        {
            switch ((string)format)
            {
                case "JSON":
                    LogUtils.ChangeFormat(LogsFormat.JSON);
                    break;
                case "XML":
                    LogUtils.ChangeFormat(LogsFormat.XML);
                    break;
            }
        }

        public static void ChangeKey(object obj)
        {
            DirectoryUtils.ChangeKey((string)obj);
        }

        public static void ChangeExtensions(object obj)
        {
            DirectoryUtils.ChangeExtensions(((string)obj).Split("\r\n").ToHashSet());
        }

    }
}