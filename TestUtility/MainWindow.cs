using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Glee;
using Microsoft.Glee.Drawing;
using ORegex;
using ORegex.Core.Parse;
using ORegex.Core.StateMachine;
using Edge = Microsoft.Glee.Edge;

namespace TestUtility
{
    public partial class MainWindow : Form
    {
        private readonly ORegexParser<object> _parser = new ORegexParser<object>();
        private readonly StateMachineBuilder<object> _stb = new StateMachineBuilder<object>();
        private readonly PredicateTable<object> _table = new PredicateTable<object>();

        public MainWindow()
        {
            _table.AddPredicate("a", x => x == null);
            _table.AddPredicate("b", x => x != null);
            InitializeComponent();
        }

        private void ProcessORegex(string oregex)
        {
            Dictionary<object, int> idGen = new Dictionary<object, int>();
            var result = _parser.Parse(oregex, _table);
            var start = new State<object>();
            var end = new State<object> { IsFinal = true };
            _stb.Evaluate(start, end, result);

            var graph = new Graph("graph");
            Visit(start, idGen, new HashSet<object>(), graph);

            gViewer1.Graph = graph;
            gViewer1.Refresh();
        }

        private void Visit(State<object> s, Dictionary<object, int> idGen, HashSet<object> visited, Graph graph)
        {
            if (visited.Contains(s))
            {
                return;
            }
            visited.Add(s);
            var id = GetId(idGen, s);
            foreach (var t in s.Transitions)
            {
                var tid = GetId(idGen, t.Item2);
                if (t.Item1 == null)
                {
                    graph.AddEdge(id.ToString(), "eps" ,tid.ToString());
                }
                else
                {
                    graph.AddEdge(id.ToString(), _table.GetName(t.Item1), tid.ToString());
                }
                Visit(t.Item2, idGen, visited, graph);
            }
        }

        private int GetId(Dictionary<object, int> idGen, object obj)
        {
            int res;
            if (!idGen.TryGetValue(obj, out res))
            {
                res = idGen.Count;
                idGen[obj] = res;
            }
            return res;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            var text = richTextBox1.Text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                ProcessORegex(text);
            }
        }
    }
}
