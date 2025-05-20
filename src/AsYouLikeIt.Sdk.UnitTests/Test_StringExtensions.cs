using AsYouLikeIt.Sdk.Common.Extensions;

namespace AsYouLikeIt.Sdk.UnitTests
{
    public class Test_StringExtensions
    {
        [Fact]
        public void Guidify_WithSemicolon_ReturnsGuids()
        {
            // Arrange
            var input = "40523707-5a17-49ce-b924-5efc1eca240b;8bd16aa3-44f2-4099-b013-7f6b54891b63";

            // Act
            var result = input.Guidify();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void Guidify_WithCustomSeparators_ReturnsGuids()
        {
            // Arrange
            var input = "40523707-5a17-49ce-b924-5efc1eca240b,8bd16aa3-44f2-4099-b013-7f6b54891b63";

            // Act
            var result = input.Guidify(',');

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void ToCsvWithSpace_ReturnsCommaSeparated()
        {
            // Arrange
            var input = new List<string> { "One", "Two", "Three" };

            // Act
            var result = input.ToCsvWithSpace();

            // Assert
            Assert.Equal("One, Two, Three", result);
        }

        [Theory]
        [InlineData("Monday", DayOfWeek.Monday)]
        [InlineData("Sunday", DayOfWeek.Sunday)]
        public void ToEnum_ParsesEnumValues(string input, DayOfWeek expected)
        {
            // Act
            var result = input.ToEnum<DayOfWeek>();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GenerateStreamFromString_ReturnsReadableStream()
        {
            // Arrange
            var input = "Hello World";

            // Act
            using var stream = input.GenerateStreamFromString();
            using var reader = new StreamReader(stream);
            var result = reader.ReadToEnd();

            // Assert
            Assert.Equal(input, result);
        }

        [Theory]
        [InlineData("40523707-5a17-49ce-b924-5efc1eca240b", true)]
        [InlineData("not-a-guid", false)]
        public void IsGuid_WorksAsExpected(string input, bool expected)
        {
            // Act
            var result = input.IsGuid();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MakeFileNameSafe_ReplacesSpaces_RemovesAccents()
        {
            // Arrange
            var input = "Tést Fïle Nàme";

            // Act
            var result = input.MakeFileNameSafe();

            // Assert
            Assert.Equal("Test-File-Name", result);
        }

        [Fact]
        public void MakeBlobNameSafe_ThrowsWhenNull()
        {
            // Arrange
            string input = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => input.MakeBlobNameSafe());
        }

        [Fact]
        public void MakeBlobNameSafe_TrimsAndStripsSlashes()
        {
            // Arrange
            var input = "/folder/subfolder//Test File.pdf/";

            // Act
            var result = input.MakeBlobNameSafe(true);

            // Assert
            Assert.Equal("folder/subfolder/test-file.pdf", result);
        }

        [Fact]
        public void ReplaceCaseInsensitive_ReplacesIgnoringCase()
        {
            // Arrange
            var input = "Hello hello HeLLo";

            // Act
            var result = input.ReplaceCaseInsensitive("hello", "X");

            // Assert
            Assert.Equal("X X X", result);
        }

        [Fact]
        public void ReplaceFirst_OnlyReplacesFirstOccurrence()
        {
            // Arrange
            var input = "abc123abc123";

            // Act
            var result = input.ReplaceFirst("abc", "XYZ");

            // Assert
            Assert.Equal("XYZ123abc123", result);
        }

        [Theory]
        [InlineData("tést", "test")]
        [InlineData("", "")]
        [InlineData(null, "")]
        public void RemoveAccents_Success(string input, string expected)
        {
            // Act
            var result = input.RemoveAccents();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SetFixedLengthWithSpaces_ExtendsStringWithSpaces()
        {
            // Arrange
            var input = "Test";

            // Act
            var result = input.SetFixedLengthWithSpaces(10);

            // Assert
            Assert.Equal(10, result.Length);
            Assert.StartsWith("Test", result);
        }

        [Fact]
        public void CapFirst_LowercasesThenTitleCase()
        {
            // Arrange
            var input = "hEllo wOrld";

            // Act
            var result = input.CapFirst();

            // Assert
            Assert.Equal("Hello World", result);
        }

        [Theory]
        [InlineData(null, 5, false)]
        [InlineData("12", 5, false)]
        [InlineData("abc123", 5, false)]
        [InlineData("Abc12", 5, true)]
        public void IsValidKeyStrict_BehavesAsExpected(string input, int maxLength, bool expected)
        {
            // Act
            var result = input.IsValidKeyStrict(maxLength);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToValidKey_ReplacesSpacesAndNonAlphanumeric()
        {
            // Arrange
            var input = "Hello World !!!";

            // Act
            var result = input.ToValidKey();

            // Assert
            Assert.Equal("Hello-World", result);
        }

        [Theory]
        [InlineData(null, "Default", false, "Default")]
        [InlineData("", "Default", false, "Default")]
        [InlineData("   ", "Default", true, "Default")]
        [InlineData("Value", "Default", true, "Value")]
        public void DefaultIfNullOrEmpty_ReturnsExpected(
            string input,
            string defaultValue,
            bool trimWhitespace,
            string expected)
        {
            // Act
            var result = input.DefaultIfNullOrEmpty(defaultValue, trimWhitespace);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Hello", 3, "Hel")]
        [InlineData(null, 3, "")]
        [InlineData("Hi", 5, "Hi")]
        public void Left_ReturnsExpected(string input, int length, string expected)
        {
            // Act
            var result = input.Left(length);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Hello", 3, "llo")]
        [InlineData(null, 3, "")]
        [InlineData("Hi", 5, "Hi")]
        public void Right_ReturnsExpected(string input, int length, string expected)
        {
            // Act
            var result = input.Right(length);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("123", 5, "00123")]
        [InlineData("9", 1, "9")]
        public void PadWithZeros_ReturnsExpected(string input, int totalDigits, string expected)
        {
            // Act
            var result = input.PadWithZeros(totalDigits);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void EmptyIfNull_ReturnsEmptyIfNull()
        {
            // Arrange
            string input = null;

            // Act
            var result = input.EmptyIfNull();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Theory]
        [InlineData("Hello", "hello", true)]
        [InlineData("Hello", "WORLD", false)]
        [InlineData(null, null, true)]
        public void EqualsCaseInsensitive_ComparesIgnoringCase(string helper, string compareTo, bool expected)
        {
            // Act
            var result = helper.EqualsCaseInsensitive(compareTo);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ContainsCaseInsensitive_FindsPatternIgnoringCase()
        {
            // Arrange
            var helper = "Hello World";

            // Act
            var result = helper.ContainsCaseInsensitive("WORLD");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SplitStringAndTrim_SplitsBySpecifiedChars()
        {
            // Arrange
            var helper = "One;Two;  Three  ";

            // Act
            var result = helper.SplitStringAndTrim(';').ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal("Two", result[1]);
        }

        [Fact]
        public void TitleCase_ConvertsToTitleCase()
        {
            // Arrange
            var helper = "helLo worLd";

            // Act
            var result = helper.TitleCase();

            // Assert
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void ToCamelCase_ConvertsToCamelCase()
        {
            // Arrange
            var helper = "Hello World";

            // Act
            var result = helper.ToCamelCase();

            // Assert
            Assert.Equal("helloWorld", result);
        }

        [Theory]
        [InlineData("abc123", "abc123")]
        [InlineData("abc@123!", "abc123")]
        [InlineData(null, "")]
        public void StripNonAlphaNumeric_RemovesNonAlphaNumeric(string input, string expected)
        {
            // Act
            var result = input.StripNonAlphaNumeric();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Reverse_ReversesString()
        {
            // Arrange
            var input = "ABC";

            // Act
            var result = input.Reverse();

            // Assert
            Assert.Equal("CBA", result);
        }

        [Fact]
        public void StripFileExtension_RemovesExtension()
        {
            // Arrange
            var input = "myFile.txt";

            // Act
            var result = input.StripFileExtension();

            // Assert
            Assert.Equal("myFile", result);
        }

        [Fact]
        public void GetFileExtension_ReturnsExpectedExtension()
        {
            // Arrange
            var input = "myFile.txt";

            // Act
            var result = input.GetFileExtension();

            // Assert
            Assert.Equal(".txt", result);
        }

        [Fact]
        public void LeftWithoutBreakingWords_TruncatesAtWordBoundary()
        {
            // Arrange
            var input = "Hello world, this is a test.";

            // Act
            var result = input.LeftWithoutBreakingWords(11);

            // Assert
            Assert.Equal("Hello world", result);
        }

        [Fact]
        public void TruncateToWordWithEllipses_RespectsMaxLength()
        {
            // Arrange
            var input = "The quick brown fox jumps over the lazy dog";

            // Act
            var result = input.TruncateToWordWithEllipses(16);

            // Assert
            Assert.StartsWith("The quick brown", result);
            Assert.EndsWith("\u2026", result);
        }

        [Fact]
        public void TruncateToWordWithEllipses_RespectsMaxLength_UseThreePeriods()
        {
            // Arrange
            var input = "The quick brown fox jumps over the lazy dog";

            // Act
            var result = input.TruncateToWordWithEllipses(18, useThreePeriods: true);

            // Assert
            Assert.StartsWith("The quick", result);
            Assert.EndsWith("...", result);
        }
    }
}