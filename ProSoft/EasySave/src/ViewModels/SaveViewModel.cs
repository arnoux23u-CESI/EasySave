using EasySave.src.Models.Data;
using EasySave.src.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Data;

namespace EasySave.src.ViewModels
{
    /// <summary>
    /// ViewModel for the Save page
    /// </summary>
    public class SaveViewModel : INotifyPropertyChanged
    {

        /// <summary>
        /// Save items collection
        /// </summary>
        private readonly CollectionViewSource _saveItemsCollection;
        public ICollectionView SaveSourceCollection => _saveItemsCollection.View;

        /// <summary>
        /// is view visible
        /// </summary>
        public static bool IsVisible { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SaveViewModel()
        {
            ObservableCollection<Save> menuItems = new ObservableCollection<Save>(Save.GetSaves());
            _saveItemsCollection = new CollectionViewSource { Source = menuItems };
        }

        /// <summary>
        /// Get all saves names
        /// </summary>
        /// <returns>saves names</returns>
        public HashSet<string> GetSaves()
        {
            HashSet<string> data = new HashSet<string>();
            foreach (Save s in Save.GetSaves())
                data.Add(s.ToString());
            return data;
        }

        /// <summary>
        /// Create save method
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="src">source path</param>
        /// <param name="dest">destination path</param>
        /// <param name="type">type of save</param>
        /// <returns>save object</returns>
        public static Save CreateSave(string name, string src, string dest, SaveType type)
        {
            return Save.CreateSave(name, src, dest, type);
        }

        /// <summary>
        /// Edit save method
        /// </summary>
        /// <param name="s">save to edit</param>
        /// <param name="name">new name</param>
        public void EditSave(Save s, string name)
        {
            s.Rename(name);
        }

        /// <summary>
        /// Delete save method
        /// </summary>
        /// <param name="s">save to deletee</param>
        public void DeleteSave(Save s)
        {
            Save.Delete(s.uuid);
        }

        /// <summary>
        /// Pause save method
        /// </summary>
        /// <param name="s">save</param>
        public void PauseSave(Save s)
        {
            s.Pause();
            DirectoryUtils.PauseTransfer();
        }

        /// <summary>
        /// Resume save method
        /// </summary>
        /// <param name="s">save</param>
        public void ResumeSave(Save s)
        {
            s.Resume();
            DirectoryUtils.ResumeTransfer();
        }

        /// <summary>
        /// Cancel save method
        /// </summary>
        /// <param name="s">save</param>
        public void CancelSave(Save s)
        {
            s.Cancel();
        }

        /// <summary>
        /// Stop save method
        /// </summary>
        /// <param name="s">save</param>
        public void StopSave(Save s)
        {
            s.Stop();
        }

        /// <summary>
        /// Run save method
        /// </summary>
        /// <param name="save">save to run</param>
        public void RunSave(Save save)
        {
            new Thread(() =>
            {
                save.Run();
            }).Start();
        }

        /// <summary>
        /// Stop all saves
        /// </summary>
        public static void StopAllSaves()
        {
            foreach (Save s in Save.GetSaves())
            {
                s.Stop();
            }
            LogUtils.LogSaves();
        }

        /// <summary>
        /// Get saves by uuids
        /// </summary>
        /// <param name="names">uuids list of saves</param>
        /// <returns>list of saves</returns>
        public HashSet<Save> GetSavesByUuid(HashSet<string> names)
        {
            HashSet<Save> result = new HashSet<Save>();
            foreach (Save s in Save.GetSaves())
                foreach (var _ in from string name in names
                                  where name.Contains(s.uuid.ToString())
                                  select new { })
                    result.Add(s);
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}