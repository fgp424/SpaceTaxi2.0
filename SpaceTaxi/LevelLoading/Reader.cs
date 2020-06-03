using System.IO;
using System.Collections.Generic;
using SpaceTaxi.Utilities;

namespace SpaceTaxi.LevelLoading
{
    public class Reader {
//fields
        public List<string> MapData {get; private set;}
        public List<string> NameData {get; private set;}
        public string PlatformData {get; private set;}
        public List<string> LegendData {get; private set;}
        public List<string> CustomerData {get; private set;}

        public List<string> PngData {get; private set;}

        public string[] lines{get; private set;}

        public string pngcharstring = "";


/// <summary> Reads given file and seperates data in to fields </summary>
/// <param name="filename"> Filename to be read </param>
        public void ReadFile(string filename) {
            // Get each line of file as an entry in array lines.
            string[] lines = File.ReadAllLines(Utils.GetLevelFilePath(filename));


            // Iterate over lines and add data till the corresponding field
            // MapData for saveing txt file of the map, in lines.
            MapData = new List<string>();
            // MetaData for name and amout of platforms
            NameData = new List<string>();
            // LegendData for char and .png ref.
            LegendData = new List<string>();
            // CustomerData for Customer information.
            CustomerData = new List<string>();
            PlatformData = lines[25];
            PngData = new List<string>();


            for (int i = 0 ; i<23 ; i++){
                MapData.Add(lines[i]);
            }

            NameData.Add(lines[24]);

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
