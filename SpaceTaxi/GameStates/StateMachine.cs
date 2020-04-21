using DIKUArcade.EventBus; 
using DIKUArcade.State;
using System;
using System.IO;
using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.Timers;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Input;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Utilities;



namespace SpaceTaxi.GameStates { 
    public class StateMachine : IGameEventProcessor<object> { 
        public IGameState ActiveState { get; private set; }
        private GameEventBus<object> taxiBus;
        public StateMachine() {
            taxiBus = TaxiBus.GetBus();
            taxiBus.Subscribe(GameEventType.GameStateEvent, this); 
            taxiBus.Subscribe(GameEventType.InputEvent, this);
            taxiBus.Subscribe(GameEventType.WindowEvent, this);
            ActiveState = GameRunning.GetInstance();
        }
        public void ProcessEvent(GameEventType eventType,
        GameEvent<object> gameEvent) {
            throw new NotImplementedException();
        }
    }
}