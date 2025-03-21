using AsYouLikeIt.Sdk.Common.Extensions;

namespace AsYouLikeIt.Sdk.UnitTests
{
    public class Test_BlobNameSafeExtension
    {
        [Fact]
        public void TestMakeBlobNameSafe_ComplexAllowedNames()
        {
            // Arrange
            string input = "folder@name/file@name.txt";
            string expected = "folder@name/file@name.txt";

            // Act
            string result = input.MakeBlobNameSafe();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestMakeBlobNameSafe_SegmentLengthExceeded()
        {
            // Arrange
            string input = new string('a', 257) + "/file.txt";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => input.MakeBlobNameSafe());
        }

        [Fact]
        public void TestMakeBlobNameSafe_PathTooLong()
        {
            // Arrange
            string input = new string('a', 1025);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => input.MakeBlobNameSafe());
        }

        [Fact]
        public void TestMakeBlobNameSafe_ManyIterations()
        {
            // Arrange
            string input = "folder1/folder2/folder3/file.txt";
            string expected = "folder1/folder2/folder3/file.txt";

            // Act
            string result = input.MakeBlobNameSafe();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestMakeBlobNameSafe_DisallowedCharacters()
        {
            // Arrange
            string input = "folder*name><|/{asdfasdfas}/file?name.txt";
            string expected = "folder*name/{asdfasdfas}/filename.txt";

            // Act
            string result = input.MakeBlobNameSafe();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}