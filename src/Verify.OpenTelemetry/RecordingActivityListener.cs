namespace VerifyTests.OpenTelemetry;

public static class RecordingActivityListener
{
    static ActivityListener? listener;

    public static void Start(Func<ActivitySource, bool>? sourceFilter = null)
    {
        listener = new()
        {
            ShouldListenTo = sourceFilter ?? (_ => true),
            Sample = (ref ActivityCreationOptions<ActivityContext> _) =>
                ActivitySamplingResult.AllDataAndRecorded,
            ActivityStopped = activity => Recording.Add("activity", activity)
        };
        ActivitySource.AddActivityListener(listener);
    }

    public static void Stop()
    {
        listener?.Dispose();
        listener = null;
    }
}
