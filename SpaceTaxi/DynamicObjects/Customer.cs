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

    public Vec2F position {get; private set;}
    public Orientation orientation {get; private set;}
    public float bounderyleft{get; private set;}
    public float bounderyright{get; private set;}
    public string name{get; private set;}
    public float spawntimer{get; private set;}
    public char currentplatform{get; private set;}
    public string destinationplatform{get; private set;}
    public float dropofftimer{get; private set;}
    public float points{get; private set;}
    public bool isSpawned{get; private set;}
    private IBaseImage standRight;
    private IBaseImage standLeft;
    private IBaseImage walkRight;
    private IBaseImage walkLeft;
    

    public Customer(DynamicShape shape, IBaseImage image, string Name, char Currentplatform, string Destinationplatform,
        float Dropofftimer, float Points) : base(shape, image){
        
        standRight = new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png"));
        standLeft= new Image(Path.Combine("Assets", "Images", "CustomerStandLeft.png"));

        walkRight = new ImageStride(80,ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkRight.png")));
        walkLeft = new ImageStride(80,ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkLeft.png")));
        
        position.X = 5.0f;
        position.Y = 5.0f;
        
        
        shape = new DynamicShape(position.X, position.Y, 0.05f, 0.1f);
        image = standRight;
        orientation = (Orientation)1;

        dropofftimer = Dropofftimer;
        currentplatform = Currentplatform;
        destinationplatform = Destinationplatform;
        points = Points;
        name = Name;
        
        isSpawned = false;

    }

    public void Spawn(Vec2F Startpos){
        position.X = Startpos.X;
        position.Y = Startpos.Y;
    }
    public void Bounderies(float Bounderyleft, float Bounderyright){
        bounderyleft = Bounderyleft;
        bounderyright = Bounderyright;
    }
    public void Move(){
        
    }

    public void Timer(){
        spawntimer = spawntimer - 1f/60f;
        if(spawntimer <= 0){
            isSpawned = true;
        }
        dropofftimer = dropofftimer - 1f/60f;
        if(dropofftimer < 0){
            points = points - 1f/60f;
        }
    }


}