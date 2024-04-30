namespace HeatItOn.Tests
{
    public class ResultDataManagerTests
    {
        [Fact]
        public void ReadResultData_EmptyOrInvalidFileName_ThrowsException()
        {
            // Arrange
            var resultDataManager = new ResultDataManager();
            
            // Act & Assert
            Assert.ThrowsAny<IOException>(() => resultDataManager.ReadResultData(""));
            Assert.ThrowsAny<IOException>(() => resultDataManager.ReadResultData("invalid"));
        }
    }
}