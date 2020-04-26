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




/// <summary> Player class for player logic </summary>

public class Player : IGameEventProcessor<object> {


/// <summary> Properties </summary>
    public Entity Entity {get; private set;}
    private Vec2F Physics;

/// <summary> A constructor that creates Player as an instance</summary>
/// <param name="shape"> Defines the position of the dynamic shape </param>
/// <param name="image"> The image used for the object</param>
/// <returns> Player instance </returns>   
    public Player(DynamicShape shape, IBaseImage image) {
        Entity = new Entity(shape, image);
        TaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
    }


    public void Gravity(){
        Physics.Y = Physics.Y-0.001f;
        if (Physics.X >= 0){
            Physics.X = Physics.X - 0.001f;
        }
    }



    public void Direction(Vec2F vec) {
        Entity.Shape.AsDynamicShape().Direction = vec;
    }

    public void Move() {
        var x = Entity.Shape.Position.X;
        var y = Entity.Shape.Position.Y;
        var vecx = Entity.Shape.AsDynamicShape().Direction.X;
        var vecy = Entity.Shape.AsDynamicShape().Direction.Y;
        Entity.Shape.Move();
        if ((x + vecx) > 0.0 && (x + vecx) < 0.90)
            Entity.Shape.Move();
    }
    
/// <summary> Method for eventbus </summary>
    public void ProcessEvent(GameEventType eventType,
        GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch(gameEvent.Message) {
                    case "BOOSTER_UPWARDS":
                        Direction(new Vec2F(0.0f, 0.01f));
                        break;

                    case "BOOSTER_TO_RIGHT":
                        Direction(new Vec2F(0.01f, 0.0f));
                        break;

                    case "BOOSTER_TO_LEFT":
                        Direction(new Vec2F(-0.01f, 0.0f));
                        break;

                    case "STOP_ACCELERATE_UP":
                        Direction(new Vec2F(0.00f, 0.0f));
                        break;
                    case "STOP_ACCELERATE_RIGHT":
                        Direction(new Vec2F(0.00f, 0.0f));
                        break;   
                    case "STOP_ACCELERATE_LEFT":
                        Direction(new Vec2F(0.00f, 0.0f));
                        break;                   
                }
            }
        }
}
