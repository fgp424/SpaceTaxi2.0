using System;
using DIKUArcade.Entities;
using SpaceTaxi.StaticObjects;
using SpaceTaxi.Enums;
using DIKUArcade.Math;
using System.Collections.Generic;

namespace SpaceTaxi.LevelLoading {
    public class Level {
        // Add fields as needed
        public EntityContainer<VisualObjects> obstacles = new EntityContainer<VisualObjects>();

        public List<EntityContainer<Platform>> speratedplatforms = new List<EntityContainer<Platform>>();
        public EntityContainer<VisualObjects> portal = new EntityContainer<VisualObjects>();
        public char[] Platforms;
        public List<Customer> CustomerList = new List<Customer>();

        public Player player;
        public string name;
        public Vec2F startpos;

/// <summary> Level method in charge of changeing the name </summary>
/// <param name="Name"> Defines name of the level </param>
        public Level(string Name) { 
            name = Name;
            Console.WriteLine(Platforms);

        }


/// <summary> Updates the logic of the level </summary>
        public void UpdateLevelLogic() { 
            if (!player.Entity.IsDeleted()){
                player.Move();
                player.GraficUpdate();
                player.Gravity();
            }
            
        }
/// <summary> Renders the objects of the level </summary>
        public void RenderLevelObjects() {
            if (!player.Entity.IsDeleted()){
                player.Entity.RenderEntity();   
            }
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
