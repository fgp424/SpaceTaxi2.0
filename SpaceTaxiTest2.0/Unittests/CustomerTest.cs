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

/// <summary> Test class in charge of unittesting on the player objectt </summary>
/// <returns> Test results </returns>
    public class CustomerTest
    {
        
        public Customer Customer;

        [SetUp]
        public void Setup()
        {
            DIKUArcade.Window.CreateOpenGLContext();
            Customer = new Customer (new DynamicShape(new Vec2F(0.0f, 0.0f), new Vec2F((.1f), (.1f))), 
                                new Image(Path.Combine("Assets", "Images", "Taxi_Thrust_None_Right.png")),
                                "Kurt", "1", "^J", 10, 100, 10);
                
        }


        [Test]
        public void CustomerMoveRightTest()
        {
            Customer.Move(1.0f);
            Vec2F Expected = new Vec2F(0.001F,0.0f);
            Assert.AreEqual(Expected.X, Customer.Shape.Position.X);
            Assert.AreEqual(Expected.Y, Customer.Shape.Position.Y);
        }

        [Test]
        public void CustomerMoveLeftTest()
        {
            Customer.Move(-1.0f);
            Vec2F Expected = new Vec2F(-0.001F,0.0f);
            Assert.AreEqual(Expected.X, Customer.Shape.Position.X);
            Assert.AreEqual(Expected.Y, Customer.Shape.Position.Y);
        }
        
        [Test]
        public void CustomerÏstantiationTest(){
            Assert.AreEqual(Customer.name, "Kurt");
            Assert.AreEqual(Customer.currentplatform, "1");
            Assert.AreEqual(Customer.destinationplatform, "^J");
            Assert.AreEqual(Customer.name, "Kurt");
        }

        [Test]
        public void CountScoreTest(){
            Customer.CountScore();
            Assert.IsTrue(Customer.scoreCounted);
        }

        [Test]
        public void EdgeOfDestinationTest(){
            Customer.EdgeOfDestination(1.0f);
            Assert.AreEqual(Customer.edgeOfDestination, 1.0f);
        }

        [Test]
        public void PickedUpTest(){
            Customer.PickedUp();
            Assert.IsTrue(Customer.dropoffLevelNext);
            Assert.AreEqual(Customer.destinationplatform, "J");
        }

        [Test]
        public void SpawnTest(){
            Customer.Spawn(new Vec2F(0.001F,0.0f));
            Assert.AreEqual(MathF.Abs(Customer.Shape.Position.X), MathF.Abs(0.001f));
            Assert.AreEqual(MathF.Abs(Customer.Shape.Position.Y), MathF.Abs(0.000f));
        }

        [Test]
        public void DropOffTest(){
            Customer.dropOff();
            Assert.IsTrue(Customer.isDroppedOff);
            Assert.IsFalse(Customer.pickedUp);
        }

        [Test]
        public void DropOffResetTest(){
            Customer.dropOffReset();
            Assert.IsFalse(Customer.isDroppedOff);
        }

        [Test]
        public void IsDroppedOffMoveTest(){
            Customer.dropOff();
            Customer.IsDroppedOffMove();
            Assert.AreEqual(MathF.Abs(Customer.Shape.Position.X), MathF.Abs(-0.001f));
            Assert.AreEqual(MathF.Abs(Customer.Shape.Position.Y), MathF.Abs(0.000f));
        }

        [Test]
        public void StopMoveTest(){
            Customer.StopMove();
            Assert.AreEqual(Customer.Image, Customer.standRight);
        }

        [Test]
        public void SpawnedTest(){
            Customer.Spawned();
            Assert.IsTrue(Customer.HasSpawned);
        }

        [Test]
        public void TimerTest(){
            Customer.Timer();
        }
        




    }
