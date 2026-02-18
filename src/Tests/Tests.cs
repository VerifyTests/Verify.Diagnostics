using VerifyTests.OpenTelemetry;

public class Tests
{
    #region Usage

    [Fact]
    public Task Usage()
    {
        Recording.Start();
        using var source = new ActivitySource("TestSource");
        RecordingActivityListener.Start();

        using (var activity = source.StartActivity("MyOperation"))
        {
            activity!.SetTag("key1", "value1");
            activity.SetTag("key2", 42);
        }

        RecordingActivityListener.Stop();
        return Verify("result");
    }

    #endregion

    [Fact]
    public Task ActivityWithEvents()
    {
        Recording.Start();
        using var source = new ActivitySource("TestSource.Events");
        RecordingActivityListener.Start();

        using (var activity = source.StartActivity("OperationWithEvents"))
        {
            activity!.AddEvent(new("EventOne"));
            activity.AddEvent(new("EventTwo", default,
                new()
                {
                    {
                        "eventKey", "eventValue"
                    }
                }));
        }

        RecordingActivityListener.Stop();
        return Verify("result");
    }

    [Fact]
    public Task ActivityWithStatus()
    {
        Recording.Start();
        using var source = new ActivitySource("TestSource.Status");
        RecordingActivityListener.Start();

        using (var activity = source.StartActivity("OperationWithStatus"))
        {
            activity!.SetStatus(ActivityStatusCode.Error, "Something went wrong");
        }

        RecordingActivityListener.Stop();
        return Verify("result");
    }

    [Fact]
    public Task ActivityKinds()
    {
        Recording.Start();
        using var source = new ActivitySource("TestSource.Kinds");
        RecordingActivityListener.Start();

        using (var activity = source.StartActivity("ServerOperation", ActivityKind.Server))
        {
            activity!.SetTag("http.method", "GET");
        }

        RecordingActivityListener.Stop();
        return Verify("result");
    }

    [Fact]
    public Task ActivityWithLinks()
    {
        Recording.Start();
        using var source = new ActivitySource("TestSource.Links");
        RecordingActivityListener.Start();

        var linkedContext = new ActivityContext(
            ActivityTraceId.CreateRandom(),
            ActivitySpanId.CreateRandom(),
            ActivityTraceFlags.Recorded);

        var links = new List<ActivityLink>
        {
            new(linkedContext)
        };

        using (source.StartActivity("OperationWithLinks", ActivityKind.Internal, default(ActivityContext), null, links))
        {
        }

        RecordingActivityListener.Stop();
        return Verify("result");
    }

    [Fact]
    public Task ActivityWithBaggage()
    {
        Recording.Start();
        using var source = new ActivitySource("TestSource.Baggage");
        RecordingActivityListener.Start();

        using (var activity = source.StartActivity("OperationWithBaggage"))
        {
            activity!.AddBaggage("baggage1", "value1");
            activity.AddBaggage("baggage2", "value2");
        }

        RecordingActivityListener.Stop();
        return Verify("result");
    }

    [Fact]
    public Task Empty()
    {
        Recording.Start();
        using var source = new ActivitySource("TestSource.Empty");
        RecordingActivityListener.Start();

        // No activities created

        RecordingActivityListener.Stop();
        return Verify("result");
    }

    [Fact]
    public Task SourceFilter()
    {
        Recording.Start();
        using var source1 = new ActivitySource("AllowedSource");
        using var source2 = new ActivitySource("BlockedSource");
        RecordingActivityListener.Start(s => s.Name == "AllowedSource");

        using (var activity = source1.StartActivity("AllowedOperation"))
        {
            activity!.SetTag("allowed", true);
        }

        using (source2.StartActivity("BlockedOperation"))
        {
        }

        RecordingActivityListener.Stop();
        return Verify("result");
    }
}
