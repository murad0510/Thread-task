using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Thread_task.Commands;

namespace Thread_task.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private string word;

        public string Word
        {
            get { return word; }
            set { word = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> wordsListBox = new ObservableCollection<string>();

        public ObservableCollection<string> WordsListBox
        {
            get { return wordsListBox; }
            set { wordsListBox = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> wordsMixListBox = new ObservableCollection<string>();

        public ObservableCollection<string> WordsMixListBox
        {
            get { return wordsMixListBox; }
            set { wordsMixListBox = value; OnPropertyChanged(); }
        }


        public RelayCommand EnterCommand { get; set; }
        public RelayCommand Play { get; set; }
        public RelayCommand Pause { get; set; }
        public RelayCommand Resume { get; set; }
        public RelayCommand Stop { get; set; }
        Thread thread;
        public MainWindowViewModel()
        {
            //App.Current.Dispatcher.Invoke(() =>
            //{
            thread = new Thread(() =>
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timer.Start();
            });
            //});

            EnterCommand = new RelayCommand((obj) =>
            {
                WordsListBox.Add(Word);
            });

            Play = new RelayCommand((obj) =>
            {
                thread.Start();
            });

            Pause = new RelayCommand((obj) =>
            {
                thread.Suspend();
            });

            Stop = new RelayCommand((obj) =>
            {
                thread.Abort();
            });
        }
        static string sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
            Random random = new Random();
            var a = random.Next(1, 10);
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString($"x{a}");
            }
            return hash;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            WordsMixListBox.Clear();
            for (int i = 0; i < WordsListBox.Count; i++)
            {
                var mixword = sha256(WordsListBox[i]);
                WordsMixListBox.Add(mixword);
            }
        }
    }
}
