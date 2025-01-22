using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;
public class CashFlowClassFixture : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;

    public CashFlowClassFixture(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    protected async Task<HttpResponseMessage> DoPost(string requestUri, object request, string token = "", string cultureInfo = "en")
    {
        if(!string.IsNullOrWhiteSpace(token))
            AuthorizeRequest(token);

        ChangeRequestCulture(cultureInfo);

        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }

    private void AuthorizeRequest(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void ChangeRequestCulture(string cultureInfo)
    {
        // Fazer com que o header/culture fique vazia
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(cultureInfo));
    }
}

