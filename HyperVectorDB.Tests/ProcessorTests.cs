// MODIFIED FILE
// MODIFIED ON THE FOLLOWING DATES: 8/19/2024, 8/20/2024

using OpenAI;

namespace HyperVectorDB.Tests;

[TestFixture]
public class ProcessorTests
{
    [SetUp]
    public void Setup()
    {
        if (Directory.Exists("TestDatabase"))
        {
            Directory.Delete("TestDatabase", true);
        }
    }

    [TearDown]
    public void Teardown()
    {
        if (Directory.Exists("TestDatabase"))
        {
            Directory.Delete("TestDatabase", true);
        }
    }

    [Test]
    public void PreprocessorTest()
    {
        OpenAIClientOptions options = new OpenAIClientOptions
        {
            Endpoint = new Uri("http://localhost:11434/v1")
        };
        HyperVectorDB DB = new HyperVectorDB(new Embedder.EmbedderOpenAI("all-minilm", "dummy_key", options), "TestDatabase");
        DB.IndexDocument("This is a test document about dogs", TestPreprocessor, null, "TestDatabase");
        DB.IndexDocument("This is a test document about cats", TestPreprocessor, null, "TestDatabase");
        DB.IndexDocument("This is a test document about fish", TestPreprocessor, null, "TestDatabase");
        DB.IndexDocument("This is a test document about birds", TestPreprocessor, null, "TestDatabase");
        DB.IndexDocument("This is a test document about dogs and cats", TestPreprocessor, null, "TestDatabase");
        DB.IndexDocument("This is a test document about cats and fish", TestPreprocessor, null, "TestDatabase");
        DB.IndexDocument("This is a test document about fish and birds", TestPreprocessor, null, "TestDatabase");
        DB.IndexDocument("This is a test document about birds and dogs", TestPreprocessor, null, "TestDatabase");
        DB.IndexDocument("This is a test document about dogs and cats and fish", TestPreprocessor, null, "TestDatabase");
        DB.IndexDocument("This is a test document about cats and fish and birds", TestPreprocessor, null, "TestDatabase");
        DB.IndexDocument("This is a test document about fish and birds and dogs", TestPreprocessor, null, "TestDatabase");
        DB.IndexDocument("This is a test document about birds and dogs and cats", TestPreprocessor, null, "TestDatabase");
        DB.IndexDocument("This is a test document about dogs and cats and fish and birds", TestPreprocessor, null, "TestDatabase");
        DB.IndexDocument("This is a test document about cats and fish and birds and dogs", TestPreprocessor, null, "TestDatabase");
        DB.IndexDocument("This is a test document about fish and birds and dogs and cats", TestPreprocessor, null, "TestDatabase");
        DB.IndexDocument("This is a test document about birds and dogs and cats and fish", TestPreprocessor, null, "TestDatabase");
        DB.Save();
        var result = DB.QueryCosineSimilarity("dogs", 1);
        var document = result.Documents[0];
        ClassicAssert.IsNotNull(document);
        ClassicAssert.IsTrue(document.DocumentString == document.DocumentString.ToUpperInvariant());
        ClassicAssert.Pass();
    }

    [Test]
    public void PostprocessorTest()
    {
        OpenAIClientOptions options = new OpenAIClientOptions
        {
            Endpoint = new Uri("http://localhost:11434/v1")
        };
        HyperVectorDB DB = new HyperVectorDB(new Embedder.EmbedderOpenAI("all-minilm", "dummy_key", options), "TestDatabase");
        DB.IndexDocument("This is a test document about dogs", null, TestPostprocessor, "TestDatabase");
        DB.Save();
        var result = DB.QueryCosineSimilarity("dogs", 1);
        var document = result.Documents[0];
        ClassicAssert.IsNotNull(document);
        ClassicAssert.IsTrue(document.DocumentString == document.DocumentString.ToLowerInvariant());
        ClassicAssert.Pass();
    }

    private string? TestPreprocessor(string line, string? path, int? lineNumber)
    {
        return line.ToUpperInvariant();
    }

    private string? TestPostprocessor(string line, string? path, int? lineNumber)
    {
        return line.ToLowerInvariant();
    }



}