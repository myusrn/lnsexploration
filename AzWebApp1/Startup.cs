using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzWebApp1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private IHostingEnvironment env;
        private ILogger<Startup> log;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
                .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));

            // using Microsoft.Extensions.Http.PolicyHttpMessageHandler; -> IHttpClientBuilder.AddTransientHttpErrorPolicy ???
            //https://github.com/aspnet/HttpClientFactory/blob/master/src/Microsoft.Extensions.Http.Polly/PolicyHttpMessageHandler.cs
            //HttpClientFactory / src / Microsoft.Extensions.Http.Polly / PolicyHttpMessageHandler.cs
            // using Microsoft.Extensions.Http.Polly; -> IHttpClientBuilder.AddTransientHttpErrorPolicy ???
            //services.AddHttpClient("api").AddTransientHttpErrorPolicy(p => p.RetryAsync(6));

            // services.addlogging .net core 2.0 -> https://stackoverflow.com/questions/45781873/is-net-core-2-0-logging-broken
            //services.AddLogging(builder =>
            //{
            //    builder.AddConfiguration(Configuration.GetSection("Logging"))
            //        .AddConsole() // see appsettings.Development.json
            //        .AddDebug();
            //});

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> log)
        {
            this.env = env; this.log = log;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            //app.UseMiddleware<RandomFailureMiddleware>(); // added to enable random request/response failures to test httpclient retry policy
            app.UseMvc();
        }

        //public class RandomFailureMiddleware : IMiddleware
        //{
        //    private Random random;

        //    public RandomFailureMiddleware()
        //    {
        //        random = new Random();
        //    }

        //    public Task InvokeAsync(HttpContext context, RequestDelegate next)
        //    {
        //        if (random.NextDouble() >= 0.5)
        //        {
        //            throw new Exception("Computer says no.");
        //        }
        //        return next(context);
        //    }
        //}
    }
}
