using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using RaceRoomStats.Models;

namespace RaceRoomStats
{
    public partial class MainWindow : Window
    {
        private List<LapTime> _lapTimes;
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
                var dir = new DirectoryInfo(@"C:\GIT\RaceRoomStats\RaceRoomStats\RaceRoomStats\bin\Debug\races\");

                //release:
                //var dir = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\races\");
                files = dir.GetFiles().Where(n => n.Extension == ".json").OrderByDescending(p => p.LastWriteTime).ToArray();
            }
            catch (Exception)
            {
                files = new FileInfo[1];
                MessageBox.Show("No races found in races folder. Press OK to close the app.");
                //Environment.Exit(0);
            }

            return files;
        }
    }
}
