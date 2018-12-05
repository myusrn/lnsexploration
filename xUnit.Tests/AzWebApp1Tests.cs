using System;
using System.Net.Http;
using Xunit;

namespace xUnit.Tests
{
    public class AzWebApp1Tests
    {
        [Fact]
        public async void Call_MyWebApi_ReturnsValues()
        {
// sep18 .net conference keynote callout of new simple to adopt http client retry policies story covered in 8min – 12min mark https://www.youtube.com/watch?v=qQdGC2jIP8s&t=8m0s
// httpclient retry policy c# -> https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly
// and https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-2.2#use-polly-based-handlers
            var httpClient = new System.Net.Http.HttpClient();
            //add parameter via dependency injection ([FromServices]IHttpClientFactory factory, . . . )
            //var httpClient = factory.CreateClient("api");
            System.Net.Http.HttpResponseMessage response;
            var expected = "foobar";
            var actual = string.Empty;
            try
            {
                const string url = "https://localhost:44373/api/values";
                const string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6IndVTG1ZZnNxZFF1V3RWXy1oeFZ0REpKWk00USJ9.eyJhdWQiOiI3OGNhODUyYS05NTlkLTQwMWUtYTAyNS1jNjY1ZjY2OTZhMDQiLCJpc3MiOiJodHRwczovL2xvZ2luLm1pY3Jvc29mdG9ubGluZS5jb20vOTJlMzRlMmYtYzI5OC00MDA2LThiMjItMWMyNWY5NGMyNDU2L3YyLjAiLCJpYXQiOjE1NDM5ODM4MjYsIm5iZiI6MTU0Mzk4MzgyNiwiZXhwIjoxNTQzOTg3NzI2LCJhaW8iOiJBVlFBcS84SkFBQUFVSVd5K1k2UEdBM0J3VjZTK0FxbU9zclhtQmhaY2xXakxmV3N6eGlDM2pSQmZTQzZYRW1SaHF0RUFOWndIYTUwNlVUT1pBNHhpYmNVWE5NNHBYQStOMjcwYmsyNk9admhNRFB3c2cyRmFjcz0iLCJhenAiOiI2ZTk1NjU2Ny1lZTJjLTQxNTctOTFiYi1hNGRkY2VkZTY2N2MiLCJhenBhY3IiOiIwIiwiaWRwIjoibGl2ZS5jb20iLCJuYW1lIjoib2Igb25lIiwib2lkIjoiNzJjNGY5MGEtNjE2NC00ZjMwLWJmYTQtODI3YjdlZTM2MmFiIiwicHJlZmVycmVkX3VzZXJuYW1lIjoib2IxQG91dGxvb2suY29tIiwicm9sZXMiOlsiQ29tbW9kaXR5Il0sInNjcCI6ImFjY2Vzc19hc191c2VyIiwic3ViIjoiUTd4MDZ3eWJnc1pPVXhzUTFNVFhrZUdoaG96bi10WHBPZFAwUjl6VS1XVSIsInRpZCI6IjkyZTM0ZTJmLWMyOTgtNDAwNi04YjIyLTFjMjVmOTRjMjQ1NiIsInV0aSI6IkE3NGdHNS1MWWtxdURYZnpxM1djQUEiLCJ2ZXIiOiIyLjAifQ.CJNXnqDra_ZQ9SXC0dduRWgVxWQtBoqT7PTLU2a7P6chNnyEw1M4sc0jqpuz38BdyKWywYfsZzxcoLTlYKr0etETiPZYGNx__dP1ED1JRymQYEFLU2ZmfuEsb2iV7YlfrngwYU659jfScM46xaxKECqfSDHCb4f9D_rYd-sYCafMnG951sICW8mNZ1AqDbrSiicI63uNxTEtT8lfCEQEcXJyVEevegokp-8px1_zDT20OJ7R0zNuV-5V11YrslBnVUEmoLa4sdKNMpH_O-63KvE7__tJrRty_jYsnJa-ED9xPv3paD2qZH-i0wjBmRlYklKy5oJSKG04sIkBT5Q5PA";
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                //Add the token in Authorization header
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                actual =  content;
            }
            catch (Exception ex)
            {
                actual = ex.ToString();
            }

            Assert.Equal(expected, actual);
        }
    }
}
