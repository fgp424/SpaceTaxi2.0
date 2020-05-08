using NUnit.Framework;
using System.IO;
using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Input;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using DIKUArcade.State;
using DIKUArcade.Utilities;
using SpaceTaxi;
using SpaceTaxi.Enums;
using SpaceTaxi.StaticObjects;

namespace UnitTests
{
    public class PlayerTest
    {
        
        public Player Player;

        [SetUp]
        public void Setup()
        {
            DIKUArcade.Window.CreateOpenGLContext();
            Player = new Player (new DynamicShape(new Vec2F(0.0f, 0.0f), new Vec2F((.1f), (.1f))), 
                                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png")),
                                (Orientation)1);
                
        }


        [Test]
        public void PlayerMoveUpTest()
        {
            Player.ProcessEvent(GameEventType.PlayerEvent, 
                GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.PlayerEvent, this, "BOOSTER_UPWARDS", "", ""));
            Player.Move();
            Vec2F Expected = new Vec2F(0.0F,0.0001f);
            Assert.AreEqual(Expected.X, Player.Entity.Shape.Position.X);
            Assert.AreEqual(Expected.Y, Player.Entity.Shape.Position.Y);
        }

        [Test]
        public void PlayerMoveLeftTest()
        {
            Player.ProcessEvent(GameEventType.PlayerEvent, 
                GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.PlayerEvent, this, "BOOSTER_TO_LEFT", "", ""));
            Player.Move();
            Vec2F Expected = new Vec2F(-0.0001F,0.0f);
            Assert.AreEqual(Expected.X, Player.Entity.Shape.Position.X);
            Assert.AreEqual(Expected.Y, Player.Entity.Shape.Position.Y);
        }

        [Test]
        public void PlayerMoveRightTest()
        {
            Player.ProcessEvent(GameEventType.PlayerEvent, 
                GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.PlayerEvent, this, "BOOSTER_TO_RIGHT", "", ""));
            Player.Move();
            Vec2F Expected = new Vec2F(0.0001F,0.0f);
            Assert.AreEqual(Expected.X, Player.Entity.Shape.Position.X);
            Assert.AreEqual(Expected.Y, Player.Entity.Shape.Position.Y);
        }

        [Test]
        public void PlayerMoveLeftUpTest()
        {
            Player.ProcessEvent(GameEventType.PlayerEvent, 
                GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.PlayerEvent, this, "BOOSTER_TO_LEFT", "", ""));
            Player.ProcessEvent(GameEventType.PlayerEvent, 
                GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.PlayerEvent, this, "BOOSTER_UPWARDS", "", ""));
            Player.Move();
            Vec2F Expected = new Vec2F(-0.0001F,0.0001f);
            Assert.AreEqual(Expected.X, Player.Entity.Shape.Position.X);
            Assert.AreEqual(Expected.Y, Player.Entity.Shape.Position.Y);
        }

        [Test]
        public void PlayerMoveRightUpTest()
        {
            Player.ProcessEvent(GameEventType.PlayerEvent, 
                GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.PlayerEvent, this, "BOOSTER_TO_RIGHT", "", ""));
            Player.Move();
            Orientation Right = (SpaceTaxi.Enums.Orientation)1;
            Assert.AreEqual(Right, Player.Orientation);
            }

        [Test]
        public void PlayerOrientationLeftTest()
        {
            Player.ProcessEvent(GameEventType.PlayerEvent, 
                GameEventFactory<object>.CreateGameEventForAllProcessors(GameEventType.PlayerEvent, this, "BOOSTER_TO_LEFT", "", ""));
            Player.Move();
            Orientation Left = (SpaceTaxi.Enums.Orientation)0;
            Assert.AreEqual(Left, Player.Orientation);
        }
    }
}