
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

            var input = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
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

            var input = new int[] { 0, 0, 0, 1, 0, 2, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0 };
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
            var sentence = new Word[]
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

            //OUTPUT:
            //Person found: Тамара, length: 2
            //Person found: Джека, length: 1
            //Person found: Михаилом, length: 3
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
            var inp = str.Trim(new[] { '.', ' ', '\t', '\n', '\r' });
            return inp.Length == 1 && char.IsUpper(inp[0]);
        }
    }
}
