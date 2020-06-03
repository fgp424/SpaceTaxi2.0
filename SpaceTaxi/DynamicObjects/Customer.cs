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
    public double spawntimer{get; private set;}
    public string currentplatform{get; private set;}
    public string destinationplatform{get; private set;}
    public double dropofftimer{get; private set;}
    public double points{get; private set;}
    public bool isSpawned{get; private set;}
    public bool HasSpawned{get; private set;}
    public bool dropoffLevelNext{get; private set;}
    public bool dropOffAny{get; private set;}
    public bool pickedUp{get; private set;}
    public bool isDroppedOff{get; private set;}
    public bool scoreCounted{get; private set;}
    public float edgeOfDestination{get; private set;}

    private IBaseImage standRight;
    private IBaseImage standLeft;
    private IBaseImage walkRight;
    private IBaseImage walkLeft;
    

    public Customer(DynamicShape Shape, IBaseImage image, string Name, string Currentplatform, string Destinationplatform,
        double Dropofftimer, double Points, double Spawntimer) : base(Shape, image){

        dropOffAny = false;
        dropoffLevelNext = false;
        isDroppedOff = false;
        scoreCounted = false;

        standRight = new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png"));
        standLeft= new Image(Path.Combine("Assets", "Images", "CustomerStandLeft.png"));

        walkRight = new ImageStride(80,ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkRight.png")));
        walkLeft = new ImageStride(80,ImageStride.CreateStrides(2, Path.Combine("Assets", "Images", "CustomerWalkLeft.png")));
                
        
        image = standRight;
        orientation = (Orientation)1;

        dropofftimer = Dropofftimer;
        currentplatform = Currentplatform;
        destinationplatform = Destinationplatform;
        points = Points;
        name = Name;
        spawntimer = Spawntimer;
        
        isSpawned = false;

        HasSpawned = false;
    }
    public void CountScore(){
        scoreCounted = true;
    }
    public void EdgeOfDestination(float q){
        edgeOfDestination = q;
    }
    public void PickedUp(){
        Shape.Position = new Vec2F(5.0f, 5.0f);
        pickedUp = true;
        if(destinationplatform == "^"){
            dropoffLevelNext = true;
            dropOffAny = true;
            destinationplatform.Replace("^", "");

            System.Console.WriteLine(dropOffAny);
            System.Console.WriteLine(destinationplatform);
        } else if(destinationplatform.Contains("^")){
            dropoffLevelNext = true;
            System.Console.WriteLine(dropOffAny);
            System.Console.WriteLine(destinationplatform);
            destinationplatform.Replace("^", "");
        }
    }

    public void Spawn(Vec2F Startpos){
        Shape.Position.X = Startpos.X;
        Shape.Position.Y = Startpos.Y;
    }
    public void Bounderies(float Bounderyleft, float Bounderyright){
        bounderyleft = Bounderyleft;
        bounderyright = Bounderyright;
    }
    public void Move(float x){
        if (!isDroppedOff){
            if(Shape.Position.X > x){
                Shape.Position.X = Shape.Position.X - 0.001f;
                Shape.AsDynamicShape().Direction = new Vec2F(-0.001f, 0.00f);
                Image = walkLeft;
                orientation = (Orientation)0;
            } else if(Shape.Position.X < x){
                Shape.Position.X = Shape.Position.X + 0.001f;
                Shape.AsDynamicShape().Direction = new Vec2F(0.001f, 0.00f);
                Image = walkRight;
                orientation = (Orientation)1;
            }
        } 
    }

    public void IsDroppedOffMove(){
        if (isDroppedOff){
            //if(Shape.Position.X > edgeOfDestination){
                Shape.Position.X = Shape.Position.X - 0.001f;
                Shape.AsDynamicShape().Direction = new Vec2F(-0.001f, 0.00f);
                Image = walkLeft;
                orientation = (Orientation)0;
            //}
        }
    }

    public void despawn(){
        Shape.Position = new Vec2F(5.0f, 5.0f);
    }
    public void dropOff(){
            isDroppedOff = true;
    }

    public void StopMove(){
        if (orientation == (Orientation)1){
            Image = standRight;
        } else if (orientation == (Orientation)0){
            Image = standLeft;
        }
    }

    public void Spawned(){
        HasSpawned = true;
    }
    public void Timer(){
        if(spawntimer >= 0){
            spawntimer = spawntimer - 1f/60f;
        }
        if(spawntimer <= 0){
            isSpawned = true;
        }
        dropofftimer = dropofftimer - 1f/60f;
        if(dropofftimer < 0){
            points = points - 10f/60f;
        }
    }


}