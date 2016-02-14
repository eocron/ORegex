using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ORegex;
using ORegex.Core.Ast;
using ORegex.Core.FinitieStateAutomaton;
using ORegex.Core.Parse;

namespace TestUtility
{
    public partial class MainWindow : Form
    {
        private readonly ORegexCompiler<char> _compiler = new ORegexCompiler<char>();
        private readonly DebugPredicateTable _table = new DebugPredicateTable();
        private readonly ORegexParser<char> _parser = new ORegexParser<char>(); 
        private readonly GleeGraphCreator _graphCreator = new GleeGraphCreator();

        public MainWindow()
        {
            InitializeComponent();
            richTextBox2.Text += string.Format("\nPossible names: {0}", string.Join(", ",_table.AvailableNames));
            HighLightSyntax();
        }

        private void DrawGraph(FSA<char> fsm)
        {
            var graph = _graphCreator.Create(fsm, _table);
            gViewer1.Graph = graph;
            gViewer1.Refresh();
        }

        private void DrawGraph(CFSA<char> fsm)
        {
            var graph = _graphCreator.Create(fsm, _table);
            gViewer1.Graph = graph;
            gViewer1.Refresh();
        }

        private void ProcessORegex(string oregex)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var dfa = _compiler.Build(oregex, _table);
            var elapsed = sw.Elapsed;
            label1.Text = "Compiled in: " + elapsed;
            DrawGraph(dfa);
        }

        private void HighLightSyntax()
        {
            var oregex = richTextBox1.Text;

            if (!string.IsNullOrWhiteSpace(oregex))
            {
                Colorize(richTextBox1, 0, oregex.Length, Color.LimeGreen);//asdaa
                try
                {
                    var ast = _parser.Parse(oregex, _table);

                    var stack = new Stack<AstNodeBase>();
                    stack.Push(ast);
                    while (stack.Count>0)
                    {
                        var node = stack.Pop();

                        if (node is AstGroupNode)
                        {
                            Colorize(richTextBox1, node.Range.Index, node.Range.Length, Color.DodgerBlue);
                        }
                        else if(node is AstAtomNode<object>)
                        {
                            Colorize(richTextBox1, node.Range.Index, node.Range.Length, Color.Brown);
                        }

                        foreach (var child in node.GetChildren())
                        {
                            stack.Push(child);
                        }
                    }
                }
                catch (Exception e)
                {
                    Colorize(richTextBox1, 0, oregex.Length, Color.Red);
                }
            }
        }

        private void Colorize(RichTextBox rtb, int index, int length, Color color)
        {
            var prevI = rtb.SelectionStart;
            var prevL = rtb.SelectionLength;

            rtb.Enabled = false;

            rtb.SelectionStart = index;
            rtb.SelectionLength = length;

            rtb.SelectionColor = color;

            rtb.Enabled = true; 

            rtb.SelectionLength = prevL;
            rtb.SelectionStart = prevI;
            rtb.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var text = richTextBox1.Text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                try
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    ProcessORegex(text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            HighLightSyntax();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                var test = new PerformanceTest((int)IterationsCountBox.Value);
                test.Run();
                sw.Stop();
                label2.Text = "Elapsed " + sw.Elapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TestRegexEqualityButton_Click(object sender, EventArgs e)
        {
            var regex = new Regex(RegexPatternBox.Text);
            var oregex = new ObjectRegex<char>(ORegexPatternBox.Text, ORegexOptions.None, _table);
            var matches = regex.Matches(InputTextBox.Text).Cast<Match>().ToArray();
            var omatches = oregex.Matches(InputTextBox.Text.ToCharArray()).ToArray();
        }
    }
}
