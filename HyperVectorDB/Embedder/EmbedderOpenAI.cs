// MODIFIED FILE
// MODIFIED ON THE FOLLOWING DATES: 8/20/2024

using System;
using System.ClientModel;
using System.Text.Json;
using OpenAI;
using OpenAI.Embeddings;

namespace HyperVectorDB.Embedder
{
    public class EmbedderOpenAI : IEmbedder {
        private readonly string _model;
        private readonly EmbeddingClient _client;
        public EmbedderOpenAI(string model, string apiKey, OpenAIClientOptions options) {
            _model = model;
            _client = new EmbeddingClient(model, new ApiKeyCredential(apiKey), options);
        }
        public double[] GetVector(string document)
        {
            // Create the input data
            BinaryData input = BinaryData.FromObjectAsJson(new
            {
                model = _model,
                input = document,
                encoding_format = "float"
            });

            // Send the request and get the response
            ClientResult response = _client.GenerateEmbeddings(BinaryContent.Create(input));
            BinaryData output = response.GetRawResponse().Content;

            // Parse the JSON response
            using JsonDocument outputAsJson = JsonDocument.Parse(output.ToString());
            JsonElement vector = outputAsJson.RootElement
                .GetProperty("data"u8)[0]
                .GetProperty("embedding"u8);

            // Convert the JSON array to a float array
            float[] floatArray = new float[vector.GetArrayLength()];
            int index = 0;
            foreach (JsonElement element in vector.EnumerateArray())
            {
                floatArray[index++] = element.GetSingle();
            }

            // Convert the float array to a double array
            return Array.ConvertAll(floatArray, item => (double)item);
        }

        public double[][] GetVectors(string[] documents)
        {
            // Create the input data
            BinaryData input = BinaryData.FromObjectAsJson(new
            {
                model = _model,
                input = documents,
                encoding_format = "float"
            });

            // Send the request and get the response
            ClientResult response = _client.GenerateEmbeddings(BinaryContent.Create(input));
            BinaryData output = response.GetRawResponse().Content;

            // Parse the JSON response
            using JsonDocument outputAsJson = JsonDocument.Parse(output.ToString());
            JsonElement dataArray = outputAsJson.RootElement.GetProperty("data");

            // Initialize the result array
            double[][] result = new double[dataArray.GetArrayLength()][];

            // Iterate over each document's embedding
            for (int i = 0; i < dataArray.GetArrayLength(); i++)
            {
                JsonElement vector = dataArray[i].GetProperty("embedding");

                // Convert the JSON array to a float array
                float[] floatArray = new float[vector.GetArrayLength()];
                int index = 0;
                foreach (JsonElement element in vector.EnumerateArray())
                {
                    floatArray[index++] = element.GetSingle();
                }

                // Convert the float array to a double array
                result[i] = Array.ConvertAll(floatArray, item => (double)item);
            }

            return result;
        }
    }
}
