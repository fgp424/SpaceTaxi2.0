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

public class Customer : Entity{

    public Vec2F startpos {get; private set;}
    public Orientation Orientation {get; private set;}
    public float bounderyleft{get; private set;}
    public float bounderyright{get; private set;}
    

    public Customer(DynamicShape shape, IBaseImage image) : base(shape, image){

    }

    public void bounderies(float Bounderyleft, float Bounderyright){
        bounderyleft = Bounderyleft;
        bounderyright = Bounderyright;
    }
    public void move(){
        
    }
}