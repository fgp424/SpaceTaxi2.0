using System;
using System.Collections.Generic;
using DIKUArcade.Graphics;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using SpaceTaxi.StaticObjects;

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

        public EntityContainer<VisualObjects> mapGrafics = new EntityContainer<VisualObjects>();

        private char [] PngChar; 

        

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


            foreach (string a in reader.MapData){
                xValue = 0.0f-(1.0f/40.0f);
                yValue = yValue-(1.0f/23.0f);
                var rand = a;
                Console.WriteLine("");
                foreach(char c in rand){
                    xValue = xValue+(1.0f/40.0f);
                    for (int i = 0; i<PngChar.Length; i++){
                        if (PngChar[i] == c){
                            mapGrafics.AddStationaryEntity(new VisualObjects(
                                new StationaryShape(new Vec2F(xValue, yValue), new Vec2F((1.0f/40.0f), (1.0f/23.0f))), 
                                mapPics[i]));
                        }
                    }
                }
            }

            foreach (string a in reader.MapData){
                playerx = 0.0f-(1.0f/40.0f);
                playery = playery-(1.0f/23.0f);
                var rand = a;
                foreach(char c in rand){
                    playerx = playerx+(1.0f/40.0f);
                    if (c == '>'){
                        Console.WriteLine("Found it");
                        Console.WriteLine(playerx);
                        Console.WriteLine(playery);
                        player = new Player(
                            new DynamicShape(new Vec2F(playerx-.05f, playery-.05f), new Vec2F((.1f), (.1f))), 
                            new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png")));
                        }
                    }
                }
            

            return level;
        }
    }
}