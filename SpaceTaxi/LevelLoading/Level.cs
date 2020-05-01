using DIKUArcade.Entities;
using SpaceTaxi.StaticObjects;
using SpaceTaxi.Enums;

namespace SpaceTaxi.LevelLoading {
    public class Level {
        // Add fields as needed
        public EntityContainer<VisualObjects> obstacles = new EntityContainer<VisualObjects>();
        public EntityContainer<Platform> platforms = new EntityContainer<Platform>();
        public Player player;
        public string name;

        public Level(string Name) { 
            name = Name;
        }

        public void UpdateLevelLogic() { 
            player.Move();
            player.GraficUpdate();
            player.Gravity();
        }

        public void RenderLevelObjects() {
            player.Entity.RenderEntity();   
            platforms.RenderEntities();
            obstacles.RenderEntities();
        }
    }
}
