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
using SpaceTaxi.qq;
using System.Linq;



namespace SpaceTaxi.GameStates { 
    public class GameRunning : IGameState { 

//fields
        private static GameRunning instance = null;
        private GameEventBus<object> taxiBus;
        private Entity backGroundImage;
        private Level ActiveLevel;
        private Level Level1;
        private Level Level2;
        private AnimationContainer explosions;
        private List<Image> explosionStrides;
        private int explosionLength = 500;
        private LevelCreator LevelCreator1;

        private LevelCreator LevelCreator2;
        private bool Gamenotready;
        private Text Gamenotreadytext1;
        private Text Gamenotreadytext2;
        private Timer timer;
        private Score score;
        private List<Level> levels = new List<Level>();

        
/// <summary> Constructor that creates game running instance if not already exisiting</summary>

        public static GameRunning GetInstance() { 
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning()); 
        }

        public void GameLoop(){

        }

/// <summary> Method that initializes game state</summary>
        public void InitializeGameState(){
            Gamenotready = true;
            Gamenotreadytext1 = new Text("Press space to",new Vec2F(0.35f, 0.3f), new Vec2F(0.3f, 0.3f));
            Gamenotreadytext1.SetColor(new Vec3I(0, 255, 0));

            Gamenotreadytext2 = new Text("start",new Vec2F(0.45f, 0.25f), new Vec2F(0.3f, 0.3f));
            Gamenotreadytext2.SetColor(new Vec3I(0, 255, 0));

            taxiBus = TaxiBus.GetBus();

            backGroundImage = new VisualObjects(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1.0f, 1.0f)),
                new Image(Path.Combine("Assets", "Images", "SpaceBackground.png"))
            );

            explosionStrides = ImageStride.CreateStrides(8,
                Path.Combine("Assets", "Images", "Explosion.png"));
            explosions = new AnimationContainer(10);

            score = new Score(new Vec2F(0.7972f, -0.1f), new Vec2F(0.3f, 0.3f));
            timer = new Timer(new Vec2F(0.8f, -0.18f), new Vec2F(0.3f, 0.3f));

            LevelCreator1 = new LevelCreator();
            LevelCreator2 = new LevelCreator();


            Level1 = LevelCreator1.CreateLevel("short-n-sweet.txt");
            Level2 = LevelCreator2.CreateLevel("the-beach.txt");
            ActiveLevel = Level1;

            levels.Add(Level1);
            levels.Add(Level2);

        }
/// <summary> Method that collects what is to be updated in the Game class</summary>
        public void UpdateGameLogic(){
            if (!Gamenotready){
                ActiveLevel.UpdateLevelLogic();
                Collision();
                CustomerSpawned();
                timer.AddTime();
                foreach(Customer c in ActiveLevel.CustomerList){
                    c.Timer();
                    if (MathF.Round(c.Shape.Position.Y, 3) == MathF.Round(ActiveLevel.player.Entity.Shape.Position.Y, 3)){
                        c.Move(ActiveLevel.player.Entity.Shape.Position.X);
                    } else {
                        c.StopMove();
                    }
                }           
            }
        }
/// <summary> Method that collects what is to be rendered in the game class</summary>
        public void RenderState(){
                backGroundImage.RenderEntity();
                ActiveLevel.RenderLevelObjects();
                explosions.RenderAnimations();
                score.RenderScore();
                timer.RenderTimer();
                foreach(Customer c in ActiveLevel.CustomerList){
                    c.RenderEntity();
                }
            if (Gamenotready == true ){
                Gamenotreadytext1.RenderText();
                Gamenotreadytext2.RenderText();
            }
        }




/// <summary> Method that handles key events in the main menu </summary>
/// <param name="keyValue"> What value of input key </param>
/// <param name="keyAction"> What action of inputkey</param>
        public void HandleKeyEvent(string keyValue, string keyAction){
            if (keyAction == "KEY_PRESS") {
                switch(keyValue) {
                case "KEY_ESCAPE":
                    TaxiBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, this, "CHANGE_STATE", "GAME_PAUSED", ""));
                    break;
                case "KEY_UP":
                    taxiBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_UPWARDS", "", ""));
                    break;
                case "KEY_LEFT":
                    taxiBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_TO_LEFT", "", ""));
                    break;
                case "KEY_RIGHT":
                    taxiBus.RegisterEvent(
                        GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.PlayerEvent, this, "BOOSTER_TO_RIGHT", "", ""));
                    break;
                case "KEY_SPACE":
                    Gamenotready = false;
                    break;
                }
            } else if (keyAction == "KEY_RELEASE"){ 
                switch(keyValue) {
                    case "KEY_LEFT":
                        taxiBus.RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.PlayerEvent, this, "STOP_ACCELERATE_LEFT", "", ""));
                        break;
                    case "KEY_RIGHT":
                        taxiBus.RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.PlayerEvent, this, "STOP_ACCELERATE_RIGHT", "", ""));
                        break;
                    case "KEY_UP":
                        taxiBus.RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                                GameEventType.PlayerEvent, this, "STOP_ACCELERATE_UP", "", ""));
                        break;
                }
            } else {
                throw new System.ArgumentException("KeyState unknown");
            }
        }

    /// <summary> Method that creates an explosion animation </summary>
    /// <returns> Updated animation container explosions </returns>
        public void AddExplosion(float posX, float posY,
            float extentX, float extentY) {
            explosions.AddAnimation( 
                new StationaryShape(posX, posY, extentX, extentY), explosionLength, 
                new ImageStride(explosionLength / 8, explosionStrides));
        }




        public void CustomerSpawned(){
            foreach(Customer c in ActiveLevel.CustomerList){
                if (c.isSpawned){
                    if(!c.HasSpawned){
                        char[] temp = c.currentplatform.ToCharArray();
                        char tempc = temp[0];
                        var tempindex = ActiveLevel.Platforms.ToList().FindIndex( q => q == tempc);
                        var tempcontainer = ActiveLevel.speratedplatforms[tempindex].GetEnumerator();
                        tempcontainer.MoveNext();
                        tempcontainer.MoveNext();
                        tempcontainer.MoveNext();
                        Platform e = (Platform) tempcontainer.Current;
                        c.Spawned();
                        c.Spawn(e.Entity.Shape.Position + (new Vec2F(0.0f, (1f/23f))));
                        Console.WriteLine(e.Entity.Shape.Position);
                    }
                }
            }
        }
    /// <summary> Method that tracks collision between the player object and given objects on the map </summary>
        public void Collision(){

            foreach (Customer c in ActiveLevel.CustomerList){
                              
                if (DIKUArcade.Physics.CollisionDetection.Aabb(ActiveLevel.player.Entity.Shape.AsDynamicShape(), c.Shape).Collision){ 
                    Console.Write("qq");
                }
            }

            foreach (Entity o in ActiveLevel.obstacles) {
                    if (DIKUArcade.Physics.CollisionDetection.Aabb(ActiveLevel.player.Entity.Shape.AsDynamicShape(), o.Shape).Collision){
                        ActiveLevel.player.Entity.DeleteEntity();
                        AddExplosion(ActiveLevel.player.Entity.Shape.Position.X-0.05f, ActiveLevel.player.Entity.Shape.Position.Y-0.05f,0.2f,0.2f);
                        ActiveLevel.player.Entity.Shape.Position = new Vec2F(5.0f,5.0f);
                    }
            }

            foreach (EntityContainer<Platform> e in ActiveLevel.speratedplatforms) {
                foreach (Platform p in e){
                    if (DIKUArcade.Physics.CollisionDetection.Aabb(ActiveLevel.player.Entity.Shape.AsDynamicShape(), p.Shape).Collision){
                        if(ActiveLevel.player.Physics.Y > -0.004f){
                            ActiveLevel.player.Physics.Y = 0.0f;
                            ActiveLevel.player.Physics.X = 0.0f;

                            //Console.WriteLine(ActiveLevel.Platforms[ActiveLevel.speratedplatforms.IndexOf(e)]);
                            foreach (Customer c in ActiveLevel.CustomerList){
                                char[] tempcharlist = c.currentplatform.ToCharArray();
                                char tempc = tempcharlist[0];
                                if(c.isSpawned){
                                    if (tempc == ActiveLevel.Platforms[ActiveLevel.speratedplatforms.IndexOf(e)]){
                                    }
                                } 
                            }
                            
                        } else{
                            ActiveLevel.player.Entity.DeleteEntity();
                            AddExplosion(ActiveLevel.player.Entity.Shape.Position.X-0.05f, ActiveLevel.player.Entity.Shape.Position.Y-0.05f,0.2f,0.2f);
                            ActiveLevel.player.Entity.Shape.Position = new Vec2F(5.0f,5.0f);
                        }
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