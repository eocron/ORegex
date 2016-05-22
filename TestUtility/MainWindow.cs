using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Eocron;
using Eocron.Core.Ast;
using Eocron.Core.FinitieStateAutomaton;
using Eocron.Core.Parse;
using Tests.Core;

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
            //richTextBox2.Text += string.Format("\nPossible names: {0}", string.Join(", ",_table.AvailableNames));
            HighLightSyntax();
        }

        private void DrawGraphFastFsa(IFSA<char> fsm)
        {
            var graph = _graphCreator.Create(fsm, _table);
            fastFsaGraph.Graph = graph;
            fastFsaGraph.Refresh();
        }

        private void DrawGraphCmdFsa(IFSA<char> fsa)
        {
            var graph = _graphCreator.Create(fsa, _table);
            cmdFsaGraph.Graph = graph;
            cmdFsaGraph.Refresh();
        }

        private void ProcessORegex(string oregex)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var fa = (FiniteAutomaton<char>)_compiler.Build(oregex, _table, Options);
            var elapsed = sw.Elapsed;
            label1.Text = "Compiled in: " + elapsed;
            DrawGraphCmdFsa(fa.CmdFsa);
            DrawGraphFastFsa(fa.FastFsa);
        }

        private ORegexOptions Options
        {
            get
            {
                var additionalOptions = ORegexOptions.None;
                var xoptions = optionsBox.Text;
                if (!string.IsNullOrWhiteSpace(xoptions))
                {
                    try
                    {
                        additionalOptions = additionalOptions |
                                            xoptions.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                                                .Select(x => Enum.Parse(typeof (ORegexOptions), x))
                                                .Cast<ORegexOptions>()
                                                .Aggregate((output, next) => output | next);
                    }
                    catch (Exception)
                    {
                        additionalOptions = ORegexOptions.None;
                    }
                }
                return additionalOptions;
            }
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
                //try
                //{
                    Stopwatch sw = Stopwatch.StartNew();
                    ProcessORegex(text);
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
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
                var test = new PerformanceTest();
                test.BuildTest((int)IterationsCountBoxBuild.Value, Options);
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
            var oregex = new ORegex<char>(ORegexPatternBox.Text, ORegexOptions.None, _table);
            var matches = regex.Matches(InputTextBox.Text).Cast<Match>().ToArray();
            var omatches = oregex.Matches(InputTextBox.Text.ToCharArray()).ToArray();

            if (matches.Length != omatches.Length)
            {
                MessageBox.Show("Invalid matches count!", "Count", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < matches.Length; i++)
            {
                var exp = matches[i];
                var act = omatches[i];

                if (exp.Index != act.Index || exp.Length != act.Length)
                {
                    MessageBox.Show("Invalid range!" , "Match dismatch", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            MessageBox.Show("All good.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                var sw = Stopwatch.StartNew();
                var test = new PerformanceTest();
                test.RunTest((int)IterationsCountBoxRun.Value);
                sw.Stop();
                label7.Text = "Elapsed " + sw.Elapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
