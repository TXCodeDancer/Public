﻿using QuikGraph;
using QuikGraph.Algorithms.ConnectedComponents;
using System.Collections.Generic;

namespace Algorithms.ConnectedComponents
{
    public static class WeaklyConnectedComponents
    {
        public static IDictionary<int, int> Get(AdjacencyGraph<int, Edge<int>> g)
        {
            var algorithm = new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(g);
            algorithm.Compute();
            return algorithm.Components;
        }

        public static IDictionary<string, int> Get(AdjacencyGraph<string, TaggedEdge<string, string>> g)
        {
            var algorithm = new WeaklyConnectedComponentsAlgorithm<string, TaggedEdge<string, string>>(g);
            algorithm.Compute();
            return algorithm.Components;
        }

        public static BidirectionalGraph<int, Edge<int>>[] GetGraphs(AdjacencyGraph<int, Edge<int>> g)
        {
            var algorithm = new WeaklyConnectedComponentsAlgorithm<int, Edge<int>>(g);
            algorithm.Compute();
            return algorithm.Graphs;
        }

        public static BidirectionalGraph<string, TaggedEdge<string, string>>[] GetGraphs(AdjacencyGraph<string, TaggedEdge<string, string>> g)
        {
            var algorithm = new WeaklyConnectedComponentsAlgorithm<string, TaggedEdge<string, string>>(g);
            algorithm.Compute();
            return algorithm.Graphs;
        }
    }
}