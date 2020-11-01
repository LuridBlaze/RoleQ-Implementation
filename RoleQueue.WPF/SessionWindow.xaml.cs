using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
using System.Windows.Shapes;

namespace RoleQueue.WPF
{
    /// <summary>
    /// Interaction logic for SessionWindow.xaml
    /// </summary>
    public partial class SessionWindow : Window
    {
        Session ongoingSession;
        //public event SessionDone Completed;
        public SessionWindow(Session sesh)
        {
            InitializeComponent();
            ongoingSession = sesh;
            Team1.ItemsSource = ongoingSession.Team1;
            Team2.ItemsSource = ongoingSession.Team2;
            
        }

        private void Team1Win_Click(object sender, RoutedEventArgs e)
        {   
            ongoingSession.Cancelled = false;
            ongoingSession.SessionResults(true);
            
            this.Close();
        }

        private void Team2Win_Click(object sender, RoutedEventArgs e)
        {   
            ongoingSession.Cancelled = false;
            ongoingSession.SessionResults(false);
            
            this.Close();   
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.medium.ShutdownSession(ongoingSession, MainWindow.queue);
            MainWindow.ongoingSessions.Remove(ongoingSession);
            (Application.Current.MainWindow as MainWindow).updateData();
        }
    }
}
