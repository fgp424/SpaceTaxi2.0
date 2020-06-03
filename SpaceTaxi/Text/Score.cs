using System;
using DIKUArcade.Graphics;
using DIKUArcade.Math;



namespace SpaceTaxi.qq
{

    public class Score{
        private double score; 
        private double scorefloor;
        private Text display;

        ///<summary> Constructor that creates score instance </summary>
        /// <param name="position"> Defines the position of the score </param>
        /// <param name="extent"> Defines the extention of the score </param>
        public Score(Vec2F position, Vec2F extent) {
            score = 0;
            display = new Text(scorefloor.ToString(), position, extent);
        }

        ///<summary> Method time to timer </summary>
        ///<returns> Updated timer </return>
        public void AddScore(double q) { 
            score = score + q; 
            scorefloor = Math.Floor(score);
        }

        ///<summary> Method renderScore, display the score on screen </summary>
        ///<returns> the rendered score </returns>
        public void RenderScore() { 
            display.SetText(string.Format("Score: {0}", scorefloor.ToString())); 
            display.SetColor(new Vec3I(255, 0, 0)); 
            display.RenderText();
        }
    }

}