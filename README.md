# ORegex
Object oriented Regular Expressions implementation. The syntax is the same as .NET Regex.

This implementation based on original Microsoft Regular Expression Engine and persist it's syntax and usage in C#.
To declare predicate match in pattern you type {number_of_predicate} and feed ObjectRegex<T> instance you predicate as one of arguments in constructor.

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
    
You can start from viewing Unit Test porject and how you can use it (Example with number sequence), by time there will be more examples.
