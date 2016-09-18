using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using RaceRoomStats.Models;

namespace RaceRoomStats
{
    public partial class MainWindow : Window
    {
        private List<RaceResult> _allRaceResults;
        private FileInfo[] _files;

        public List<string> TrackNames { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            TrackNames = new List<string>();

            ReadDataToMemory();
            PopulateTracks();
        }

        private void PopulateTracks()
        {
            foreach (var track in _allRaceResults.Select(n => n.Track).Distinct().OrderBy(n => n))
            {
                TrackNames.Add(track);
            }

            comboBox_BestLapTimes.ItemsSource = TrackNames;
            comboBox_BestLapTimes.SelectedItem = TrackNames.First();
        }

        private void ReadDataToMemory()
        {
            //1. Get fileinfos
            _files = GetFileInfos();

            //2. Races to memory
            _allRaceResults = ReadAllRacesToMemory(_files);
        }

        private static List<RaceResult> ReadAllRacesToMemory(FileInfo[] files)
        {
            List<RaceResult> allRaceResults = new List<RaceResult>();
            foreach (var file in files)
            {
                string json = File.ReadAllText(file.FullName);
                RaceResult raceResult = JsonConvert.DeserializeObject<RaceResult>(json);
                allRaceResults.Add(raceResult);
            }

            return allRaceResults;
        }

        private static FileInfo[] GetFileInfos()
        {
            FileInfo[] files;
            try
            {
                //var dir = new DirectoryInfo(@"C:\GIT\RaceRoomStats\RaceRoomStats\RaceRoomStats\bin\Debug\races\");

                //release:
                var dir = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\races\");
                files = dir.GetFiles().Where(n => n.Extension == ".json").OrderByDescending(p => p.LastWriteTime).ToArray();
            }
            catch (Exception)
            {
                files = new FileInfo[1];
                MessageBox.Show("No races found in races folder. Press OK to close the app.");
                Environment.Exit(0);
            }

            return files;
        }

        private void button_showBestLapTimes_Click(object sender, RoutedEventArgs e)
        {
            textBox_output.Text = "Loading...";

            var lapTimes = new List<LapTime>();
            //foreach (var session in _allRaceResults
            //    .Where(n => n.Track == comboBox_BestLapTimes.Text)
            //    .SelectMany(n => n.Sessions.Where(a => a.Type == "Race"))
            //    )
            //{
            foreach (var raceResult in _allRaceResults
                .Where(n => n.Track == comboBox_BestLapTimes.Text))
            {
                foreach (var sessions in raceResult.Sessions)
                {
                    foreach (var player in sessions.Players)
                    {
                        foreach (var raceSessionLap in player.RaceSessionLaps.Where(n => n.Time > 0))
                        {
                            lapTimes.Add(new LapTime()
                            {
                                FullName = player.FullName,
                                Car = player.Car,
                                Time = int.Parse(raceSessionLap.Time.ToString()),
                                DateTime = raceResult.Time.ToString("ddd dd MMM HH:mm")
                            });
                        }
                    }
                }
            }

            //4. Display laptimes
            PrintLapTimes(lapTimes);
        }

        private void PrintLapTimes(List<LapTime> lapTimes)
        {
            var sb = new StringBuilder();
            var counter = 0;
            foreach (var lap in lapTimes.OrderBy(n => n.Time))
            {
                //if (counter == 0)
                //    Console.ForegroundColor = ConsoleColor.Green;
                //else
                //    Console.ForegroundColor = ConsoleColor.Gray;
                counter++;
                if (counter == 4)
                    sb.Append(Environment.NewLine);

                var realTime = TimeSpan.FromMilliseconds(lap.Time);
                string answer = $"{realTime.Minutes:D2}.{realTime.Seconds:D2}.{realTime.Milliseconds:D3}";
                string car = $"[{lap.Car}]";
                sb.Append("\t" + answer + " - " + lap.FullName.PadRight(20) + car.PadRight(30) + lap.DateTime + Environment.NewLine);
            }

            textBox_output.Text = sb.ToString();
        }
    }
}
