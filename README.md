# Eocron
Object oriented Regular Expressions implementation.
This is algorithm from pattern matching category.

This implementation based on original Microsoft Regular Expression Engine and will persist similar syntax and usage as much, as possible.
To declare predicate match in pattern you type {myPredicateName} and feed ObjectRegex<T> table with predicates.
Predicate tables can accept lambdas and comparers (IEqualityComparer<>) with values.
Each lambda or value should have unique name inside pattern.

#Example

    var pattern = "{0}.{0}+{1}(?<someGroup>{0}{3}){2,}";
    ObjectRegex<T> regex = new ObjectRegex<T>(pattern,IsPred0, IsPred1, IsPred2, IsPred3, ...);
    var input = new T[] { ... };
    var matches = regex.Matches(input);
    var match = regex.Match(input);
    var someGroup = match.Groups["someGroup"].Value;
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