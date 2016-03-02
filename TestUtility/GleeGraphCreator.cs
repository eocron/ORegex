using System.Linq;
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
            foreach (var look in fsa.Transitions.ToLookup(x=>x.From,x=>x))
            {
                int i = 1;
                foreach (var t in look)
                {
                    Edge edge = graph.AddEdge("q" + t.From, string.Format("[{0}]{1}",i++,t.Condition), "q" + t.To);

                    if (fsa.IsFinal(t.To))
                    {
                        edge.TargetNode.Attr.Fillcolor = Color.Gray;
                        edge.TargetNode.Attr.Shape = Shape.DoubleCircle;
                    }
                }
            }
        }
    }
}
