# Brutus

A simple brute force library for C#

## Dependencies

The library has been compiled with .NET 4.0. 

It should run also with older versions of .NET, due the usage of LINQ at least .NET 3.5 is required (to use LINQ with older .NET versions see this [stackoverflow post](http://stackoverflow.com/questions/3348348/is-there-a-way-to-use-linq-query-syntax-with-net-3-0-projects "Is there a way to use LINQ query syntax with .NET 3.0 projects?")).

## Usage

The main component is the ***Executor*** class. It requires a keyspace which is basically an array of chars. If ***Executor.ComputeNextKey*** gets called, it will compute the next key and store its current state.

```csharp
  char[] keyspace = new char[] {'a', 'b', 'c'};
  // the executor gets initialized with a selected keyspace
  Executor executor = new Executor(keyspace);
  // computes a key based on your keyspace and also stores the current state
  // will print 'a'
  Console.WriteLine(executor.ComputeNextKey());
  // will print 'b'
  Console.WriteLine(executor.ComputeNextKey());
  // will print 'c'
  Console.WriteLine(executor.ComputeNextKey());
  // will print 'aa', and so on ...
  Console.WriteLine(executor.ComputeNextKey());
```
 
The solution contains also an console program which shows a basic implementation.
