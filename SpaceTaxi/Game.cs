using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Timers;
using SpaceTaxi.GameStates;
using SpaceTaxi.Enums;

namespace SpaceTaxi
{
    public class Game : IGameEventProcessor<object> {
        private GameEventBus<object> taxiBus;
        private GameTimer gameTimer;
        public Window win {get; private set;} 
        private StateMachine stateMachine;

        public Game() {
            // window
            win = new Window("Space Taxi Game v0.1", 500, AspectRatio.R16X9);

            stateMachine = new StateMachine();

            // event bus
            taxiBus = TaxiBus.GetBus();
            taxiBus.InitializeEventBus(new List<GameEventType> {
                GameEventType.InputEvent, // key press / key release
                GameEventType.WindowEvent, // messages to the window, e.g. CloseWindow()
                GameEventType.PlayerEvent, // commands issued to the player object, e.g. move,d
                GameEventType.GameStateEvent
            });

            stateMachine.SwitchState(GameStateType.MainMenu);

            win.RegisterEventBus(taxiBus);

            // game timer
            gameTimer = new GameTimer(60); // 60 UPS, no FPS limit

            // game assets

            // event delegation
            taxiBus.Subscribe(GameEventType.InputEvent, this);
            taxiBus.Subscribe(GameEventType.WindowEvent, this);
            taxiBus.Subscribe(GameEventType.GameStateEvent, stateMachine); 
        }

/// <summary> Method that creates game loop e.g. game updates </summary>

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();

                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents();
                    taxiBus.ProcessEvents();
                    stateMachine.ActiveState.UpdateGameLogic();
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();

                    stateMachine.ActiveState.RenderState(); 

                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps from the timer
                    win.Title = "Space Taxi | UPS: " + gameTimer.CapturedUpdates + ", FPS: " +
                                 gameTimer.CapturedFrames;
                }
            }
        }


/// <summary> Method that checks for keypress </summary>
/// <returns> Returns value compared to keypress </returns>

        public void KeyPress(string key) {
            stateMachine.ActiveState.HandleKeyEvent(key, "KEY_PRESS");

        }


/// <summary> Method that checks for keyrelease </summary>
/// <returns> Returns value compared to keyrelease </returns>
        public void KeyRelease(string key) {
            stateMachine.ActiveState.HandleKeyEvent(key, "KEY_RELEASE");

        }

/// <summary> Method for eventbus </summary>    
        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            if (eventType == GameEventType.WindowEvent) {
                switch (gameEvent.Message) {
                case "CLOSE_WINDOW":
                    win.CloseWindow();
                    break;
                }
            } else if (eventType == GameEventType.InputEvent) {
                switch (gameEvent.Parameter1) {
                case "KEY_PRESS":
                    KeyPress(gameEvent.Message);
                    break;
                case "KEY_RELEASE":
                    KeyRelease(gameEvent.Message);
                    break;
                }
            }
        }
    }
}
