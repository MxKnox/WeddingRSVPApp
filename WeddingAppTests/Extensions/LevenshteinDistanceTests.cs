using WeddingApp.Extensions;

namespace WeddingAppTests.Extensions;

[TestClass]
public class LevenshteinDistanceTests
{
    [TestMethod]
    [DataRow("", "", 0, DisplayName = "Two empty strings")]
    [DataRow("hello", "", 5, DisplayName = "Empty string vs non-empty")]
    [DataRow("", "world", 5, DisplayName = "Non-empty vs empty string")]
    [DataRow("hello", "hello", 0, DisplayName = "Identical strings")]
    public void LevenshteinDistance_BasicCases_ReturnsExpectedDistance(string s1, string s2, int expected)
    {
        // Act
        var actual = s1.LevenshteinDistance(s2);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow("cat", "bat", 1, DisplayName = "Single substitution")]
    [DataRow("cat", "cats", 1, DisplayName = "Single insertion at end")]
    [DataRow("cats", "cat", 1, DisplayName = "Single deletion from end")]
    [DataRow("cat", "at", 1, DisplayName = "Single deletion from start")]
    [DataRow("at", "cat", 1, DisplayName = "Single insertion at start")]
    [DataRow("a", "b", 1, DisplayName = "Single character substitution")]
    [DataRow("a", "", 1, DisplayName = "Single character to empty")]
    [DataRow("", "a", 1, DisplayName = "Empty to single character")]
    public void LevenshteinDistance_SingleCharacterOperations_ReturnsExpectedDistance(string s1, string s2,
        int expected)
    {
        // Act
        var actual = s1.LevenshteinDistance(s2);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow("kitten", "sitting", 3, DisplayName = "Classic example: kitten -> sitting")]
    [DataRow("saturday", "sunday", 3, DisplayName = "saturday -> sunday")]
    [DataRow("intention", "execution", 5, DisplayName = "intention -> execution")]
    [DataRow("abc", "xyz", 3, DisplayName = "Completely different short strings")]
    [DataRow("hello", "world", 4, DisplayName = "Completely different words")]
    public void LevenshteinDistance_MultipleOperations_ReturnsExpectedDistance(string s1, string s2, int expected)
    {
        // Act
        var actual = s1.LevenshteinDistance(s2);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow("John", "Jon", 1, DisplayName = "Missing 'h' in name")]
    [DataRow("Smith", "Smyth", 1, DisplayName = "i/y substitution")]
    [DataRow("Michael", "Micheal", 2, DisplayName = "Transposed characters")]
    [DataRow("Catherine", "Katherine", 1, DisplayName = "C/K substitution")]
    [DataRow("Johnson", "Jonson", 1, DisplayName = "Missing 'h'")]
    [DataRow("MacDonald", "McDonald", 1, DisplayName = "a/c difference")]
    [DataRow("O'Connor", "OConnor", 1, DisplayName = "Apostrophe removal")]
    [DataRow("De Silva", "DeSilva", 1, DisplayName = "Space removal")]
    [DataRow("Van Der Berg", "VanDerBerg", 2, DisplayName = "Multiple spaces removed")]
    [DataRow("José", "Jose", 1, DisplayName = "Accent removal")]
    public void LevenshteinDistance_NameVariations_ReturnsExpectedDistance(string s1, string s2, int expected)
    {
        // Act
        var actual = s1.LevenshteinDistance(s2);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow("JOHN", "john", 0, DisplayName = "Case difference only")]
    [DataRow("John", "JOHN", 0, DisplayName = "Mixed case")]
    [DataRow("Hello", "HELLO", 0, DisplayName = "Upper vs lower case")]
    public void LevenshteinDistance_CaseVariations_ReturnsZeroDistance(string s1, string s2, int expected)
    {
        // Act
        var actual = s1.LevenshteinDistance(s2);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow("Christopher", "Chris", 6, DisplayName = "Full name vs nickname")]
    [DataRow("Elizabeth", "Liz", 6, DisplayName = "Another full name vs nickname")]
    [DataRow("Alexander Hamilton", "Alexandra Hamilton", 2, DisplayName = "Gender variant of first name")]
    [DataRow("Mary-Jane Watson", "Mary Jane Watson", 1, DisplayName = "Hyphen vs space")]
    public void LevenshteinDistance_LongerNameDifferences_ReturnsExpectedDistance(string s1, string s2, int expected)
    {
        // Act
        var actual = s1.LevenshteinDistance(s2);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow("ab", "ba", 2, DisplayName = "Simple transposition")]
    [DataRow("abcd", "abdc", 2, DisplayName = "Internal transposition")]
    [DataRow("aa", "aaa", 1, DisplayName = "One extra repeated character")]
    [DataRow("abab", "ab", 2, DisplayName = "Pattern removal")]
    public void LevenshteinDistance_TranspositionsAndPatterns_ReturnsExpectedDistance(string s1, string s2,
        int expected)
    {
        // Act
        var actual = s1.LevenshteinDistance(s2);

        // Assert
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void LevenshteinDistance_ClassicKittenSittingExample_Returns3()
    {
        // Arrange
        var s1 = "kitten";
        var s2 = "sitting";
        var expected = 3;

        // Act
        var actual = s1.LevenshteinDistance(s2);

        // Assert
        Assert.AreEqual(expected, actual, "The classic 'kitten' to 'sitting' example should return 3");
    }

    [TestMethod]
    [DataRow("John Smith", "Jon Smith", 0.9, DisplayName = "Minor spelling variation")]
    [DataRow("Michael Johnson", "Micheal Johnson", 0.86, DisplayName = "Common misspelling")]
    [DataRow("Catherine Williams", "Katherine Williams", 0.94, DisplayName = "C/K name variation")]
    [DataRow("Christopher Brown", "Chris Brown", 0.64, DisplayName = "Full name vs nickname")]
    [DataRow("Elizabeth Davis", "Liz Davis", 0.60, DisplayName = "Long name vs nickname")]
    [DataRow("Robert Miller", "Bob Miller", 0.65, DisplayName = "Traditional nickname")]
    public void CalculateSimilarity_RealWorldNames_ReturnsExpectedSimilarity(string name1, string name2,
        double expectedMinSimilarity)
    {
        // Act
        var distance = name1.LevenshteinDistance(name2);
        var actualSimilarity = 1.0 - (double)distance / Math.Max(name1.Length, name2.Length);

        // Assert
        Assert.IsTrue(actualSimilarity >= expectedMinSimilarity,
            $"Similarity between '{name1}' and '{name2}' should be at least {expectedMinSimilarity:P1}, but was {actualSimilarity:P1}");
    }

    [TestMethod]
    public void LevenshteinDistance_NullInputs_ThrowsException()
    {
        // Arrange
        string nullString = null;
        var validString = "test";

        // Act & Assert
        Assert.ThrowsException<NullReferenceException>(() => nullString.LevenshteinDistance(validString));
        Assert.ThrowsException<NullReferenceException>(() => validString.LevenshteinDistance(nullString));
    }

    [TestMethod]
    [DataRow("John Doe", "John Doe", 1.0, DisplayName = "Identical names should have 100% similarity")]
    [DataRow("", "", 1.0, DisplayName = "Two empty strings should have 100% similarity")]
    [DataRow("A", "Z", 0.0, DisplayName = "Completely different single chars should have 0% similarity")]
    public void CalculateSimilarity_EdgeCases_ReturnsExpectedSimilarity(string name1, string name2,
        double expectedSimilarity)
    {
        // Act
        var distance = name1.LevenshteinDistance(name2);
        double actualSimilarity;

        if (name1.Length == 0 && name2.Length == 0)
            actualSimilarity = 1.0; // Both empty strings are identical
        else
            actualSimilarity = 1.0 - (double)distance / Math.Max(name1.Length, name2.Length);

        // Assert
        Assert.AreEqual(expectedSimilarity, actualSimilarity, 0.01,
            $"Similarity between '{name1}' and '{name2}' should be {expectedSimilarity:P1}, but was {actualSimilarity:P1}");
    }
}