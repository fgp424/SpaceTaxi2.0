
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



namespace SpaceTaxi.GameStates { 
    public class GameRunning : IGameState { 

        private static GameRunning instance = null;

        private Entity backGroundImage;

        private LevelCreator Level;



        public static GameRunning GetInstance() { 
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning()); 
        }

        public void GameLoop(){

        }

        public void InitializeGameState(){
            backGroundImage = new VisualObjects(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );
            Level = new LevelCreator();
            Level.CreateLevel("the-beach.txt");

        }

        public void UpdateGameLogic(){
            
        }

        public void RenderState(){
            backGroundImage.RenderEntity();
            Level.mapGrafics.RenderEntities();
            Level.player.Entity.RenderEntity();

        }

        public void HandleKeyEvent(string keyValue, string keyAction){

        }

    }
}