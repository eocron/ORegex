# ORegex
Object oriented Regular Expressions implementation.

This implementation based on original Microsoft Regular Expression Syntax and will follow it as much as possible.
To declare predicate in pattern you type:

    {myPredicateName}

...and feed ORegex predicate table. 

PredicateTable is simple key value dictionary for predicates.
Predicate tables can accept lambda's and comparer's (IEqualityComparer<T>) with values.
Each lambda or value should have unique name inside pattern.

### Features
- Default regex engine support;
- Capture groups support;
- Greedy/Lazy support;
- Backtracking DFA reduce support;
- Exact begin/end match support.

### Example

Simple prime sequence test:

        public static bool IsPrime(int number)
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

        public void PrimeTest()
        {
            var oregex = new ORegex<int>("{0}(.{0})*", IsPrime);
            var input = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
            foreach (var match in oregex.Matches(input))
            {
                Trace.WriteLine(string.Join(",", match.Values));
            }
        }

        //OUTPUT:
        //2
        //3,4,5,6,7
        //11,12,13

Or more complex test from NLP field:

        public void PersonSelectionTest()
        {
            //INPUT_TEXT: Ïÿòî÷êîâà Òàìàðà ðåøèëà âûãóëÿòü Äæåêà è âñòðåòèëàñü ñ Ìèõàèëîì À.Ì.
            var sentence = new Word[]
            {
                new Word("Ïÿòî÷êîâà", SemanticType.FamilyName),
                new Word("Òàìàðà", SemanticType.Name),
                new Word("ðåøèëà", SemanticType.Other),
                new Word("âûãóëÿòü", SemanticType.Other),
                new Word("Äæåêà", SemanticType.Name),
                new Word("è", SemanticType.Other),
                new Word("âñòðåòèëàñü", SemanticType.Other),
                new Word("ñ", SemanticType.Other),
                new Word("Ìèõàèëîì", SemanticType.Name),
                new Word("À.", SemanticType.Other),
                new Word("Ì", SemanticType.Other),
            };

            //Creating table which will contain our predicates.
            var pTable = new PredicateTable<Word>();
            pTable.AddPredicate("Ôàìèëèÿ", x => x.SemType == SemanticType.FamilyName);  //Check if word is FamilyName.
            pTable.AddPredicate("Èìÿ", x => x.SemType == SemanticType.Name);            //Check if word is simple Name.
            pTable.AddPredicate("Èíèöèàë", x => IsInitial(x.Value));                    //Complex check if Value is Inital character.

            var oregex = new ORegex<Word>(@"
                {Ôàìèëèÿ}(?<name>{Èìÿ})                     //Comments can written inside pattern...
                |
                (?<name>{Èìÿ})({Ôàìèëèÿ}|{Èíèöèàë}{1,2})?  /*...even complex ones.*/
            ", pTable);

            var persons = oregex.Matches(sentence).Select(x => new Person(x)).ToArray();

            foreach (var person in persons)
            {
                Console.WriteLine("Person found: {0}, length: {1}", person.Name, person.Words.Length);
            }

            //OUTPUT:
            //Person found: Òàìàðà, length: 2
            //Person found: Äæåêà, length: 1
            //Person found: Ìèõàèëîì, length: 3
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
                Name = match.OCaptures["name"].First().Values.First().Value;
                //Now just normalize this name and you are good.
            }
        }

        private static bool IsInitial(string str)
        {
            var inp = str.Trim(new[] { '.', ' ', '\t', '\n', '\r' });
            return inp.Length == 1 && char.IsUpper(inp[0]);
        }


You can start from viewing Unit Test project to see how you can use it, by time there will be more examples. 
Also, you can find there test utility and see how things work inside engine.

### Performance
- Compared to real life cases performance is 2-3 times slower than .NET Regex engine on character sequences;
- Greedy exhausting test (x+x+y+ pattern on a 'xxxxxxxxxxxxxxxxxxxx' string) is ~10 times faster than Regex engine. This result achieved due internal ORegex optimizations so pattern x+x+x+ becomes xxx+ and unnecessary backtrack avoided.

### Future
- Reverse search support;
- C/C++ macros definition support;
- Overlap capture support;
- [{Positive}{Negative}] Look[{ahead}{behind}] support;
