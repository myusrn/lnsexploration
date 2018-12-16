using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

// "The type or namespace name 'AspNetCore' does not exist in the namespace 'Microsoft' (are you missing an assembly reference?)"
// "The type or namespace name 'Logging' does not exist in the namespace 'Microsoft.Extensions' (are you missing an assembly reference?)"
// fix was dependencies | manage nuget packages | add Microsoft.AspNetCore, Microsoft.AspNetCore.Mvc.Core, Microsoft.Azure.WebJobs
// which all seem to be required as a result of FunctionControllerUnitTest.cs because if you exclude it things build w/o the above errors

namespace xUnit.Tests
{
    /// <summary>
    /// Azure Functions App function unit test helper.
    /// </summary>
    /// <remarks>
    /// see "unit testing azure functions v2" -> https://medium.com/@tsuyoshiushio/writing-unit-test-for-azure-durable-functions-80f2af07c65e
    /// and "moq ilogger log" -> https://stackoverflow.com/questions/43424095/how-to-unit-test-with-ilogger-in-asp-net-core
    /// </remarks>
    public abstract class FunctionControllerUnitTest
    {
        //protected IPrincipal principal = Mock.Of<IPrincipal>(); // IPrincipal doesn't expose .Claims and other ClaimsPrincipal concrete class added properties and methods
        protected ClaimsPrincipal principal; // = Mock.Of<ClaimsPrincipal>;// don't need to mock as it has no outside dependencies and you can created it un-mocked:

        //protected ILogger<FunctionUnitTest> log = Mock.Of<ILogger<FunctionUnitTest>>();
        protected ILogger log = Mock.Of<ILogger>();

        public FunctionControllerUnitTest()
        {
            //(principal as Mock<ClaimsPrincipal>).Setup(principal => principal.IsInRole("Proprietry")).Returns(true);
            var claims = new List<Claim>() { new Claim(ClaimTypes.Name, "username"), new Claim(ClaimTypes.NameIdentifier, "userId"), new Claim(ClaimTypes.Role, "Commodity") };
            var identity = new ClaimsIdentity(claims, "AuthenticationTypes.Federation");
            principal = new ClaimsPrincipal(identity);
        }

        public void MockHttpContext(ControllerBase controller)
        {
// mock asp.net core controller context -> https://stackoverflow.com/questions/41400030/mock-httpcontext-for-unit-testing-a-net-core-mvc-controller
// mock HttpContext.User -> https://stackoverflow.com/questions/40230776/how-to-mock-httpcontext-user
// setting httpcontext on unit test of controller -> https://stackoverflow.com/questions/2497575/asp-net-mvc-unit-test-controller-with-httpcontext

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //controller.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";
            controller.ControllerContext.HttpContext.User = principal; // which already has unit test relevant .User ClaimsPrincipal attached
        }

        public HttpRequest HttpRequestSetup(Dictionary<String, StringValues> query, string body)
        {
            var reqMock = new Mock<HttpRequest>();

            reqMock.Setup(req => req.Query).Returns(new QueryCollection(query));
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(body);
            writer.Flush();
            stream.Position = 0;
            reqMock.Setup(req => req.Body).Returns(stream);
            return reqMock.Object;
        }
    }

    public class AsyncCollector<T> : IAsyncCollector<T>
    {
        public readonly List<T> Items = new List<T>();

        public Task AddAsync(T item, CancellationToken cancellationToken = default(CancellationToken))
        {
            Items.Add(item);
            return Task.FromResult(true);
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(true);
        }
    }
}
