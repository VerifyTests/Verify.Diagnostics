# <img src="/src/icon.png" height="30px"> Verify.Diagnostics

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://img.shields.io/appveyor/build/SimonCropp/verify-diagnostics)](https://ci.appveyor.com/project/SimonCropp/verify-diagnostics)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.Diagnostics.svg)](https://www.nuget.org/packages/Verify.Diagnostics/)

Extends [Verify](https://github.com/VerifyTests/Verify) to allow verification of [System.Diagnostics.Activity](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activity)<!-- singleLineInclude: intro. path: /docs/intro.include.md -->

**See [Milestones](../../milestones?state=closed) for release notes.**


## Sponsors


### Entity Framework Extensions<!-- include: zzz. path: /docs/zzz.include.md -->

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.Diagnostics) is a major sponsor and is proud to contribute to the development this project.

[![Entity Framework Extensions](https://raw.githubusercontent.com/VerifyTests/Verify.Diagnostics/refs/heads/main/docs/zzz.png)](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.Diagnostics)<!-- endInclude -->


## NuGet

 * https://nuget.org/packages/Verify.Diagnostics


## Usage

<!-- snippet: Enable -->
<a id='snippet-Enable'></a>
```cs
[ModuleInitializer]
public static void Initialize() =>
    VerifyDiagnostics.Initialize();
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L3-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-Enable' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

`RecordingActivityListener` allows, when a method is being tested, for any [Activity](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.activity) created as part of that method call to be recorded and verified.

Call `RecordingActivityListener.Start()` to begin listening. All activities from any `ActivitySource` will be captured by default.

<!-- snippet: Usage -->
<a id='snippet-Usage'></a>
```cs
[Fact]
public Task Usage()
{
    Recording.Start();
    using var source = new ActivitySource("TestSource");

    using (var activity = source.StartActivity("MyOperation"))
    {
        activity!.SetTag("key1", "value1");
        activity.SetTag("key2", 42);
    }

    return Verify("result");
}
```
<sup><a href='/src/Tests/Tests.cs#L3-L20' title='Snippet source file'>snippet source</a> | <a href='#snippet-Usage' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Results in:

<!-- snippet: Tests.Usage.verified.txt -->
<a id='snippet-Tests.Usage.verified.txt'></a>
```txt
{
  target: result,
  activity: {
    MyOperation: {
      Tags: {
        key1: value1,
        key2: 42
      }
    }
  }
}
```
<sup><a href='/src/Tests/Tests.Usage.verified.txt#L1-L11' title='Snippet source file'>snippet source</a> | <a href='#snippet-Tests.Usage.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Serialization

Activities are serialized with the following conventions:

 * `OperationName` is used as the JSON property key
 * `DisplayName` only included if different from `OperationName`
 * `Kind` only included if not `Internal`
 * `Status` and `StatusDescription` only included if not `Unset`
 * `Tags`, `Events`, `Links`, and `Baggage` included when present
 * Non-deterministic values (`Id`, `TraceId`, `SpanId`, `ParentSpanId`, `Duration`, `StartTimeUtc`, `Source`) are omitted


## Icon

[Diagnostic](https://thenounproject.com/icon/diagnostic-8246832/) from [The Noun Project](https://thenounproject.com).
