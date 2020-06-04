using System;
using System.IO;
using System.Collections.Generic;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using DIKUArcade.State;
using SpaceTaxi.LevelLoading;
using SpaceTaxi.StaticObjects;
using SpaceTaxi.qq;
using System.Linq;
using SpaceTaxi.Enums;



namespace SpaceTaxi.GameStates
{
    public class GameRunning : IGameState { 

//fields
        public List<EntityContainer<Platform>> speratedplatforms = 
            new List<EntityContainer<Platform>>();
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
        private List<Customer> allCustomer;
        private Player player;
        float xValue;
        float yValue;
        float xValue1;
        float yValue1;
        string PlatformsString;
        char[] Platforms;
        Vec2F startpos1;
        Vec2F startpos2;

        
/// <summary> Constructor that creates game running instance if not already exisiting</summary>

        public static GameRunning GetInstance() { 
            return GameRunning.instance ?? (GameRunning.instance = new GameRunning()); 
        }

        public void GameLoop(){

        }

/// <summary> Method that initializes game state</summary>
        public void InitializeGameState(){
            Gamenotready = true;
            Gamenotreadytext1 = new Text("Press space to",new Vec2F(0.35f, 0.3f), 
                new Vec2F(0.3f, 0.3f));
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

            allCustomer = new List<Customer>();

            foreach (Customer c in Level1.CustomerList){
                allCustomer.Add(c);
            }
            foreach (Customer c in Level2.CustomerList){
                allCustomer.Add(c);
            }

            foreach(char c in Level1.Platforms){
                PlatformsString += c;
            }
            foreach(char c in Level2.Platforms){
                PlatformsString += c;
            }
            Platforms = PlatformsString.ToCharArray();

            foreach (EntityContainer<Platform> c in Level1.speratedplatforms){
                speratedplatforms.Add(c);
            }
            foreach (EntityContainer<Platform> c in Level2.speratedplatforms){
                speratedplatforms.Add(c);
            }
            xValue = 0.0f;
            yValue = 1.0f;
            xValue1 = 0.0f;
            yValue1 = 1.0f;


            foreach (string a in LevelCreator1.reader.MapData){
                xValue = 0.0f-(1.0f/40.0f);
                yValue = yValue-(1.0f/23.0f);
                var rand = a;
                foreach(char c in rand){
                    xValue = xValue+(1.0f/40.0f);
                    for (int i = 0; i<LevelCreator1.PngChar.Length; i++){
                        if (c == '>'){
                        player = new Player(
                            new DynamicShape(new Vec2F(xValue-.05f, yValue-.05f), 
                            new Vec2F((.05f), (.05f))), 
                            new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png")),
                            (Orientation)1);
                            startpos1 = new Vec2F(xValue-.05f, yValue-.05f);
                        }
                    }
                }
            }

            foreach (string a in LevelCreator2.reader.MapData){
                xValue1 = 0.0f-(1.0f/40.0f);
                yValue1 = yValue1-(1.0f/23.0f);
                var rand = a;
                foreach(char c in rand){
                    xValue1 = xValue1+(1.0f/40.0f);
                    for (int i = 0; i<LevelCreator2.PngChar.Length; i++){
                        if (c == '>'){
                            startpos2 = new Vec2F(xValue1-.05f, yValue1-.05f);
                        }
                    }
                }
            }
        }
/// <summary> Method that collects what is to be updated in the Game class</summary>
        public void UpdateGameLogic(){
            if (!Gamenotready){
                ActiveLevel.UpdateLevelLogic();
                Collision();
                CustomerSpawned();
                AddScore();
                timer.AddTime();
                player.Move();
                player.GraficUpdate();
                player.Gravity();
                foreach(Customer c in allCustomer){
                    c.IsDroppedOffMove();
                }
                foreach(Customer c in ActiveLevel.CustomerList){
                    c.Timer();
                    if (!player.hasCustomer){
                        if (MathF.Round(c.Shape.Position.Y, 3) == MathF.Round
                            (player.Entity.Shape.Position.Y, 3) && !c.isDroppedOff){
                                c.Move(player.Entity.Shape.Position.X);
                        }
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
                player.Entity.RenderEntity();   
                foreach(Customer c in allCustomer){
                        c.RenderEntity();
                }
                if (Gamenotready){
                    Gamenotreadytext1.RenderText();
                    Gamenotreadytext2.RenderText();
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


/// <summary>
/// Method that adds score and drops off customers
/// </summary>
        public void AddScore(){
            foreach(Customer c in allCustomer){
                if(c.isDroppedOff){
                    if(!c.dropOffAny){
                        char[] tempq = c.destinationplatform.ToCharArray();
                        char tempcq = tempq[0];
                        var tempindex = Platforms.ToList().FindIndex( q => q == tempcq);
                        var tempcontainer = speratedplatforms[tempindex].GetEnumerator();
                        tempcontainer.MoveNext();
                        Platform qqqqq = (Platform) tempcontainer.Current;

                        
                        if(!c.scoreCounted){
                            score.AddScore(c.points);
                            c.CountScore();
                        }
                        if(c.Shape.Position.X <= qqqqq.Shape.Position.X){
                            c.despawn();
                            c.dropOffReset();
                            player.DroppedOff();
                        }
                    } else if (c.dropOffAny){
                        if(!c.scoreCounted){
                            score.AddScore(c.points);
                            c.CountScore();
                        }
                    }                           
                }
            }
        }


/// <summary>
/// Customer method for spawn logic
/// </summary>
        public void CustomerSpawned(){
            foreach(Customer c in allCustomer){
                if (c.isSpawned){
                    if(!c.HasSpawned){
                        char[] temp = c.currentplatform.ToCharArray();
                        char tempc = temp[0];
                        var tempindex = Platforms.ToList().FindIndex( q => q == tempc);
                        var tempcontainer = speratedplatforms[tempindex].GetEnumerator();
                        tempcontainer.MoveNext();
                        tempcontainer.MoveNext();
                        tempcontainer.MoveNext();
                        tempcontainer.MoveNext();
                        Platform e = (Platform) tempcontainer.Current;
                        c.Spawned();
                        c.Spawn(e.Entity.Shape.Position + (new Vec2F(0.0f, (1f/23f))));
                    }
                }
            }
        }
/// <summary>
/// Method that tracks collision between the player object and given objects on the map 
///</summary>
        public void Collision(){

//Collision detection on customers
            foreach (Customer c in allCustomer){
                if (DIKUArcade.Physics.CollisionDetection.Aabb(c.Shape.AsDynamicShape(), 
                    player.Entity.Shape).Collision){ 
                        c.PickedUp();
                        player.PickUp(c.name);
                }
            }
//Collision detection on obstacles
            foreach (Entity o in ActiveLevel.obstacles) {
                    if (DIKUArcade.Physics.CollisionDetection.Aabb(player.Entity.Shape.
                        AsDynamicShape(), o.Shape).Collision){
                            player.Entity.DeleteEntity();
                            AddExplosion(player.Entity.Shape.Position.X-0.05f, 
                                player.Entity.Shape.Position.Y-0.05f,0.2f,0.2f);
                            player.Entity.Shape.Position = new Vec2F(5.0f,5.0f);
                    }
            }



//Collisision detction on platforms
            foreach (EntityContainer<Platform> e in ActiveLevel.speratedplatforms) {
                foreach (Platform p in e){
                    if (DIKUArcade.Physics.CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), 
                        p.Shape).Collision){

//Customer drop off logic
                        foreach(Customer c in allCustomer){
                            char[] tempcharlist = c.destinationplatform.ToCharArray();
                            char tempc = tempcharlist[0];

                            char[] temp = c.currentplatform.ToCharArray();
                            char tempcc = temp[0];

                            if(c.pickedUp){
                                if(!c.dropOffAny){                
                                    var tempindex = Platforms.ToList().FindIndex( q => q == tempc);
                                    var tempcontainer = speratedplatforms[tempindex].GetEnumerator();
                                    tempcontainer.MoveNext();
                                    Platform qqqqq = (Platform) tempcontainer.Current;
                                    if((!c.isDroppedOff && Platforms[speratedplatforms.IndexOf(e)] == tempc)){                                   
                                        c.Spawn(player.Entity.Shape.Position + new Vec2F(0.05f, 0.0f));
                                        c.dropOff();
                                        c.EdgeOfDestination(qqqqq.Shape.Position.X - 0.1f);
                                    }
                                } else if(c.dropOffAny){
                                    foreach (EntityContainer<Platform> q in ActiveLevel.speratedplatforms) {
                                        foreach (Platform qq in q){
                                            if ((DIKUArcade.Physics.CollisionDetection.Aabb(player.
                                                Entity.Shape.AsDynamicShape(), qq.Shape).Collision) 
                                                && ActiveLevel.Platforms[ActiveLevel.speratedplatforms.
                                                IndexOf(q)] != tempcc){
                                                    var tempq = ActiveLevel.speratedplatforms.IndexOf(q);
                                                    var tempqq = ActiveLevel.speratedplatforms[tempq].
                                                        GetEnumerator();
                                                    tempqq.MoveNext();
                                                    Platform qqq = (Platform) tempqq.Current;
                                                    c.Spawn(player.Entity.Shape.Position + 
                                                        new Vec2F(0.05f, 0.0f));
                                                    c.dropOff();
                                                    c.EdgeOfDestination(qqq.Shape.Position.X - 0.1f);
                                            }
                                        }
                                    }
                                }    
                            }
                                if (c.dropOffAny){
                                    var tempq = ActiveLevel.speratedplatforms.IndexOf(e);
                                    var tempqq = ActiveLevel.speratedplatforms[tempq].GetEnumerator();
                                    tempqq.MoveNext();
                                    Platform qqq = (Platform) tempqq.Current;
                                        if(c.Shape.Position.X <= qqq.Shape.Position.X){
                                            c.despawn();
                                            c.dropOffReset();
                                            player.DroppedOff();
                                        }
                                }


                            }
                    }
                }
            }



//Collisision detction on platforms
            foreach (EntityContainer<Platform> e in ActiveLevel.speratedplatforms) {
                foreach (Platform p in e){
                    if (DIKUArcade.Physics.CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), 
                        p.Shape).Collision){
                            if(player.Physics.Y > -0.004f){
                                player.Physics.Y = 0.0f;
                                player.Physics.X = 0.0f;

//Player death                            
                            } else {
                                player.Entity.DeleteEntity();
                                AddExplosion(player.Entity.Shape.Position.X-0.05f, 
                                    player.Entity.Shape.Position.Y-0.05f,0.2f,0.2f);
                                player.Entity.Shape.Position = new Vec2F(5.0f,5.0f);
                            }
                    }
                }
            }

//Collision detection on portal
            foreach (Entity p in ActiveLevel.portal) {
                    if (DIKUArcade.Physics.CollisionDetection.Aabb(player.Entity.Shape.AsDynamicShape(), 
                        p.Shape).Collision){
                            if (ActiveLevel == Level1){
                                player.Entity.Shape.Position = startpos2;
                                player.Physics = new Vec2F (0.0f, 0.0f);
                                ActiveLevel = Level2;
                            } else if (ActiveLevel == Level2){
                                player.Entity.Shape.Position = startpos1;
                                player.Physics = new Vec2F (0.0f, 0.0f);
                                ActiveLevel = Level1;
                            }
                    }
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

        
    }
}
