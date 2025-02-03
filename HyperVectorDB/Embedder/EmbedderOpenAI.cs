using System;
using System.ClientModel;
using System.Text.Json;
using OpenAI;
using OpenAI.Embeddings;

namespace HyperVectorDB.Embedder
{
    /// <summary>
    /// Provides functionality to generate vector embeddings for text documents
    /// using OpenAI's embedding API.
    /// </summary>
    public class EmbedderOpenAI : IEmbedder
    {
        private readonly string _model;
        private readonly EmbeddingClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmbedderOpenAI"/> class.
        /// </summary>
        /// <param name="model">
        /// The identifier of the OpenAI embedding model to use (e.g., "text-embedding-ada-002").
        /// </param>
        /// <param name="apiKey">
        /// The API key credential required to authenticate requests to the OpenAI API.
        /// </param>
        /// <param name="options">
        /// Options for configuring the OpenAI client.
        /// </param>
        public EmbedderOpenAI(string model, ApiKeyCredential apiKey, OpenAIClientOptions options)
        {
            _model = model;
            _client = new EmbeddingClient(model, apiKey, options);
        }

        /// <summary>
        /// Generates a vector embedding for a single text document.
        /// </summary>
        /// <param name="document">The text document to embed.</param>
        /// <returns>
        /// An array of <see cref="double"/> representing the embedding of the input document.
        /// </returns>
        public double[] GetVector(string document)
        {
            // Create the input data as JSON.
            BinaryData input = BinaryData.FromObjectAsJson(new
            {
                model = _model,
                input = document,
                encoding_format = "float"
            });

            // Send the request and get the response from the OpenAI embeddings API.
            ClientResult response = _client.GenerateEmbeddings(BinaryContent.Create(input));
            BinaryData output = response.GetRawResponse().Content;

            // Parse the JSON response.
            using JsonDocument outputAsJson = JsonDocument.Parse(output.ToString());
            JsonElement vector = outputAsJson.RootElement
                .GetProperty("data"u8)[0]
                .GetProperty("embedding"u8);

            // Convert the JSON array to a float array.
            float[] floatArray = new float[vector.GetArrayLength()];
            int index = 0;
            foreach (JsonElement element in vector.EnumerateArray())
            {
                floatArray[index++] = element.GetSingle();
            }

            // Convert the float array to a double array.
            return Array.ConvertAll(floatArray, item => (double)item);
        }

        /// <summary>
        /// Generates vector embeddings for an array of text documents.
        /// </summary>
        /// <param name="documents">An array of text documents to embed.</param>
        /// <returns>
        /// A jagged array where each element is an array of <see cref="double"/> representing
        /// the embedding for the corresponding document.
        /// </returns>
        public double[][] GetVectors(string[] documents)
        {
            // Create the input data as JSON.
            BinaryData input = BinaryData.FromObjectAsJson(new
            {
                model = _model,
                input = documents,
                encoding_format = "float"
            });

            // Send the request and get the response from the OpenAI embeddings API.
            ClientResult response = _client.GenerateEmbeddings(BinaryContent.Create(input));
            BinaryData output = response.GetRawResponse().Content;

            // Parse the JSON response.
            using JsonDocument outputAsJson = JsonDocument.Parse(output.ToString());
            JsonElement dataArray = outputAsJson.RootElement.GetProperty("data");

            // Initialize the result array.
            double[][] result = new double[dataArray.GetArrayLength()][];

            // Iterate over each document's embedding.
            for (int i = 0; i < dataArray.GetArrayLength(); i++)
            {
                JsonElement vector = dataArray[i].GetProperty("embedding");

                // Convert the JSON array to a float array.
                float[] floatArray = new float[vector.GetArrayLength()];
                int index = 0;
                foreach (JsonElement element in vector.EnumerateArray())
                {
                    floatArray[index++] = element.GetSingle();
                }

                // Convert the float array to a double array.
                result[i] = Array.ConvertAll(floatArray, item => (double)item);
            }

            return result;
        }
    }
}
