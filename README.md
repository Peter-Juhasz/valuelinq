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

| Operator      | Enumerables (references) | StringTokenizer |
|---------------|--------------------------|-----------------|
| `Append`     |     X                    |                 |
| `Concat`		|     X                    |				 |
| `Except`		|     X                    |				 |
| `Intersect`	|     X                    |				 |
| `Join`		|     X                    |				 |
| `Prepend`		|     X                    |				 |
| `Select`		|     X                    |		X   	 |
| `SelectMany`	|     X                    |				 |
| `Skip`		|     X                    |				 |
| `SkipWhile`	|     X                    |				 |
| `Take`		|     X                    |				 |
| `TakeWhile`	|     X                    |				 |
| `Where`		|     X                    |		X		 |
| `Zip`			|     X                    |				 |

Support is planned for `ReadOnlySpan<T>` and `ReadOnlySequence<T>` as well.

## Benchmarks

### IEnumerable&lt;T&gt; references

|      Method |     Mean |   Error |  StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------ |---------:|--------:|--------:|-------:|------:|------:|----------:|
|      Select | 156.2 ns | 0.77 ns | 0.60 ns | 0.0076 |     - |     - |      48 B |
| ValueSelect | 138.2 ns | 0.24 ns | 0.19 ns | 0.0050 |     - |     - |  **32 B** |
|       Where | 100.9 ns | 1.81 ns | 1.51 ns | 0.0076 |     - |     - |      48 B |
|  ValueWhere | 121.5 ns | 1.61 ns | 1.50 ns | 0.0050 |     - |     - |  **32 B** |

### StringTokenizer

|      Method |     Mean |   Error |  StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------ |---------:|--------:|--------:|-------:|------:|------:|----------:|
|      Select | 455.4 ns | 0.55 ns | 0.43 ns | 0.0253 |     - |     - |     160 B |
| ValueSelect | 273.2 ns | 0.30 ns | 0.26 ns |      - |     - |     - |     **-** |
|       Where | 477.5 ns | 0.66 ns | 0.58 ns | 0.0267 |     - |     - |     168 B |
|  ValueWhere | 280.3 ns | 1.48 ns | 1.31 ns |      - |     - |     - |     **-** |

Measured on:
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.329 (2004/?/20H1)
Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores

## Limitations
 - Chaining operators may result in boxing.
