# ORegex
Object oriented Regular Expressions implementation.

This implementation based on original Microsoft Regular Expression Syntax and will follow it as much as possible.
To declare predicate in pattern you type:

    {myPredicateName}

...and feed ORegex<T> predicate table. 

PredicateTable<T> is simple key value dictionary for predicates.
Predicate tables can accept lambda's and comparer's (IEqualityComparer<T>) with values.
Each lambda or value should have unique name inside pattern.

### Features
- Default regex engine support;
- Capture groups support;
- Greedy/Lazy support;
- Backtracking DFA reduce support;
- Exact begin/end match support.

### Example
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
