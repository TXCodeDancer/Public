﻿using QuikGraph;
using QuikGraph.Algorithms.Observers;
using QuikGraph.Algorithms.Search;
using System.Collections.Generic;

namespace Algorithms.Observers
{
    public class UndirectedVertexDistanceObs
    {
        public static IDictionary<int, double> Get(UndirectedGraph<int, Edge<int>> g)
        {
            var dfs = new UndirectedDepthFirstSearchAlgorithm<int, Edge<int>>(g);
            var recorder = new UndirectedVertexDistanceRecorderObserver<int, Edge<int>>(edgeWeights => 1.0);

            using (recorder.Attach(dfs))
            {
                dfs.Compute();
                return recorder.Distances;
            }
        }

        public static IDictionary<string, double> Get(UndirectedGraph<string, TaggedEdge<string, string>> g)
        {
            var dfs = new UndirectedDepthFirstSearchAlgorithm<string, TaggedEdge<string, string>>(g);
            var recorder = new UndirectedVertexDistanceRecorderObserver<string, TaggedEdge<string, string>>(edgeWeights => double.Parse(edgeWeights.Tag));

            using (recorder.Attach(dfs))
            {
                dfs.Compute();
                return recorder.Distances;
            }
        }
    }
}