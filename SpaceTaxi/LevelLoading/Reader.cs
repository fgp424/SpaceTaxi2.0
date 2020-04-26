using System;
using System.IO;
using System.Collections.Generic;
using SpaceTaxi.Utilities;
using System.Linq;

namespace SpaceTaxi.LevelLoading {
    public class Reader {
        public List<string> MapData {get; private set;}
        public List<string> MetaData {get; private set;}
        public List<string> LegendData {get; private set;}
        public List<string> CustomerData {get; private set;}

        public List<string> PngData {get; private set;}

        public string pngcharstring = "";

        public void ReadFile(string filename) {
            // Get each line of file as an entry in array lines.
            string[] lines = File.ReadAllLines(Utils.GetLevelFilePath(filename));


            // Iterate over lines and add data till the corresponding field
            // MapData for saveing txt file of the map, in lines.
            MapData = new List<string>();
            // MetaData for name and amout of platforms
            MetaData = new List<string>();
            // LegendData for char and .png ref.
            LegendData = new List<string>();
            // CustomerData for Customer information.
            CustomerData = new List<string>();
            PngData = new List<string>();


            for (int i = 0 ; i<23 ; i++){
                MapData.Add(lines[i]);
            }

            for (int i = 24 ; i<26 ; i++){
                MetaData.Add(lines[i]);
            }

            for(int i = 0; i<lines.Length; i++){
               if(lines[i].Contains("png")==true){ 
                   LegendData.Add(lines[i]);
                }
            }


            for(int i = 0; i<lines.Length; i++){
               if(lines[i].Contains("Customer")==true){ 
                   CustomerData.Add(lines[i]);
                }
            }
            


            foreach(var a in LegendData){
                pngcharstring += a.Substring(0,1);
                PngData.Add(a.Substring(3, a.Length-3));
            }

        }
    }
}
