using System.Collections.Generic;
using DIKUArcade.Graphics;
using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Math;

namespace SpaceTaxi.LevelLoading {
    public class LevelCreator {

        
        // add fields as you see fit
        float xValue = 0.0f;
        float yValue = 1.0f+(1.0f/23.0f);
        private Reader reader;

        List<Image> mapPics = new List<Image>();

        public EntityContainer<Entity> mapGrafics;

        private char [] PngChar; 

        

        public LevelCreator() {
            reader = new Reader();
            PngChar = reader.pngcharstring.ToCharArray();
        }
        
        public Level CreateLevel(string levelname) {
            // Create the Level here
            Level level = new Level();
            reader.ReadFile(levelname);
            // Gather all the information/objects the levels needs here.
            for (int i = 0; i<PngChar.Length; i++){
                mapPics.Add(new Image(Path.Combine("Assets", "Images", reader.PngData[i])));
            }
            foreach (string a in reader.MapData){
                xValue = 0.0f;
                yValue = yValue-(1.0f/23.0f);
                foreach(char c in a){
                    xValue = xValue+(1.0f/40.0f);
                    for (int i = 0; i<PngChar.Length; i++){
                        if (PngChar[i] == c){
                            mapGrafics.AddStationaryEntity(new Entity(
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