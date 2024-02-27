using Microsoft.Win32;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Movies_player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MediaElement mediaPlayer;

        private DispatcherTimer timer;


        public MainWindow()
        {
            InitializeComponent();
            InitializeMediaPlayer(); // Create the mediaElement Object
            InitializeTimer();
        }

        private void InitializeMediaPlayer()
        {
            mediaPlayer = new MediaElement
            {
                LoadedBehavior = MediaState.Manual,
                UnloadedBehavior = MediaState.Stop
            };
            grid.Children.Add(mediaPlayer);


        }
        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Audio Files|*.mp3;*.wav;*.wma;*.m4a"; // Filtre pour les fichiers audio
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic); // Dossier initial

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = openFileDialog.FileName;
                OpenText.Text = selectedFilePath;

                mediaPlayer.Source = new Uri(selectedFilePath);
                
                mediaPlayer.Play();

                timer.Start();

                // Ici, vous pouvez utiliser selectedFilePath pour charger le fichier audio dans votre application
                MessageBox.Show($"Fichier sélectionné : {selectedFilePath}");
            }
        }

        

        private void InitializeTimer()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                barProgress.Maximum = mediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                barProgress.Value = mediaPlayer.Position.TotalSeconds;
            }
        }

        private void barVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = (double)e.NewValue;
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Pause();
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
        }
    }
}