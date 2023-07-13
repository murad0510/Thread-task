using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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

        public static bool IsPlay { get; set; }

        public static bool IsResume { get; set; }


        public RelayCommand EnterCommand { get; set; }
        public RelayCommand Play { get; set; }
        public RelayCommand Pause { get; set; }
        public RelayCommand Resume { get; set; }
        Thread thread;

        public void Load()
        {
            App.Current.Dispatcher.Invoke((System.Action)delegate
            {
                thread = new Thread(() =>
                {
                    int maxIndex = WordsListBox.Count - 1;
                    for (int i = maxIndex; i >= 0; i--)
                    {
                        if (IsPlay)
                        {
                            Thread.Sleep(1000);
                            timer_Tick(i);
                        }
                    }
                });
                thread.Start();
            });
        }

        public MainWindowViewModel()
        {
            EnterCommand = new RelayCommand((obj) =>
            {
                WordsListBox.Add(Word);
                if (IsPlay)
                {
                    Load();
                }
            });

            Play = new RelayCommand((obj) =>
            {
                if (!IsResume)
                {
                    IsPlay = true;
                    IsResume = true;
                    Load();
                }
                else
                {
                    MessageBox.Show("You pressed the play button without it", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });


            Resume = new RelayCommand((obj) =>
            {
                if (IsResume)
                {
                    IsPlay = true;

                    if (IsPlay)
                    {
                        Load();
                    }
                }
            });

            Pause = new RelayCommand((obj) =>
            {

                IsPlay = false;

            });
        }
        static string sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString($"x2");
            }
            return hash;
        }

        private void timer_Tick(int index)
        {
            if (IsPlay)
            {
                App.Current.Dispatcher.Invoke((System.Action)delegate
                {
                    try
                    {
                        var mixword = sha256(WordsListBox[index]);
                        WordsMixListBox.Add(mixword);
                        WordsListBox.Remove(WordsListBox[index]);
                        //Thread.Sleep(1000);

                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }
    }
}
