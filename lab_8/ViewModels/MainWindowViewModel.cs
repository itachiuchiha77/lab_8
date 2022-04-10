using lab_8.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace lab_8.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase __content;
        private static ObservableCollection<KanbanState> __todoStates { get; set; }
        private static ObservableCollection<KanbanState> __inprogressStates { get; set; }
        private static ObservableCollection<KanbanState> __finishedStates { get; set; }
        public static ObservableCollection<KanbanState> ToDoStates
        {
            set
            {
                __todoStates = value;
            }
            get => __todoStates;
        }
        public static ObservableCollection<KanbanState> InProgressStates
        {
            set
            {
                __inprogressStates = value;
            }
            get => __inprogressStates;
        }
        public static ObservableCollection<KanbanState> FinishedStates
        {
            set
            {
                __finishedStates = value;
            }
            get => __finishedStates;
        }
        public ViewModelBase Content
        {
            get => __content;
            private set => this.RaiseAndSetIfChanged(ref __content, value);
        }

        public void DeleteCompletedTask(KanbanState x) => FinishedStates.Remove(x);
        public void DeleteInProgressTask(KanbanState x)
        {
            InProgressStates.Remove(x);
            RefreshWindow();
        }
        public void DeleteToDoTask(KanbanState x) => ToDoStates.Remove(x);
        public MainWindowViewModel()
        {
            __finishedStates = new ObservableCollection<KanbanState>();
            __inprogressStates = new ObservableCollection<KanbanState>();
            __todoStates = new ObservableCollection<KanbanState>();
            RefreshWindow();
        }
        public void ReadDataFromFile(string path)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<ObservableCollection<KanbanState>>));
                using (StreamReader reader = new StreamReader(path))
                {
                    FinishedStates.Clear();
                    InProgressStates.Clear();
                    ToDoStates.Clear();
                    List<ObservableCollection<KanbanState>> states = (List<ObservableCollection<KanbanState>>)serializer.Deserialize(reader);
                    ToDoStates = states[0];
                    InProgressStates = states[1];
                    FinishedStates = states[2];
                }
            }
            catch
            {
                return;
            }
        }
        public void WriteDataToFile(string path)
        {
            try
            {
                List<ObservableCollection<KanbanState>> states = new List<ObservableCollection<KanbanState>>();
                states.Add(ToDoStates);
                states.Add(InProgressStates);
                states.Add(FinishedStates);
                using (StreamWriter writer = new StreamWriter(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<ObservableCollection<KanbanState>>));
                    serializer.Serialize(writer, states);
                }
            }
            catch
            {
                return;
            }
        }
        public void NewTable()
        {
            FinishedStates.Clear();
            InProgressStates.Clear();
            ToDoStates.Clear();
        }
        public static void AddToDo() => ToDoStates.Add(new KanbanState("Planned Task#" + (ToDoStates.Count + 1).ToString()));
        public static void AddDoing() => InProgressStates.Add(new KanbanState("In Progress Task#" + (InProgressStates.Count + 1).ToString()));
        public static void AddDone() => FinishedStates.Add(new KanbanState("Finished Task#" + (FinishedStates.Count + 1).ToString()));
        public void RefreshWindow() => Content = new StateListViewModel();
    }
}
