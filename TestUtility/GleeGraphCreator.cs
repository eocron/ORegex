using Microsoft.Glee.Drawing;
using Eocron;
using Eocron.Core.FinitieStateAutomaton;

namespace TestUtility
{
    public sealed class GleeGraphCreator
    {
        public Graph Create<TValue>(IFSA<TValue> fsa, PredicateTable<TValue> table)
        {
            var graph = new Graph(fsa.Name);
            FillGraph(graph, fsa, table);
            return graph;
        }

        private static void FillGraph<TValue>(Graph graph, IFSA<TValue> fsa, PredicateTable<TValue> table)
        {
            foreach (var t in fsa.Transitions)
            {
                Edge edge = graph.AddEdge("q" + t.From, t.Condition.ToString(), "q" + t.To);

                if (fsa.IsFinal(t.To))
                {
                    edge.TargetNode.Attr.Fillcolor = Color.Gray;
                    edge.TargetNode.Attr.Shape = Shape.DoubleCircle;
                }
            }
        }
    }
}
