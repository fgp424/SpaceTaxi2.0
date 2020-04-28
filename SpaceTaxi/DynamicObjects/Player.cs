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
using SpaceTaxi.Enums;




/// <summary> Player class for player logic </summary>

public class Player : IGameEventProcessor<object> {


/// <summary> Properties </summary>
    public Entity Entity {get; private set;}
    public Orientation Orientation;
    private Vec2F Physics;
    private IBaseImage UPRIGHT;
    private IBaseImage UPLEFT;
    private IBaseImage UPNRIGHT;
    private IBaseImage UPNLEFT;
    private IBaseImage RIGHT;
    private IBaseImage LEFT;
    private IBaseImage NONERIGHT;
    private IBaseImage NONELEFT;
    private bool IsUpPressed = false;
    private bool IsLeftPressed = false;
    private bool IsRightPressed = false;

/// <summary> A constructor that creates Player as an instance</summary>
/// <param name="shape"> Defines the position of the dynamic shape </param>
/// <param name="image"> The image used for the object</param>
/// <param name="orientation"> Enum, which indicates direction</param>
/// <returns> Player instance </returns>   
    public Player(DynamicShape shape, IBaseImage image, Orientation orientation) {
        Entity = new Entity(shape, image);
        Orientation = orientation;
        TaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);

        NONELEFT = new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None.png"));
        NONERIGHT= new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png"));

        UPNLEFT = new ImageStride(80,ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back.png")));
        UPNRIGHT = new ImageStride(80,ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Back_Right.png")));

        UPLEFT = new ImageStride(80,ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom.png")));
        UPRIGHT = new ImageStride(80,ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Bottom_Right.png")));

        LEFT = new ImageStride(80,ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Back.png")));
        RIGHT = new ImageStride(80,ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "Taxi_Thrust_Back_Right.png")));

        Physics = new Vec2F(0.0f, 0.0f);
    }


    public void Gravity(){
        Physics.Y = Physics.Y-0.00001f;
        if (Physics.X < 0 ){
            Physics.X = Physics.X + 0.00001f;
        } else if (Physics.X > 0 ){
            Physics.X = Physics.X - 0.00001f;
        }
    }

    public void GraficUpdate(){
        if (Orientation == (Orientation)0){
            if (IsUpPressed == true && IsLeftPressed == true){
                Entity.Image = UPNLEFT;
            } else if (IsUpPressed == true){
                Entity.Image = UPLEFT;
            } else if (IsLeftPressed == true){
                Entity.Image = LEFT;
            } else {
                Entity.Image = NONELEFT;
            }

        } else if (Orientation == (Orientation)1){
            if (IsUpPressed == true && IsRightPressed == true){
                Entity.Image = UPNRIGHT;
            } else if (IsUpPressed == true){
                Entity.Image = UPRIGHT;
            } else if (IsRightPressed == true){
                Entity.Image = RIGHT;
            } else {
                Entity.Image = NONERIGHT;
            }

        }

    }


    public void Direction(Vec2F vec) {
        Entity.Shape.AsDynamicShape().Direction = vec;
    }

    public void Move() {
        Entity.Shape.Move();
        Entity.Shape.AsDynamicShape().Direction = Physics;
            if (IsUpPressed == true && IsLeftPressed == true){
                Physics.Y = Physics.Y + 0.00005f;
                Physics.X = Physics.X - 0.0001f;
            } else if (IsUpPressed == true){
                Physics.Y = Physics.Y + 0.00005f;
            } else if (IsLeftPressed == true){
                Physics.X = Physics.X - 0.0001f;
            } else if (IsUpPressed == true && IsRightPressed == true){
                Physics.Y = Physics.Y + 0.0001f;
                Physics.X = Physics.X + 0.0001f;
            } else if (IsRightPressed == true){
                Physics.X = Physics.X + 0.0001f;}

    }

    
/// <summary> Method for eventbus </summary>
    public void ProcessEvent(GameEventType eventType,
        GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch(gameEvent.Message) {
                    case "BOOSTER_UPWARDS":
                        IsUpPressed = true;
                        break;

                    case "BOOSTER_TO_RIGHT":
                        Orientation = (Orientation)1;
                        IsRightPressed = true;
                        break;

                    case "BOOSTER_TO_LEFT":
                        Orientation = (Orientation)0;
                        IsLeftPressed = true;
                        break;

                    case "STOP_ACCELERATE_UP":
                        IsUpPressed = false;
                        break;
                    case "STOP_ACCELERATE_RIGHT":
                        IsRightPressed = false;
                        break;   
                    case "STOP_ACCELERATE_LEFT":
                        IsLeftPressed = false;
                        break;                   
                }
            }
        }
}
