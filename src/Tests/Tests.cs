public class Tests
{
    #region Usage

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

    #endregion

    [Fact]
    public Task ActivityWithEvents()
    {
        Recording.Start();
        using var source = new ActivitySource("TestSource.Events");

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

        return Verify("result");
    }

    [Fact]
    public Task ActivityWithStatus()
    {
        Recording.Start();
        using var source = new ActivitySource("TestSource.Status");

        using (var activity = source.StartActivity("OperationWithStatus"))
        {
            activity!.SetStatus(ActivityStatusCode.Error, "Something went wrong");
        }

        return Verify("result");
    }

    [Fact]
    public Task ActivityKinds()
    {
        Recording.Start();
        using var source = new ActivitySource("TestSource.Kinds");

        using (var activity = source.StartActivity("ServerOperation", ActivityKind.Server))
        {
            activity!.SetTag("http.method", "GET");
        }

        return Verify("result");
    }

    [Fact]
    public Task ActivityWithLinks()
    {
        Recording.Start();
        using var source = new ActivitySource("TestSource.Links");

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

        return Verify("result");
    }

    [Fact]
    public Task ActivityWithBaggage()
    {
        Recording.Start();
        using var source = new ActivitySource("TestSource.Baggage");

        using (var activity = source.StartActivity("OperationWithBaggage"))
        {
            activity!.AddBaggage("baggage1", "value1");
            activity.AddBaggage("baggage2", "value2");
        }

        return Verify("result");
    }

    [Fact]
    public Task Empty()
    {
        Recording.Start();
        using var source = new ActivitySource("TestSource.Empty");

        // No activities created

        return Verify("result");
    }
}
