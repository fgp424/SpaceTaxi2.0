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

        public Level(string Name) { 
            name = Name;
        }

        public void UpdateLevelLogic() { 
            if (player.Entity.IsDeleted() == false){
                player.Move();
                player.GraficUpdate();
                player.Gravity();
            }
            
        }

        public void RenderLevelObjects() {
            if (player.Entity.IsDeleted() == false){
                player.Entity.RenderEntity();   
            }
            platforms.RenderEntities();
            obstacles.RenderEntities();
        }
    }
}
