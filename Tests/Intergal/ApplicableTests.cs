using System;
using System.Diagnostics;
using System.Linq;
using Eocron;
using NUnit.Framework;

namespace Tests.Intergal
{
    [TestFixture]
    public sealed class ApplicableTests
    {
        [Test]
        public void PrimeTest()
        {
            var oregex = new ORegex<int>("{0}(?<pair>.{0})*", IsPrime);

            var input = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
            foreach (var match in oregex.Matches(input))
            {
                Trace.WriteLine(string.Join(",", match.Values));
            }
            //OUTPUT:
            //2
            //3,4,5,6,7
            //11,12,13
        }

        [Test]
        public void NumberGroupTest()
        {
            var oregex = new ORegex<int>("((?<pair>{0}){1})+", x => x != 0, x => x == 0);

            var input = new[] { 0, 0, 0, 1, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (var match in oregex.Matches(input))
            {
                Trace.WriteLine(string.Join(",", match.Values));
            }
        }

        private static bool IsPrime(int number)
        {
            int boundary = (int)Math.Floor(Math.Sqrt(number));

            if (number == 1) return false;
            if (number == 2) return true;

            for (int i = 2; i <= boundary; ++i)
            {
                if (number % i == 0) return false;
            }

            return true;
        }

        [Test]
        public void PersonSelectionTest()
        {
            //INPUT_TEXT: Пяточкова Тамара решила выгулять Джека и встретилась с Михаилом А.М.
            var sentence = new[]
            {
                new Word("Пяточкова", SemanticType.FamilyName),
                new Word("Тамара", SemanticType.Name),
                new Word("решила", SemanticType.Other),
                new Word("выгулять", SemanticType.Other),
                new Word("Джека", SemanticType.Name),
                new Word("и", SemanticType.Other),
                new Word("встретилась", SemanticType.Other),
                new Word("с", SemanticType.Other),
                new Word("Михаилом", SemanticType.Name),
                new Word("А.", SemanticType.Other),
                new Word("М", SemanticType.Other),
            };

            //Creating table which will contain our predicates.
            var pTable = new PredicateTable<Word>();
            pTable.AddPredicate("Фамилия", x => x.SemType == SemanticType.FamilyName);  //Check if word is FamilyName.
            pTable.AddPredicate("Имя", x => x.SemType == SemanticType.Name);            //Check if word is simple Name.
            pTable.AddPredicate("Инициал", x => IsInitial(x.Value));                    //Complex check if Value is Inital character.

            var oregex = new ORegex<Word>(@"
                {Фамилия}(?<name>{Имя})                     //Comments can written inside pattern...
                |
                (?<name>{Имя})({Фамилия}|{Инициал}{1,2})?  /*...even complex ones.*/
            ", pTable);

            var persons = oregex.Matches(sentence).Select(x => new Person(x)).ToArray();

            foreach (var person in persons)
            {
                Console.WriteLine("Person found: {0}, length: {1}", person.Name, person.Words.Length);
            }
            Assert.AreEqual(persons.Length, 3);
            //OUTPUT:
            //Person found: Тамара, length: 2
            //Person found: Джека, length: 1
            //Person found: Михаилом, length: 3
        }

        [Test]
        public void ByteMaskSearchTest()
        {
            var oregex = new ORegex<byte>("{0}{1}{2}{3}{4}{5}{6}{7}", 12, 3, 5, 76, 8, 0, 6, 125);
            byte[] toBeSearched = new byte[]
            {
                23, 36, 43, 76, 125, 56, 34, 234, 12, 3, 5, 76, 8, 0, 6, 125, 234, 56, 211, 122, 22, 4, 7, 89, 76, 64, 12,
                3, 5, 76, 8, 0, 6, 125
            };

            var matches = oregex.Matches(toBeSearched);
            Assert.AreEqual(matches.Count, 2);
            foreach (var m in matches)
            {
                Console.WriteLine("Match at {0} found:\t{1}",m.Index,string.Join(",", m.Values));
            }
            //OUTPUT:
            //Match at 8 found:     12,3,5,76,8,0,6,125
            //Match at 26 found:    12,3,5,76,8,0,6,125
        }
        public enum SemanticType
        {
            Name,
            FamilyName,
            Other,
        }

        public class Word
        {
            public readonly string Value;
            public readonly SemanticType SemType;

            public Word(string value, SemanticType semType)
            {
                Value = value;
                SemType = semType;
            }
        }

        public class Person
        {
            public readonly Word[] Words;
            public readonly string Name;
            public Person(OMatch<Word> match)
            {
                Words = match.Values.ToArray();
                Name = match.Captures["name"].First().Values.First().Value;
                //Now just normalize this name and you are good.
            }
        }

        private static bool IsInitial(string str)
        {
            var inp = str.Trim('.', ' ', '\t', '\n', '\r');
            return inp.Length == 1 && char.IsUpper(inp[0]);
        }
    }
}
