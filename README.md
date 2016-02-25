# ORegex
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
    
You can start from viewing Unit Test project to see how you can use it (Example with number sequence), by time there will be more examples.

#Future

It will support:

1) Capture - this is very important for NLP sphere and many others where sequence is object itself. 

2) It will cure 'static calculation' bug which creates incorrect captures because of invalid mapping Object State to char symbols. It can't be cured without reimplementation of regex engine.

3) Replace - you will be able to change sequence based on retrieved match. For example:

    input:    first_name last_name verb noun dot;             //Jhon Smith watching tv.
    pattern:  (?<person>{first_name} {last_name}?);               //Jhon watching tv.        - possible form.
    replace:  $person;
    output:   person verb noun dot;

4) Lookahead and Greedy search.
