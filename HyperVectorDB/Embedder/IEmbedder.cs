using System;

namespace HyperVectorDB.Embedder
{
    public interface IEmbedder {

        public Double[] GetVector(String Document);
        public Double[][] GetVectors(String[] Documents);

    }
}
