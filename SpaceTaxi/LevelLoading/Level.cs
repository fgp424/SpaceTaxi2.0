using System;
using DIKUArcade.Entities;
using SpaceTaxi.StaticObjects;
using SpaceTaxi.Enums;
using DIKUArcade.Math;

namespace SpaceTaxi.LevelLoading {
    public class Level {
        // Add fields as needed
        public EntityContainer<VisualObjects> obstacles = new EntityContainer<VisualObjects>();
        public EntityContainer<Platform> platforms = new EntityContainer<Platform>();
        public EntityContainer<VisualObjects> portal = new EntityContainer<VisualObjects>();
        public Player player;
        public string name;
        public Vec2F startpos;

/// <summary> Level method in charge of changeing the name </summary>
/// <param name="Name"> Defines name of the level </param>
        public Level(string Name) { 
            name = Name;
        }


/// <summary> Updates the logic of the level </summary>
        public void UpdateLevelLogic() { 
            if (player.Entity.IsDeleted() == false){
                player.Move();
                player.GraficUpdate();
                player.Gravity();
            }
            
        }
/// <summary> Renders the objects of the level </summary>
        public void RenderLevelObjects() {
            if (player.Entity.IsDeleted() == false){
                player.Entity.RenderEntity();   
            }
            platforms.RenderEntities();
            obstacles.RenderEntities();
        }
    }
}
