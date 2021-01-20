using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using WordCount.Controllers;

namespace WordCount.Test
{
    [TestClass]
    public class WordsCountConrollerTests
    {
        [TestMethod]
        public void Should_Get_WordCounts()
        {

            // Arrange
            var controller = new WordsController();

            // Act
            var actionResult = controller.WordCounts();

            //Assert                
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(6583,actionResult.Value.Count);
        }

        [TestMethod]
        public void Should_Get_TopWords()
        {

            // Arrange
            var controller = new WordsController();

            // Act
            var actionResult = controller.TopWords(3);

            //Assert                
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(3, actionResult.Value.Count);
        }

        [TestMethod]
        public async Task Should_Get_WordDefinition()
        {

            // Arrange
            var controller = new WordsController();

            // Act
            var actionResult = await controller.WordDefinition("south");

            //Assert                
            Assert.IsNotNull(actionResult);
            Assert.AreEqual(2, actionResult.Value.Count);
        }


        [TestMethod]
        public async Task Should_Get_NULL_WordDefinition()
        {

            // Arrange
            var controller = new WordsController();

            // Act
            var actionResult = await controller.WordDefinition("aaaaaa");

            //Assert                
            Assert.IsNull(actionResult.Value);            
        }
    }
}
