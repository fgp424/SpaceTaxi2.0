using DIKUArcade.Entities;
using SpaceTaxi.StaticObjects;
using System.Collections.Generic;

namespace SpaceTaxi.LevelLoading
{
    public class Level {
        // Add fields as needed
        public EntityContainer<VisualObjects> obstacles = new EntityContainer<VisualObjects>();

        public List<EntityContainer<Platform>> speratedplatforms = new List<EntityContainer<Platform>>();
        public EntityContainer<VisualObjects> portal = new EntityContainer<VisualObjects>();
        public char[] Platforms;
        public List<Customer> CustomerList = new List<Customer>();
        public string name;

/// <summary> Level method in charge of changeing the name </summary>
/// <param name="Name"> Defines name of the level </param>
        public Level(string Name) { 
            name = Name;
        }


/// <summary> Updates the logic of the level </summary>
        public void UpdateLevelLogic() {            
        }
/// <summary> Renders the objects of the level </summary>
        public void RenderLevelObjects() {
            foreach (EntityContainer<Platform> e in speratedplatforms) {
                foreach (Platform p in e){
                    p.RenderEntity();
                }
            }


            foreach(Customer c in CustomerList){
                c.RenderEntity();
            }



            obstacles.RenderEntities();
        }
    }
}
