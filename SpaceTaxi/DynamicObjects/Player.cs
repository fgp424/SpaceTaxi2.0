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

/// <summary> A constructor that creates Player as an instance</summary>
/// <param name="shape"> Defines the position of the dynamic shape </param>
/// <param name="image"> The image used for the object</param>
/// <returns> Player instance </returns>   
    public Player(DynamicShape shape, IBaseImage image) {
        Entity = new Entity(shape, image);
        TaxiBus.GetBus().Subscribe(GameEventType.PlayerEvent, this);
    }


    private void Direction(Vec2F vec) {
        Entity.Shape.AsDynamicShape().Direction = vec;
    }

    public void Move() {
        var x = Entity.Shape.Position.X;
        var vecx = Entity.Shape.AsDynamicShape().Direction.X;
        if ((x + vecx) > 0.0 && (x + vecx) < 0.90)
            Entity.Shape.Move();
    }
    
/// <summary> Method for eventbus </summary>
    public void ProcessEvent(GameEventType eventType,
        GameEvent<object> gameEvent) {
            if (eventType == GameEventType.PlayerEvent) {
                switch(gameEvent.Message) {
                    case "MOVE_PLAYER_RIGHT":
                        Direction(new Vec2F(0.01f, 0.0f));
                        break;

                    case "MOVE_PLAYER_LEFT":
                        Direction(new Vec2F(-0.01f, 0.0f));
                        break;

                    case "STOP_PLAYER":
                        Direction(new Vec2F(0.00f, 0.0f));
                        break;                    
                }
            }
        }
}
