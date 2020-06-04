using System.IO;
using DIKUArcade.Graphics;
using DIKUArcade.Entities;
using DIKUArcade.Math;
using SpaceTaxi.Enums;


/// <summary> Customer class for customer logic </summary>
public class Customer : Entity{

/// <summary> Fields </summary>
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

    public IBaseImage standRight{get; private set;}
    private IBaseImage standLeft;
    private IBaseImage walkRight;
    private IBaseImage walkLeft;
    
/// <summary>
/// Customer constructer that instantiaties the customer as an object
/// </summary>
/// <param name="Shape">Extend and position of object</param>
/// <param name="image">Visual representation of object</param>
/// <param name="Name">Name of customer</param>
/// <param name="Currentplatform">Current location of Customer</param>
/// <param name="Destinationplatform">Destination location of customer</param>
/// <param name="Dropofftimer">Time to drop off customer</param>
/// <param name="Points">How many points a instant deliver will give</param>
/// <param name="Spawntimer">Time for spawning of customer</param>
/// <returns></returns>
    public Customer(DynamicShape Shape, IBaseImage image, string Name, string Currentplatform, 
        string Destinationplatform, double Dropofftimer, double Points, double Spawntimer) 
        : base(Shape, image){

        dropOffAny = false;
        dropoffLevelNext = false;
        isDroppedOff = false;
        scoreCounted = false;

        standRight = new Image(Path.Combine("Assets", "Images", "CustomerStandRight.png"));
        standLeft= new Image(Path.Combine("Assets", "Images", "CustomerStandLeft.png"));

        walkRight = new ImageStride(80,ImageStride.CreateStrides(2, Path.Combine
            ("Assets", "Images", "CustomerWalkRight.png")));
        walkLeft = new ImageStride(80,ImageStride.CreateStrides(2, Path.Combine
            ("Assets", "Images", "CustomerWalkLeft.png")));
                
        
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
/// <summary>
/// Method that changes field values in customer
/// </summary>
    public void CountScore(){
        scoreCounted = true;
    }
/// <summary>
/// Method that changes field values in customer
/// </summary>
    public void EdgeOfDestination(float q){
        edgeOfDestination = q;
    }
/// <summary>
/// Markes customer as picked up and reduces destination platform
/// to a single char
/// </summary>
    public void PickedUp(){
        Shape.Position = new Vec2F(5.0f, 5.0f);
        pickedUp = true;
        if(destinationplatform == "^"){
            dropoffLevelNext = true;
            dropOffAny = true;
            destinationplatform.Replace("^", "");
        } else if(destinationplatform.Contains('^')){
            dropoffLevelNext = true;
            destinationplatform = destinationplatform.Replace("^", "");
        }
    }

/// <summary>
/// Updates customer position when spawned
/// </summary>
/// <param name="Startpos"></param>
    public void Spawn(Vec2F Startpos){
        Shape.Position.X = Startpos.X;
        Shape.Position.Y = Startpos.Y;
    }

/// <summary>
/// Moves customer to wards the x input
/// </summary>
/// <param name="x">destination input</param>
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

/// <summary>
/// Moves the player towards the left edge if the Customer is tagged as dropped off
/// </summary>
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

/// <summary>
/// "Despawns" the player by moving it out of frame
/// </summary>
    public void despawn(){
        Shape.Position = new Vec2F(5.0f, 5.0f);
    }
/// <summary>
/// Method that changes field values in customer
/// </summary>
    public void dropOff(){
            isDroppedOff = true;
            pickedUp = false;
    }
/// <summary>
/// Method that changes field values in customer
/// </summary>
    public void dropOffReset(){
        isDroppedOff = false;
    }

/// <summary>
/// Method that stops the player and changes it's visual representation
/// to standing still in the orientation it is
/// </summary>
    public void StopMove(){
        if (orientation == (Orientation)1){
            Image = standRight;
        } else if (orientation == (Orientation)0){
            Image = standLeft;
        }
    }
/// <summary>
/// Method that changes field values in customer
/// </summary>
    public void Spawned(){
        HasSpawned = true;
    }

/// <summary>
/// timer tracker for player
/// </summary>
    public void Timer(){
        if(spawntimer >= 0){
            spawntimer = spawntimer - 1f/60f;
        }
        if(spawntimer <= 0){
            isSpawned = true;
        }
        dropofftimer = dropofftimer - 1f/60f;
        if(isSpawned){
            points = points - 10f/60f;
        }
    }


}