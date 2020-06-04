using NUnit.Framework;
using SpaceTaxi;
using SpaceTaxi.Enums;

namespace SpaceTaxiTest {
   [TestFixture]
    public class StateTransformerTests {

        [Test]
        public void StringToStateTestMainMenu() {
            Assert.AreEqual(GameStateType.MainMenu, StateTransformer.TransformStringToState("GAME_MAINMENU"));
        }

        [Test]
        public void StringToStateTestGameRunning() {
            Assert.AreEqual(GameStateType.GameRunning, StateTransformer.TransformStringToState("GAME_RUNNING"));
        }

        [Test]
        public void StringToStateTestGamePaused() {
            Assert.AreEqual(GameStateType.GamePaused, StateTransformer.TransformStringToState("GAME_PAUSED"));
        }

        [Test]
        public void StringToStateTestGameResume() {
            Assert.AreEqual(GameStateType.GameResume, StateTransformer.TransformStringToState("GAME_RESUME"));
        }


        [Test]
        public void TestTransformStateToStringGameRunning() {
            Assert.AreEqual("GAME_RUNNING", StateTransformer.TransformStateToString(GameStateType.GameRunning));
        }
        
        [Test]
        public void TestTransformStateToStringGamePaused() {
            Assert.AreEqual("GAME_PAUSED", StateTransformer.TransformStateToString(GameStateType.GamePaused));
        }

        [Test]
        public void TestTransformStateToStringGameMainMenu() {
            Assert.AreEqual("GAME_MAINMENU", StateTransformer.TransformStateToString(GameStateType.MainMenu));
        }

        [Test]
        public void TestTransformStateToStringGameResyme() {
            Assert.AreEqual("GAME_RESUME", StateTransformer.TransformStateToString(GameStateType.GameResume));
        }
    }
}