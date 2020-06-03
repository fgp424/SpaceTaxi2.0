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
using SpaceTaxi.GameStates;
using NUnit.Framework;

namespace SpaceTaxiTest {
   [TestFixture]
    public class StateMachineTesting {
        private StateMachine stateMachine;

        [SetUp]
        public void InitiateStateMachine() {
            DIKUArcade.Window.CreateOpenGLContext();


        TaxiBus.GetBus().InitializeEventBus(new List<GameEventType>() { 
            GameEventType.InputEvent, // key press / key release 
            GameEventType.WindowEvent, // messages to the window
            GameEventType.PlayerEvent,
            GameEventType.GameStateEvent,
        });
        
        stateMachine = new StateMachine();

        TaxiBus.GetBus().Subscribe(GameEventType.GameStateEvent, stateMachine);
        
        }

        [Test]
        public void TestInitialState() {
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<MainMenu>());
        }

        [Test]
        public void TestEventGamePaused() {
            TaxiBus.GetBus().RegisterEvent(
            GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.GameStateEvent,
                this,
                "CHANGE_STATE",
                "GAME_PAUSED", ""));
            TaxiBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GamePaused>());
        }

        [Test]
        public void TestEventGameRunning() {
            TaxiBus.GetBus().RegisterEvent(
            GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.GameStateEvent,
                this,
                "CHANGE_STATE",
                "GAME_RUNNING", ""));
            TaxiBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameRunning>());
        }

        [Test]
        public void TestEventGameResume() {
            TaxiBus.GetBus().RegisterEvent(
            GameEventFactory<object>.CreateGameEventForAllProcessors(
                GameEventType.GameStateEvent,
                this,
                "CHANGE_STATE",
                "GAME_RESUME", ""));
            TaxiBus.GetBus().ProcessEventsSequentially();
            Assert.That(stateMachine.ActiveState, Is.InstanceOf<GameRunning>());
        }
    }
}
