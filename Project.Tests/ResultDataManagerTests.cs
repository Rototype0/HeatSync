namespace HeatItOn.Tests
{
    public class ResultDataManagerTests
    {
        [Fact]
        public void ReadResultData_EmptyFileName_ThrowsDirectoryNotFoundException()
        {
            // Arrange
            var resultDataManager = new ResultDataManager();
            string emptyFileName = "";

            // Act & Assert
            Assert.Throws<DirectoryNotFoundException>(() => resultDataManager.ReadResultData(emptyFileName));
        }
    }
}