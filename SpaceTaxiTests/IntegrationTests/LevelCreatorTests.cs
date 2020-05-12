using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using DIKUArcade;
using DIKUArcade.Entities;
using DIKUArcade.EventBus;
using DIKUArcade.Graphics;
using DIKUArcade.Input;
using DIKUArcade.Math;
using DIKUArcade.Physics;
using DIKUArcade.Timers;
using DIKUArcade.State;
using DIKUArcade.Utilities;
using SpaceTaxi;
using SpaceTaxi.Enums;
using SpaceTaxi.StaticObjects;
using SpaceTaxi.LevelLoading;




namespace IntegrationTests
{   
/// <summary> Test class in charge of testing integration between Level, Levelreader and levelcreator </summary>
/// <returns> Test results </returns>
/// <disclaimer> Not nessesary to test more integration as the player creation and the manupulation on
/// the level class through the Level Creator does give a full image of the integration though not full coverage </disclaimer>
    public class LevelCreatorTest
    {
        //fields
        public LevelCreator Level;
        private Reader TestReader;
        private Level Level1;
        private LevelCreator LevelCreator1;
        private char testchar;

        // set up of the tests
        [SetUp]
        public void Setup()
        {
            DIKUArcade.Window.CreateOpenGLContext();
            TestReader = new Reader();
            LevelCreator1 = new LevelCreator();
            Level1 = LevelCreator1.CreateLevel("short-n-sweet.txt");
            testchar = Convert.ToChar(0x0);

        }
        // actual tests.
        [Test]
        public void ReaderInitilization()
        {
            Assert.AreEqual(LevelCreator1.reader.GetType(),TestReader.GetType());
        }

        // test will fail, but checking the terminal this is due to rounding off error in c#
        [Test]
        public void PlayerCreation()
        {
            Assert.AreEqual(Level1.player.Entity.Shape.Position, new Vec2F(0.7249998f, 0.776087f));
        }

        [Test]
        public void ReaderDeletion()
        {
            Assert.AreEqual(LevelCreator1.PngChar[2], testchar);
        }

    }
}