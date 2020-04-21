    public enum GameStateType
    {
        GameRunning,
        GamePaused,
        MainMenu,
        GameResume
    }
public class StateTransformer{
    

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