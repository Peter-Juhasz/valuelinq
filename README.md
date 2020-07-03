# ValueLinq
LINQ implementation using value typed enumerators.

# Introduction
The goal of this project is to provide an option to reduce the number of heap allocations while you can still preserve the convenience of using LINQ-like operators.

By default, enumerables and their enumerator pairs are reference types. So, every time you call a `.Select(d => d.Id)` or a `.Where(u => u.Enabled)`, you allocate at least one or two instances to the heap. If you work in a performance critical environment, like measuring samples at a high frequency, or reading data as a server, you have two choices:
 1. Write a specialized algorithm the iterative way, so you can decide what to put where to avoid allocations, but also lose the readability and maintainability compared to a functional implementation what LINQ is good at.
 2. Use LINQ but allocate to the heap in each itaration, literally producing garbage, putting high pressure on GC.

`IEnumerable<T>` and `IEnumerator<T>` are interfaces, which means that even if you would have a value type (`struct`) implementation that implements those interfaces, but you would refer to the instance as the interface, you would end up with boxing, and create a reference on the heap anyway. But less people know, that if you have a `foreach` statement or either use the [query syntax](https://docs.microsoft.com/en-us/dotnet/csharp/linq/query-expression-basics), the C# compiler doesn't require collection to implement those interfaces, but uses [patterns](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/statements#the-foreach-statement) instead. So even if the type doesn't implement the interface, but implements an appropriate `GetEnumerator()` method, it is able to use that. Also, it is smart enough to choose a value type implementation over the interface implementation, if you have one. For this reason, many of the built-in types (e.g. [StringTokenizer](https://github.com/dotnet/runtime/blob/master/src/libraries/Microsoft.Extensions.Primitives/src/StringTokenizer.cs)) use value type enumerators to avoid allocations.

So, this project provides you a (limited) 3rd option where you can have the advantages of both worlds: write more readable, functional code, while not having to worry about allocations.

# Example
The extensions methods are in the `System.Linq` namespace, but they are all prefixed with `Value`.

```cs
foreach (var item in products.ValueWhere(p => p.Price < 1000))
{
	Console.WriteLine(item.Name);
}
```

# Supported operators
 - `Select`
 - `SelectMany`
 - `Take`
 - `TakeWhile`
 - `Where`

# Limitations
 - Some combinations of operator chains may not be implemented yet, so they may use boxing.