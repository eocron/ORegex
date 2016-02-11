# ORegex
Object oriented Regular Expressions implementation. The syntax is the same as .NET Regex.

This implementation based on original Microsoft Regular Expression Engine and persist it's syntax and usage in C#.
To declare predicate match in pattern you type {number_of_predicate} and feed ObjectRegex<T> instance your predicates as arguments in constructor.

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

#Future

Current implementation based on originial Regex engine and very buggy, but it will be comletely rewritten to separate solution. Main features will be:

1) Multicapture - this is very important for NLP sphere and many others where sequence contain multiple conclusion's. For example:

    input: 1 2 3 4 5;
    pattern: (?<q1>{even} {noteven}) | (?<q2>{even} {prime});
    output: <q1>[2 3] <q2>[2 3] <q1>[4 5];

2) Parallel - it will be parallel with defined degree if some flag set.

3) It will cure 'static calculation' bug which creates incorrect captures because of invalid mapping Object State to char symbols. It can't be cured without reimplementation of regex engine.

4) Replace - you will be able to change sequence based on retrieved match. For example:

    input: first_name last_name verb word dot; //Jhon Smith watching tv.
    pattern: first_name last_name?;//Jhon watchin tv. - possible form.
    replace: person;
    output: person verb word dot;

As you see it is very simple. Yes, it can be done through grammars and syntax analyzers, but I can surely say: regex is more popular for such cases because of comactness, clearness and easy to use conception
