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
using SpaceTaxi.LevelLoading;
using SpaceTaxi.StaticObjects;


namespace SpaceTaxi.GameStates { 
    public class MainMenu : IGameState {
        private static MainMenu instance = null;
        private Entity backGroundImage;
        private Text[] menuButtons; 
        private int activeMenuButton = 0; 
        private int maxMenuButtons;
         
         
        public static MainMenu GetInstance() { 
            return MainMenu.instance ?? (MainMenu.instance = new MainMenu()); 
        }
        public void GameLoop(){
            
        }

        public void InitializeGameState(){
            backGroundImage = new Entity(
                new StationaryShape(new Vec2F(0.0f, 0.0f), new Vec2F(1f, 1f)),
                new ImageStride(55,ImageStride.CreateStrides(24, Path.Combine("Assets", "Images", "menu.png"))));

            maxMenuButtons = 2;

            menuButtons = new[]{new Text("New Game",new Vec2F(0.35f, 0.3f), new Vec2F(0.4f, 0.4f)),
            new Text("Exit Game",new Vec2F(0.35f, 0.1f), new Vec2F(0.4f, 0.4f))
            };
            
            menuButtons[0].SetColor(new Vec3I(0, 255, 0));
            menuButtons[1].SetColor(new Vec3I(0, 0, 0));
        }

        public void UpdateGameLogic(){
            
        }

        public void RenderState(){
            backGroundImage.RenderEntity();

            for (int i = 0; i< maxMenuButtons; i++){
                menuButtons[i].RenderText();
            }


            
        }

        public void HandleKeyEvent(string keyValue, string keyAction){
            if (keyAction == "KEY_PRESS") {
                switch(keyValue) {
                    case "KEY_UP":
                        activeMenuButton = 0;
                        menuButtons[0].SetColor(new Vec3I(0, 255, 0));
                        menuButtons[1].SetColor(new Vec3I(0, 0, 0));                   
                        break;
                    case "KEY_DOWN":
                        activeMenuButton = 1;
                        menuButtons[0].SetColor(new Vec3I(0, 0, 0));
                        menuButtons[1].SetColor(new Vec3I(0, 255, 0));
                        break;
                    case "KEY_SPACE":
                        if (activeMenuButton == 0) {
                            TaxiBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.GameStateEvent, this, "CHANGE_STATE", "GAME_RUNNING", ""));
                        } else if (activeMenuButton ==1) {
                            TaxiBus.GetBus().RegisterEvent(
                            GameEventFactory<object>.CreateGameEventForAllProcessors(
                            GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                        }
                        break;
                }
            } else if (keyAction == "KEY_RELEASE"){ 
                switch(keyValue) {
                    case "KEY_UP":
                        break;
                    case "KEY_DOWN":
                        break;
                    case "KEY_SPACE":
                        break;
                }
            } else {
                throw new System.ArgumentException("KeyState unknown");
            }
        }



    }
}
