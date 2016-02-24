using Microsoft.Glee.Drawing;
using ORegex;
using ORegex.Core.FinitieStateAutomaton;

namespace TestUtility
{
    public sealed class GleeGraphCreator
    {
        public Graph Create<TValue>(FSA<TValue> fsa, PredicateTable<TValue> table)
        {
            var graph = new Graph(fsa.Name);
            FillGraph(graph, fsa, table);
            return graph;
        }

        public Graph Create<TValue>(CFSA<TValue> fsa, PredicateTable<TValue> table)
        {
            var graph = new Graph(fsa.Name);
            FillGraph(graph, fsa, table);
            return graph;
        }

        private static void FillGraph<TValue>(Graph graph, IFSA<TValue> fsa, PredicateTable<TValue> table)
        {
            foreach (var t in fsa.Transitions)
            {
                Edge edge = graph.AddEdge("q" + t.From, t.Condition.IsComplexPredicate? "complex" : "func", "q" + t.To);

                if (fsa.IsFinal(t.To))
                {
                    edge.TargetNode.Attr.Fillcolor = Color.Gray;
                    edge.TargetNode.Attr.Shape = Shape.DoubleCircle;
                }
            }
        }
    }
}
