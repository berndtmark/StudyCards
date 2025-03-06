using Microsoft.Extensions.Caching.Memory;
using Moq;
using StudyCards.Application.Interfaces;
using StudyCards.Application.SecretsManager;

namespace StudyCard.Application.Tests.SecretManagerTests;

[TestClass]
public class CachedSecretsManagerTests
{
    private Mock<ISecretClient> _secretClientMock = default!;
    private Mock<IMemoryCache> _memoryCacheMock = default!;
    private CachedSecretsManager _cachedSecretsManager = default!;

    [TestInitialize]
    public void Setup()
    {
        _secretClientMock = new Mock<ISecretClient>();
        _memoryCacheMock = new Mock<IMemoryCache>();
        _cachedSecretsManager = new CachedSecretsManager(_secretClientMock.Object, _memoryCacheMock.Object);
    }

    [TestMethod]
    public void GetSecret_WhenKeyInCache_ReturnsCachedValue()
    {
        // Arrange
        const string key = "testKey";
        const string expectedValue = "cachedValue";
        object value = expectedValue;
        _memoryCacheMock.Setup(x => x.TryGetValue(key, out value!)).Returns(true);

        // Act
        var result = _cachedSecretsManager.GetSecret(key);

        // Assert
        Assert.AreEqual(expectedValue, result);
        _secretClientMock.Verify(x => x.Get(It.IsAny<string[]>()), Times.Never);
    }

    [TestMethod]
    public void GetSecrets_WhenAllKeysInCache_ReturnsAllFromCache()
    {
        // Arrange
        var keys = new[] { "key1", "key2" };
        var expectedValues = new Dictionary<string, string>
        {
            { "key1", "value1" },
            { "key2", "value2" }
        };

        object value1 = expectedValues["key1"];
        object value2 = expectedValues["key2"];
        _memoryCacheMock.Setup(x => x.TryGetValue("key1", out value1!)).Returns(true);
        _memoryCacheMock.Setup(x => x.TryGetValue("key2", out value2!)).Returns(true);

        // Act
        var result = _cachedSecretsManager.GetSecrets(keys).ToDictionary();

        // Assert
        CollectionAssert.AreEquivalent(expectedValues.Keys, result.Keys);
        CollectionAssert.AreEquivalent(expectedValues.Values, result.Values);
        _secretClientMock.Verify(x => x.Get(It.IsAny<string[]>()), Times.Never);
    }

    [TestMethod]
    public void GetSecret_Generic_DeserializesJson()
    {
        // Arrange
        const string key = "testKey";
        var testObject = new { Name = "Test", Value = 123 };
        var jsonString = System.Text.Json.JsonSerializer.Serialize(testObject);

        object value = jsonString;
        _memoryCacheMock.Setup(x => x.TryGetValue(key, out value!)).Returns(true);

        // Act
        var result = _cachedSecretsManager.GetSecret<TestObject>(key);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Test", result.Name);
        Assert.AreEqual(123, result.Value);
    }

    private class TestObject
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }
}
