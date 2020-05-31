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

        List<Image> mapPics = new List<Image>();
        List<Image> platformPics = new List<Image>();

        public EntityContainer<VisualObjects> mapGrafics = new EntityContainer<VisualObjects>();
        
        public EntityContainer<Platform> platforms = new EntityContainer<Platform>();

        public char [] PngChar{get; private set;} 
        
        private string PlatformString;

        public char[] Platforms{get; private set;}

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
                            new DynamicShape(new Vec2F(xValue-.05f, yValue-.05f), new Vec2F((.1f), (.1f))), 
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