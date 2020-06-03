using System;
using System.Collections.Generic;
using DIKUArcade.Graphics;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using SpaceTaxi.StaticObjects;
using SpaceTaxi.Enums;
using System.Linq;

namespace SpaceTaxi.LevelLoading {
    public class LevelCreator {

        
        //fields
        float xValue = 0.0f;
        float yValue = 1.0f;
        public Reader reader{get; private set;}
        public Player player;

        public List<Customer> CustomerList = new List<Customer>();

        List<Image> mapPics = new List<Image>();
        List<Image> platformPics = new List<Image>();

        public char [] PngChar{get; private set;} 
        
        private string PlatformString;

        public char[] Platforms{get; private set;}
        private List<string> customerString = new List<string>();

        public Vec2F rand;
        public Vec2F rand1;

/// <summary> Levelcreator method called when instiantiating the level crator </summary>
/// <returns> reader instance </returns>

        public LevelCreator() {
            reader = new Reader();
        }

/// <summary> Method in charge of creating the level </summary>
/// <param name="levelname"> Passes the level name on to the level </param>
/// <returns> updated fields in level and levelreader </returns>

        public Level CreateLevel(string levelname) {
            // Create the Level here
            Level level = new Level(levelname);
            reader.ReadFile(levelname);
            PngChar = reader.pngcharstring.ToCharArray();

            // Gather all the information/objects the levels needs here.
            for (int i = 0; i<PngChar.Length; i++){
                mapPics.Add(new Image(Path.Combine("Assets", "Images", reader.PngData[i])));
            }

            PlatformString = reader.PlatformData.Remove(0, 10);
            PlatformString = PlatformString.Replace(",", "");
            PlatformString = PlatformString.Replace(" ", "");
            Platforms = PlatformString.ToCharArray();
            level.Platforms = Platforms;

            for(int i=0; i< Platforms.Length+1; i++){
                level.speratedplatforms.Add(new EntityContainer<Platform>());
            }


            foreach(string s in reader.CustomerData){
                customerString.Add(s.Remove(0, 10));
            }
            rand = new Vec2F(0.5f,0.5f);
            rand1 = new Vec2F(0.1f,0.1f);
            foreach(string s in customerString){
                string[] temp = s.Split(null);
                level.CustomerList.Add(new Customer(new DynamicShape(new Vec2F(5.0f,5.0f), new Vec2F((0.03f), (0.06f))), 
                            new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png")), temp[0], temp[2], temp[3], Convert.ToDouble(temp[4]), Convert.ToDouble(temp[5]), Convert.ToDouble(temp[1])));
            }



            foreach (char c in Platforms){
                for(int i = 0; i<PngChar.Length; i++)
                    if(c == PngChar[i]){
                        PngChar[i] = Convert.ToChar(0x0);
                        platformPics.Add(mapPics[i]);
                    }
            }

            foreach (string a in reader.MapData){
                xValue = 0.0f-(1.0f/40.0f);
                yValue = yValue-(1.0f/23.0f);
                var rand = a;
                foreach(char c in rand){
                    xValue = xValue+(1.0f/40.0f);
                    
                    for (int i = 0; i<PngChar.Length; i++){
                        if (PngChar[i] == c){
                            level.obstacles.AddStationaryEntity(new VisualObjects(
                                new StationaryShape(new Vec2F(xValue, yValue), new Vec2F((1.0f/40.0f), (1.0f/23.0f))), 
                                mapPics[i]));
                        }
                        if (c == '>'){
                        level.player = new Player(
                            new DynamicShape(new Vec2F(xValue-.05f, yValue-.05f), new Vec2F((.05f), (.05f))), 
                            new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png")),
                            (Orientation)1);
                            level.startpos = new Vec2F(xValue-.05f, yValue-.05f);
                        }
                    }
                    for (int i = 0; i<Platforms.Length; i++){
                        if (Platforms[i] == c){
                            level.speratedplatforms[i].AddStationaryEntity(new Platform(
                                new StationaryShape(new Vec2F(xValue, yValue), new Vec2F((1.0f/40.0f), (1.0f/23.0f))), 
                                platformPics[i]));
                            
                        }
                    }
                    
                    if ('^'== c){
                        level.portal.AddStationaryEntity(new VisualObjects(
                            new StationaryShape(new Vec2F(xValue, yValue), new Vec2F((1.0f/40.0f), (1.0f/23.0f))), 
                            null ));
                    }
                }
            }
            return level;
        }
    }
}