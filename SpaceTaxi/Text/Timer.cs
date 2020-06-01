using System;
using System.IO;
using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Timers;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Input;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.State;
using DIKUArcade.Utilities;
using SpaceTaxi.LevelLoading;
using SpaceTaxi.StaticObjects;



namespace SpaceTaxi.qq {

    public class Timer{
        private double time; 
        public double timefloor{get; private set;}
        private Text display;

        ///<summary> Constructor that creates timer instance </summary>
        /// <param name="position"> Defines the position of the timer </param>
        /// <param name="extent"> Defines the extention of the timer </param>
        public Timer(Vec2F position, Vec2F extent) {
            time = 0;
            display = new Text(timefloor.ToString(), position, extent);
        }

        ///<summary> Method time to timer </summary>
        ///<returns> Updated timer </return>
        public void AddTime() { 
            time = time + (1.0f/60.0f); 
            timefloor = Math.Floor(time);
        }

        ///<summary> Method RenderTimer, display the timer on screen </summary>
        ///<returns> the rendered time </returns>
        public void RenderTimer() { 
            display.SetText(string.Format("Timer: {0}", timefloor.ToString())); 
            display.SetColor(new Vec3I(255, 0, 0)); 
            display.RenderText();
        }
    }

}