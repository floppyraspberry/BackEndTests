using System.Net;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace ApiTester;

public class Program
{
    protected readonly HttpClient Client;
    private readonly Uri Url;

    public Program()
    {
        Client = new HttpClient();
        var getUrl = Environment.GetEnvironmentVariable("url");
        Url = new(getUrl);
    }

    [Fact]
    public async void GetToken()
    {
        try
        {
            var client = new HttpClient();
            var userName = Environment.GetEnvironmentVariable("username");
            var password = Environment.GetEnvironmentVariable("password");

            var encoded = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", userName, password)));

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encoded);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await Client.PostAsync(Url, null);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = response.Content.ReadAsStringAsync();
            var actual = JsonConvert.DeserializeObject<AuthenticationResponse>(content.Result);
            Assert.NotEmpty(actual.Token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [Fact]
    public async void Test()
    {
        return;
    }

    public class AuthenticationResponse
    {
        public string Token { get; set; }
    }
}