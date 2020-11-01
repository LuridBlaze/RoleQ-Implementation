using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoleQueue.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    //public enum Role
    //{
    //    Tank,
    //    DamageDealer,
    //    Support
    //}
    public partial class MainWindow : Window
    {
        //I know there is a Queue type in c sharp, I still would like to handle the creation of the abstract queue type by myself, that's first of all. 
        //Second of all, I did this project to get more familiar with the WPF Library and its functionalities.
        //I know this sounds like an excuse for not using the Queue type, but tbh I don't care that much about it.
        public static Medium medium = new Medium();
        static public List<Player> queue;
        //For the JSON serrializer
        private const string SessionsFilename = "sessionsLogs.json";
        private const string PlayersFilename = "playerLogs.json";


        //One is for JSON, the other one is strictly for technical purpose
        public static List<Session> allSessions { get;  set; }
        public static List<Session> ongoingSessions = new List<Session>();
        
        public MainWindow()
        {
            try
            {
                LoadSessions();
            }
            catch
            {
                allSessions = new List<Session>();
                //queue = new List<Player>();
            }
            //In case one of the files gets deleted, we still boot up the thing, because they technically
            //don't affect each other, even if they were to contradict the other fella.
            try
            {
                LoadQueue();
            }
            catch
            {
                //allSessions = new List<Session>();
                queue = new List<Player>();
            }

            InitializeComponent();
            currentQueue.ItemsSource = queue;
            if (queue.Count() > 9)
                newSesh.IsEnabled = true;
        }
        
        private void currentQueue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        //Creates a new player with a random ID and role and instantly enqueues them.
        private void newPlayer_Click(object sender, RoutedEventArgs e)
        {
            queue.Add(medium.CreatePlayer());
            updateData();

            Console.WriteLine(this);
        }

        //Creates a session with the top-most 6 available players, obvi 2 for each of the three roles.
        private void newSesh_Click(object sender, RoutedEventArgs e)
        {
            Session sesh = medium.AssembleSession(queue);
            SessionWindow seshWin = new SessionWindow(sesh);
            // seshWin.Completed += updateData();
            seshWin.Show();
            allSessions.Add(sesh);
            ongoingSessions.Add(sesh);
            updateData();
        }
        //Pretty self-explanatory stuff right there
        public void updateData()
        {
            currentQueue.Items.Refresh();
            if (medium.sessionAvailable(queue))
                newSesh.IsEnabled = true;
            SaveSessions();
            SaveQueue();
        }

        #region Data Handling
        //====================================================================JSON

        private void LoadData()
        { 
            LoadSessions();
            LoadQueue();
        }
        private void SaveSessions()
        {
            using (var sw = new StreamWriter(SessionsFilename))
            {
                using (var jsonWriter = new JsonTextWriter(sw))
                {
                    var serilizer = new JsonSerializer()
                    {
                        Formatting = Newtonsoft.Json.Formatting.Indented
                    };

                    serilizer.Serialize(jsonWriter, allSessions);
                }
            }
        }
        private void SaveQueue()
        {
            using (var sw = new StreamWriter(PlayersFilename))
            {
                using (var jsonWriter = new JsonTextWriter(sw))
                {
                    var serilizer = new JsonSerializer()
                    {
                        Formatting = Newtonsoft.Json.Formatting.Indented
                    };

                    serilizer.Serialize(jsonWriter, queue);
                }
            }
        }
        //  finalSessions Не исполтзуется, но мб добавлю функционал позже
        private void LoadSessions()
        {
            using (var sr = new StreamReader(SessionsFilename))
            {
                using (var jsonReader = new JsonTextReader(sr))
                {
                    var serilizer = new JsonSerializer();

                    allSessions = serilizer.Deserialize<List<Session>>(jsonReader);

                }
            }
        }
        private void LoadQueue()
        {
            using (var sr = new StreamReader(PlayersFilename))
            {
                using (var jsonReader = new JsonTextReader(sr))
                {
                    var serilizer = new JsonSerializer();

                    queue = /* (List<Player>) */serilizer.Deserialize<List<Player>>(jsonReader);

                }
            }
        }
        #endregion

        //private void currentQueue_SourceUpdated(object sender, DataTransferEventArgs e)
        //{
        //    updateData();
        //}

        //Just added this for testing purposes, whenever the serializer didn't work as intended,
        //I just commented out the updateData() line so that I can use a healthy set of data every time without needing to reset it manually.
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            updateData();
        }
    }
}
