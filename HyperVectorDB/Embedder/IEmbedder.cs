using System;

namespace HyperVectorDB.Embedder
{
    /// <summary>
    /// Provides a contract for generating vector embeddings from text documents.
    /// </summary>
    public interface IEmbedder
    {
        /// <summary>
        /// Generates a vector embedding for the specified document.
        /// </summary>
        /// <param name="Document">
        /// A <see cref="String"/> containing the text document for which to generate the embedding.
        /// </param>
        /// <returns>
        /// A <see cref="Double"/> array representing the vector embedding of the document.
        /// </returns>
        public Double[] GetVector(String Document);

        /// <summary>
        /// Generates vector embeddings for an array of documents.
        /// </summary>
        /// <param name="Documents">
        /// An array of <see cref="String"/> where each element is a text document to be embedded.
        /// </param>
        /// <returns>
        /// A jagged array of <see cref="Double"/> arrays. Each sub-array represents the embedding vector for the corresponding document.
        /// </returns>
        public Double[][] GetVectors(String[] Documents);
    }
}
