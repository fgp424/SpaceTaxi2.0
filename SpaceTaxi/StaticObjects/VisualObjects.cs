using DIKUArcade.Graphics;
using DIKUArcade.Entities;


namespace SpaceTaxi.StaticObjects {

    
    public class VisualObjects : Entity {

    /// <summary> Properties </summary>
        public Entity Entity {get; private set;}

    /// <summary> Constructor that creates a static image </summary>
    /// <param name="shape"> Defines the position of the stationary shape </param>
    /// <param name="image"> The image used for the object</param>
    /// <returns> Visual objects instance </returns>
        public VisualObjects(StationaryShape shape, IBaseImage image) : base(shape, image){
            Entity = new Entity(shape, image);
        }
    }












}