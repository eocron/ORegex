# ORegex
Object oriented Regular Expressions implementation.

This implementation based on original Microsoft Regular Expression Syntax and will follow it as much as possible.
To declare predicate in pattern you type:

    {myPredicateName}

...and feed ORegex<T> predicate table. 

PredicateTable<T> is simple key value dictionary for predicates.
Predicate tables can accept lambda's and comparer's (IEqualityComparer<T>) with values.
Each lambda or value should have unique name inside pattern.

#Example

    var pattern = "{0}.{0}+{1}(?<someGroup>{0}{3}){2,}";
    ObjectRegex<T> regex = new ObjectRegex<T>(pattern,IsPred0, IsPred1, IsPred2, IsPred3, ...);
    var input = new T[] { ... };
    var matches = regex.Matches(input);
    var match = regex.Match(input);
    var someGroup = match.Captures["someGroup"].Value;
    if(regex.IsMatch(input))
    {
      ...
    }
    
You can start from viewing Unit Test project to see how you can use it, by time there will be more examples. 
Also, you can find there test utility and see how things work inside engine.

#Performance

- Compared to real life cases performance is 2-3 times slower than .NET Regex engine on character sequences.
- Greedy exhausting test ('x+x+y+' pattern on a 'xxxxxxxxxxxxxxxxxxxx' string) is ~20 times slower.

#Future

- Lookahead support.