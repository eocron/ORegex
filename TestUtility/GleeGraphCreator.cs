using System.Drawing;
using System.Linq;
using Microsoft.Glee.Drawing;
using Eocron.Core.FinitieStateAutomaton;

namespace TestUtility
{
    public sealed class GleeGraphCreator
    {
        public Graph Create<TValue>(IFSA<TValue> fsa)
        {
            var graph = new Graph(fsa.Name);
            FillGraph(graph, fsa);
            return graph;
        }

        private static void FillGraph<TValue>(Graph graph, IFSA<TValue> fsa)
        {
            foreach (var look in fsa.Transitions.ToLookup(x=>x.BeginState,x=>x))
            {
                int i = 1;
                foreach (var t in look)
                {
                    Edge edge = graph.AddEdge("q" + t.BeginState, string.Format("[{0}]{1}",i++,t.Condition), "q" + t.EndState);

                    if (fsa.IsFinal(t.EndState))
                    {
                        edge.TargetNode.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.Gray;
                        edge.TargetNode.Attr.Shape = Shape.DoubleCircle;
                    }
                }
            }
        }
    }
}
