using System;
using System.Collections.Generic;
using DIKUArcade.Graphics;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using SpaceTaxi.StaticObjects;
using SpaceTaxi.Enums;

namespace SpaceTaxi.LevelLoading {
    public class LevelCreator {

        
        // add fields as you see fit
        float xValue = 0.0f;
        float yValue = 1.0f;
        float playerx = 0.0f;
        float playery = 1.0f;
        private Reader reader;
        public Player player;

        List<Image> mapPics = new List<Image>();
        List<Image> platformPics = new List<Image>();

        public EntityContainer<VisualObjects> mapGrafics = new EntityContainer<VisualObjects>();
        
        public EntityContainer<Platform> platforms = new EntityContainer<Platform>();

        private char [] PngChar; 
        
        private string PlatformString;

        private char[] Platforms;

        

        public LevelCreator() {
            reader = new Reader();
        }
        
        public Level CreateLevel(string levelname) {
            // Create the Level here
            Level level = new Level();
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
                            mapGrafics.AddStationaryEntity(new VisualObjects(
                                new StationaryShape(new Vec2F(xValue, yValue), new Vec2F((1.0f/40.0f), (1.0f/23.0f))), 
                                mapPics[i]));
                        }
                        if (c == '>'){
                        player = new Player(
                            new DynamicShape(new Vec2F(xValue-.05f, yValue-.05f), new Vec2F((.1f), (.1f))), 
                            new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png")),
                            (Orientation)1);
                        }
                    }
                    for (int i = 0; i<Platforms.Length; i++){
                        if (Platforms[i] == c){
                            platforms.AddStationaryEntity(new Platform(
                                new StationaryShape(new Vec2F(xValue, yValue), new Vec2F((1.0f/40.0f), (1.0f/23.0f))), 
                                platformPics[i]));
                        }
                    }
                }
            }
            
            return level;
        }
    }
}