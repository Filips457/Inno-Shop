namespace UnitTests.FakeHttpHandler;

public class FakeHandler : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("OK")
        };
        return Task.FromResult(response);
    }
}