using System.Linq;
using Eocron.Core;
using Eocron.Core.Ast;
using NUnit.Framework;

namespace Tests.Unit
{
    [TestFixture]
    public sealed class SequenceHandlerTests
    {
        [Test]
        public void ForwardRun()
        {
            var seq = new[] {0, 1, 2, 3, 4, 5};
            var handler = new SequenceHandler<int>(seq);
            Assert.AreEqual(handler.Count, seq.Length);
            for (int i = 0; i < handler.Count; i++)
            {
                Assert.AreEqual(seq[i], handler[i]);
            }
        }

        [Test]
        public void InvertRun()
        {
            var seq = new[] {0, 1, 2, 3, 4, 5};
            var handler = new SequenceHandler<int>(seq);
            handler.Reverse = true;
            var inverted = seq.Reverse().ToArray();
            Assert.AreEqual(handler.Count, seq.Length);
            for (int i = 0; i < handler.Count; i++)
            {
                Assert.AreEqual(inverted[i], handler[i]);
            }
        }

        [Test]
        public void CorrectInvertTransform()
        {
            var seq = new[] {0, 1, 2, 3, 4, 5};
            var handler = new SequenceHandler<int>(seq);
            Range range = new Range(1, 3);
            Assert.AreEqual(new Range(1, 3), handler.Translate(new Range(1, 3)));
        }

        [Test]
        public void CorrectRealInvertTransform()
        {
            var seq = new[] {0, 1, 2, 3, 4, 5};
            var handler = new SequenceHandler<int>(seq);
            handler.Reverse = true;

            Assert.AreEqual(new Range(2, 3), handler.Translate(new Range(1, 3)));
            Assert.AreEqual(new Range(0, 0), handler.Translate(new Range(6, 0)));
            Assert.AreEqual(new Range(3, 3), handler.Translate(new Range(0, 3)));
        }
    }
}
