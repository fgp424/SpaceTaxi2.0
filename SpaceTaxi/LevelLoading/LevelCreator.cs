using System;
using System.Collections.Generic;
using DIKUArcade.Graphics;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using SpaceTaxi.visObjects;

namespace SpaceTaxi.LevelLoading {
    public class LevelCreator {

        
        // add fields as you see fit
        float xValue = 0.0f;
        float yValue = 1.0f;
        private Reader reader;

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
                foreach(char c in rand){
                    xValue = xValue+(1.0f/40.0f);
                    Console.Write(reader.pngcharstring.Length);
                    for (int i = 0; i<PngChar.Length; i++){
                        Console.WriteLine(PngChar[i]);
                        if (PngChar[i] == c){
                            mapGrafics.AddStationaryEntity(new VisualObjects(
                                new StationaryShape(new Vec2F(xValue, yValue), new Vec2F((1.0f/40.0f), (1.0f/23.0f))), 
                                mapPics[i]));
                        }
                    }
                }
            }

            return level;
        }
    }
}