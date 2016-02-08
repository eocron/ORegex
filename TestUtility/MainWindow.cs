using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Glee.Drawing;
using ORegex.Core.Parse;
using ORegex.Core.StateMachine;

namespace TestUtility
{
    public partial class MainWindow : Form
    {
        private readonly ORegexCompiler<object> _compiler = new ORegexCompiler<object>();
        private readonly DebugPredicateTable<object> _table = new DebugPredicateTable<object>();

        public MainWindow()
        {
            InitializeComponent();
            richTextBox2.Text += string.Format("\nPossible names: {0}", string.Join(", ",_table.AvailableNames));
        }

        private void DrawGraph(DFA<object> dfsm)
        {
            var graph = new Graph("graph");
            foreach (var t in dfsm.Edges)
            {
                var edge = graph.AddEdge("q"+t.StartState, _table.GetName(t.Condition), "q"+t.EndState);
                if (dfsm.final.Contains(t.EndState))
                {
                    edge.TargetNode.Attr.Fillcolor = Color.Gray;
                    edge.TargetNode.Attr.Shape = Shape.DoubleCircle;
                }
            }
            gViewer1.Graph = graph;
            gViewer1.Refresh();
        }
        private void ProcessORegex(string oregex)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var dfa = _compiler.Build(oregex, _table);
            var elapsed = sw.Elapsed;
            label1.Text = "Compiled in: " + elapsed;
            //Visit(start, idGen, new HashSet<object>(), graph);
            DrawGraph(dfa);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var text = richTextBox1.Text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                try
                {
                    ProcessORegex(text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
    }
}
