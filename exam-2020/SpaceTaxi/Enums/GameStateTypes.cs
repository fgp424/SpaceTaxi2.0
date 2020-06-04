


namespace SpaceTaxi.Enums {
    public enum GameStateType
    {
        GameRunning,
        GamePaused,
        MainMenu,
        GameResume
    }

/// <summary>
/// Statetransformer class for converting states
/// </summary>
    public class StateTransformer{
/// <summary>
/// Changes the state from string to state
/// </summary>
/// <param name="state">string with statename input</param>
/// <returns>state enum</returns>
        public static GameStateType TransformStringToState(string state){
            GameStateType ret = GameStateType.GameRunning;
            switch (state) {
                case "GAME_RUNNING":
                    ret = GameStateType.GameRunning;
                    break;
                case "GAME_PAUSED":
                    ret = GameStateType.GamePaused;
                    break;
                case "GAME_MAINMENU":
                    ret = GameStateType.MainMenu;
                    break;
                case "GAME_RESUME":
                    ret = GameStateType.GameResume;
                    break;
                default:
                    break;
            }
        return ret;
        }
/// <summary>
/// Transforms a state to a string
/// </summary>
/// <param name="state">Game state enum</param>
/// <returns>string</returns>
        public static string TransformStateToString(GameStateType state){
            var ret = "";
            switch (state) {
                case GameStateType.GameRunning:
                    ret = "GAME_RUNNING";
                    break;
                case GameStateType.GamePaused:
                    ret = "GAME_PAUSED";
                    break;
                case GameStateType.MainMenu:
                    ret = "GAME_MAINMENU";
                    break;
                case GameStateType.GameResume:
                    ret = "GAME_RESUME";
                    break;
                default:
                    break;
            }
        return ret;
        }


    }
}