# ORegex
Object oriented Regular Expressions implementation.

This implementation based on original Microsoft Regular Expression Syntax and will follow it as much as possible.
To declare predicate in pattern you type:

    {myPredicateName}

...and feed ORegex predicate table. 

PredicateTable is simple key value dictionary for predicates.
Predicate tables can accept lambda's and comparer's (IEqualityComparer<T>) with values.
Each lambda or value should have unique name inside pattern. Pattern can have as much whitespaces as you want, so you are free to format your patterns in fancy styles and even place it in separate files:

    {f}    {a}    {n}
    {c}     |     {y}

is the same as:
    
    {f}{a}{n}{c}|{y}


## Features
- Default regex engine support;
- Capture groups support;
- Greedy/Lazy support;
- Exact begin/end match support;
- RE2 algorithm with modifications (backtracking seriously cutted down);
- Reverse pattern/input support. Also, this includes RightToLeft option support;
- Negative/Positive look ahead/behind support.

## Syntax

###Concatenation

    {a}{b}{c}
    
###Repetition operators

    {a}?            - match zero or one time.
    {a}*            - match zero or any number of times.
    {a}+            - match at least one or greater number of times.
    {a}{n,}         - match at least 'n' times.
    {a}{n,m}        - match at least 'n' times but not greater than 'm' times.
    {a}{n,n}        - match exactly 'n' times.
    
*Important to mention, all of this operator's support 'lazy' modifier:*

    {a}??
    {a}*?
    {a}+?
    {a}{n,}?
    {a}{n,m}?
    {a}{n,n}?
    
###Groups

    ({a}{b})?{c}

This group will be available by index '1'. Index '0' is root match.

###Capture

    (?<groupName>{a}{b})?{c}
    
This group will be available by index '1' and name 'groupName'. Important to mention, you can write Russian names too. For example:

    (?<НазваниеГруппы>{a}{b})?{c}

###Look somewhere

    (?={a})         - positive lookahead.
    (?!{a})         - negative lookahead.
    (?<={a})        - positive lookbehind.
    (?<!{a})        - negative lookbehind.


## Example

Simple prime sequence test:
```cs
        public void PrimeTest()
        {
            var oregex = new ORegex<int>("{0}(.{0})*", IsPrime);
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
```    
Or more complex test from NLP field:
```cs
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
                {Фамилия}(?<name>{Имя})                     //Comments can be written inside pattern...
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
                Name = match.OCaptures["name"].First().Values.First().Value;
                //Now just normalize this name and you are good.
            }
        }

        private static bool IsInitial(string str)
        {
            var inp = str.Trim(new[] { '.', ' ', '\t', '\n', '\r' });
            return inp.Length == 1 && char.IsUpper(inp[0]);
        }
```

You can start from viewing Unit Test project to see how you can use it, by time there will be more examples. 
Also, you can find there test utility and see how things work inside engine.

## Performance
- ORegex is 2-3 times slower than original .NET Regex, however it is ~2 times faster on simple patterns without many repetitions.
- Greedy exhausting test (x+x+y+ pattern on a 'xxxxxxxxxxxxxxxxxxxx' string) is ~20 times faster than Regex engine. This result achieved due to double finite state automaton implementation (fast dfa lookup, slow nfa command flow on captures) so backtracking seriously cutted down.

## Future
- C/C++ macros definition support;
- Overlap capture support;
