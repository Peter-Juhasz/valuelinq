# ValueLinq
LINQ implementation using value typed enumerators.

![.NET Core](https://github.com/Peter-Juhasz/valuelinq/workflows/.NET%20Core/badge.svg)

## Introduction
The goal of this project is to provide an option to reduce the number of heap allocations (in best case completely avoid) while you can still preserve the convenience of using LINQ-like operators.

By default, enumerables and their enumerator pairs are reference types. So, every time you call a `.Select(d => d.Id)` or a `.Where(u => u.Enabled)`, you allocate at least one or two instances to the heap. If you work in a performance critical environment, like measuring samples at a high frequency, or reading data as a server, you have two choices:
 1. Write a specialized algorithm the iterative way, so you can decide what to put where to avoid allocations, but also lose the readability and maintainability compared to a functional implementation what LINQ is good at.
 2. Use LINQ but allocate to the heap in each itaration, literally producing garbage, putting high pressure on GC.

`IEnumerable<T>` and `IEnumerator<T>` are interfaces, which means that even if you would have a value type (`struct`) implementation that implements those interfaces, but you would refer to the instance as the interface, you would end up with boxing, and create a reference on the heap anyway. But less people know, that if you have a `foreach` statement or either use the [query syntax](https://docs.microsoft.com/en-us/dotnet/csharp/linq/query-expression-basics), the C# compiler doesn't require the collection to implement those interfaces, but uses [patterns](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/statements#the-foreach-statement) instead. So even if the type doesn't implement the interface, but implements an appropriate `GetEnumerator()` method, it is able to use that. Also, it is smart enough to choose a value type implementation over the interface implementation, if you have one. For this reason, many of the built-in types (e.g. [StringTokenizer](https://github.com/dotnet/runtime/blob/master/src/libraries/Microsoft.Extensions.Primitives/src/StringTokenizer.cs)) use value type enumerators to avoid allocations.

So, this project provides you a (limited) 3rd option where you can have the advantages of both worlds: write more readable, functional code, while not having to worry about allocations.

## Example
The extensions methods are in the `System.Linq` namespace, but they are all prefixed with `Value`.

You can use them in high performance scenarios, like parsing GPS data from a CSV-like (NMEA) format. The following code snippet is easy to read and maintain, while it is allocation free and parses each line only once.

```cs
if (line.StartsWith("$GPGGA"))
{
    var parts = new StringTokenizer(line, Separators);
    foreach (var (index, part) in parts.ValueSelect((s, i) => (index: i, part: s)))
    {
        switch (index)
        {
            case 2:
                Double.TryParse(part, NumberStyles.Float, CultureInfo.InvariantCulture, out latitude);
                break;

            case 4:
                Double.TryParse(part, NumberStyles.Float, CultureInfo.InvariantCulture, out longitude);
                break;

            case 6:
                Int32.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out quality);
                break;
        }
    }
}
```

Or you can use them with regular reference type collections, like this:

```cs
foreach (var item in products.ValueWhere(p => p.Price < 1000))
{
	Console.WriteLine(item.Name);
}
```

## Supported operators

| Operator      | IEnumerable&lt;T&gt; (references) | StringTokenizer | ReadOnlySpan&lt;T&gt; |
|---------------|-----------------------------------|-----------------|-----------------------|
| `Append`     | X |   |   |
| `Concat`		| X |	|   |
| `Except`		| X |	|   |
| `Intersect`	| X |	|   |
| `Join`		| X |	|   |
| `Prepend`		| X |	|   |
| `Select`		| X | X | X |
| `SelectMany`	| X |	|   |
| `Skip`		| X |	|   |
| `SkipWhile`	| X |	|   |
| `Take`		| X |	|   |
| `TakeWhile`	| X |	|   |
| `Where`		| X | X |   |
| `Zip`			| X |	|   |

Support is planned for `T[]`, `ReadOnlySpan<T>` and `ReadOnlySequence<T>` as well.

## Benchmarks

Each test has the following three implementations to compare:
 - using built-in LINQ (built on reference types) *(usually with most allocations)*
 - using the value type enumerators of this project *(usually with less allocations, sometimes faster, sometimes slower)*
 - custom implementation of an algorithm, that uses no LINQ-like functions *(usually the fastest, with minimum allocations)*

### IEnumerable&lt;T&gt; references

|          Method |       Mean |     Error |    StdDev |      Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------- |-----------:|----------:|----------:|-----------:|------:|------:|----------:|
|          Select | 150.919 ns | 2.9182 ns | 2.7297 ns |     0.0076 |     - |     - |      48 B |
| **ValueSelect** | 124.028 ns | 0.4206 ns | 0.3935 ns | **0.0050** |     - |     - |  **32 B** |
| SelectIterative |   9.359 ns | 0.0268 ns | 0.0238 ns |          - |     - |     - |         - |
|           Where |  86.296 ns | 0.2998 ns | 0.2658 ns |     0.0076 |     - |     - |      48 B |
|  **ValueWhere** | 121.756 ns | 0.5245 ns | 0.4906 ns | **0.0050** |     - |     - |  **32 B** |
|  WhereIterative |  12.382 ns | 0.0485 ns | 0.0453 ns |          - |     - |     - |         - |

### StringTokenizer

|          Method |     Mean |   Error |  StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------- |---------:|--------:|--------:|-------:|------:|------:|----------:|
|          Select | 454.9 ns | 3.62 ns | 3.39 ns | 0.0253 |     - |     - |     160 B |
| **ValueSelect** | 276.3 ns | 0.40 ns | 0.35 ns |  **-** |     - |     - |     **-** |
| SelectIterative | 208.7 ns | 1.04 ns | 0.87 ns |      - |     - |     - |         - |
|           Where | 474.6 ns | 0.56 ns | 0.53 ns | 0.0267 |     - |     - |     168 B |
|  **ValueWhere** | 297.7 ns | 1.71 ns | 1.34 ns |  **-** |     - |     - |     **-** |
|  WhereIterative | 220.7 ns | 0.44 ns | 0.41 ns |      - |     - |     - |         - |

Measured on:
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.329 (2004/?/20H1)
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores

## Limitations
 - Chaining operators may result in boxing.
