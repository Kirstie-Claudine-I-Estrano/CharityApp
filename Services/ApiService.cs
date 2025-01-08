// In Services/ApiService.cs
public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetApiDataAsync(string url)
    {
        var response = await _httpClient.GetStringAsync(url);
        return response;
    }
}
