namespace VerifyTests;

public static class VerifyOpenTelemetry
{
    public static bool Initialized { get; private set; }

    public static void Initialize()
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;

        InnerVerifier.ThrowIfVerifyHasBeenRun();
        VerifierSettings.AddExtraSettings(settings =>
        {
            var converters = settings.Converters;
            converters.Add(new ActivityConverter());
            converters.Add(new ActivityEventConverter());
            converters.Add(new ActivityLinkConverter());
            converters.Add(new ActivityContextConverter());
        });
    }
}
