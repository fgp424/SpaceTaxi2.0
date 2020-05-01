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
using SpaceTaxi.Enums;



namespace SpaceTaxi.GameStates { 
    public class StateMachine : IGameEventProcessor<object> { 
        public IGameState ActiveState { get; private set; }
        private GameEventBus<object> taxiBus;
        public StateMachine() {
            taxiBus = TaxiBus.GetBus();
            taxiBus.Subscribe(GameEventType.GameStateEvent, this); 
            taxiBus.Subscribe(GameEventType.InputEvent, this);
            taxiBus.Subscribe(GameEventType.WindowEvent, this);
            ActiveState = MainMenu.GetInstance();
        }

        public void SwitchState(GameStateType stateType) { 
            switch (stateType) {
                case GameStateType.GameRunning:
                ActiveState = GameRunning.GetInstance();
                ActiveState.InitializeGameState();
                break;
            case GameStateType.GamePaused:
                //ActiveState = GamePaused.GetInstance();
                ActiveState.InitializeGameState();
                break;
            case GameStateType.MainMenu:
                ActiveState = MainMenu.GetInstance();
                ActiveState.InitializeGameState();
                break;
            case GameStateType.GameResume:
                ActiveState = GameRunning.GetInstance();
                break;
            default:
                break;        
            }
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.GameStateEvent) {
                    SwitchState(StateTransformer.TransformStringToState(gameEvent.Parameter1));
                }
        }
    }
}