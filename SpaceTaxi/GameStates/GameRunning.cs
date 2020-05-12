
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

//fields
        private static GameRunning instance = null;

        private Entity backGroundImage;
        private Level ActiveLevel;
        private Level Level1;
        private Level Level2;
        private AnimationContainer explosions;
        private List<Image> explosionStrides;
        private int explosionLength = 500;
        private LevelCreator LevelCreator1;

        private LevelCreator LevelCreator2;

        
/// <summary> Constructor that creates game running instance if not already exisiting</summary>

        public static GameRunning GetInstance() { 
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning()); 
        }

        public void GameLoop(){

        }

/// <summary> Method that initializes game state</summary>
        public void InitializeGameState(){
            backGroundImage = new VisualObjects(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );

            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(10);

            LevelCreator1 = new LevelCreator();
            LevelCreator2 = new LevelCreator();

            Level1 = LevelCreator1.CreateLevel("short-n-sweet.txt");
            Level2 = LevelCreator2.CreateLevel("the-beach.txt");
            ActiveLevel = Level1;
        }
/// <summary> Method that collects what is to be updated in the Game class</summary>
        public void UpdateGameLogic(){
            ActiveLevel.UpdateLevelLogic();
            Collision();
        }
/// <summary> Method that collects what is to be rendered in the game class</summary>
        public void RenderState(){
            backGroundImage.RenderEntity();
            ActiveLevel.RenderLevelObjects();
            explosions.RenderAnimations();
        }

        public void HandleKeyEvent(string keyValue, string keyAction){

        }
    /// <summary> Method that creates an explosion animation </summary>
    /// <returns> Updated animation container explosions </returns>
        public void AddExplosion(float posX, float posY,
            float extentX, float extentY) {
            explosions.AddAnimation( 
                new StationaryShape(posX, posY, extentX, extentY), explosionLength, 
                new ImageStride(explosionLength / 8, explosionStrides));
        }

    /// <summary> Method that tracks collision between the player object and given objects on the map </summary>
        public void Collision(){
            foreach (Entity o in ActiveLevel.obstacles) {
                    if (DIKUArcade.Physics.CollisionDetection.Aabb(ActiveLevel.player.Entity.Shape.AsDynamicShape(), o.Shape).Collision){
                        ActiveLevel.player.Entity.DeleteEntity();
                        AddExplosion(ActiveLevel.player.Entity.Shape.Position.X-0.05f, ActiveLevel.player.Entity.Shape.Position.Y-0.05f,0.2f,0.2f);
                        ActiveLevel.player.Entity.Shape.Position = new Vec2F(5.0f,5.0f);
                    }
            }

            foreach (Entity e in ActiveLevel.platforms) {
                    if (DIKUArcade.Physics.CollisionDetection.Aabb(ActiveLevel.player.Entity.Shape.AsDynamicShape(), e.Shape).Collision){
                        if(ActiveLevel.player.Physics.Y > -0.004f){
                            ActiveLevel.player.Physics.Y = 0.0f;
                            ActiveLevel.player.Physics.X = 0.0f;
                        }
                        else{
                            ActiveLevel.player.Entity.DeleteEntity();
                            AddExplosion(ActiveLevel.player.Entity.Shape.Position.X-0.05f, ActiveLevel.player.Entity.Shape.Position.Y-0.05f,0.2f,0.2f);
                            ActiveLevel.player.Entity.Shape.Position = new Vec2F(5.0f,5.0f);
                        }
                    }
            }
            foreach (Entity p in ActiveLevel.portal) {
                    if (DIKUArcade.Physics.CollisionDetection.Aabb(ActiveLevel.player.Entity.Shape.AsDynamicShape(), p.Shape).Collision){
                        if (ActiveLevel == Level1){
                            Level2.player.Entity.Shape.Position = Level2.startpos;
                            Level2.player.Physics = new Vec2F (0.0f, 0.0f);
                            ActiveLevel = Level2;
                        } else if (ActiveLevel == Level2){
                            Level1.player.Entity.Shape.Position = Level1.startpos;
                            Level1.player.Physics = new Vec2F (0.0f, 0.0f);
                            ActiveLevel = Level1;
                        }
                    }
            }

        }
    }
}