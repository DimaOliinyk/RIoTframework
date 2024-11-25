using CourseWorkUI.Controller;
using CourseWorkUI.Utilities;
using System.Net.NetworkInformation;

namespace CourseWorkUI.Model;

public class CircuitModel
{
    public static List<PinModel> Pins { get; private set; } = new();
    public static string? IPAddress { get; set; }
    public static double TimeDelay { get; set; } = 1000.0;
    private static HttpClient _httpClient = new HttpClient();

    private static string result;

    public static async void Send(StringContent data)
    {
        if (IPAddress == null)
        {
            throw new ArgumentNullException(nameof(IPAddress) + " must be set first");
        }
        try
        {
            await _httpClient.PostAsync(IPAddress, data);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static async Task<string> StartDataChecking()
    {
        var tokenSource = new CancellationTokenSource();
        var token = tokenSource.Token;
        await IntervaledDataCheckingAsync(TimeSpan.FromMilliseconds(TimeDelay), token);
        return result;
    }

    private static async Task<string> IntervaledDataCheckingAsync(TimeSpan interval, CancellationToken cancellationToken)
    {
        string? res = null;
        while (AppState.IsRunning)
        {
            res = await CheckForDataAsync();
            await Task.Delay(interval, cancellationToken);
        }
        return result!;
    }

    private static async Task<string> CheckForDataAsync()
    {
        if (IPAddress == null)
        {
            throw new ArgumentNullException(nameof(IPAddress) + " must be set first");
        }
        result = await _httpClient.GetStringAsync(IPAddress + "/G");
        CircuitInterpreter.Decode(result);
        return result;
    }
}
