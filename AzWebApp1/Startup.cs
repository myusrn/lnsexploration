using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Authentication.JwtBearer; // quickstart sample provided story 
using Microsoft.AspNetCore.Authentication; // project template based story, enables .AddAzureADBearer() extension method
using Microsoft.AspNetCore.Authentication.AzureAD.UI; // project template based story
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Authentication.OpenIdConnect;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc.Authorization;

namespace AzWebApp1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // quickstart sample provided story, uncomment associated using statement above
            //services.AddAuthentication(sharedOptions => { sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
            //    .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));

            // project template based story, uncomment associated using statement above
            services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
                .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));

            // using Microsoft.Extensions.Http.PolicyHttpMessageHandler; -> IHttpClientBuilder.AddTransientHttpErrorPolicy ???
            //https://github.com/aspnet/HttpClientFactory/blob/master/src/Microsoft.Extensions.Http.Polly/PolicyHttpMessageHandler.cs
            //HttpClientFactory / src / Microsoft.Extensions.Http.Polly / PolicyHttpMessageHandler.cs
            // using Microsoft.Extensions.Http.Polly; -> IHttpClientBuilder.AddTransientHttpErrorPolicy ???
            //services.AddHttpClient("api").AddTransientHttpErrorPolicy(p => p.RetryAsync(6));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
            //    .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            //services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        // Instead of using the default validation (validating against a single issuer value, as we do in
            //        // line of business apps), we inject our own multitenant validation logic
            //        ValidateIssuer = false,

            //        // If the app is meant to be accessed by entire organizations, add your issuer validation logic here.
            //        //IssuerValidator = (issuer, securityToken, validationParameters) => {
            //        //    if (myIssuerValidationLogic(issuer)) return issuer;
            //        //}
            //    };

            //    options.Events = new OpenIdConnectEvents
            //    {
            //        OnTicketReceived = context =>
            //        {
            //            // If your authentication logic is based on users then add your logic here
            //            return Task.CompletedTask;
            //        },
            //        OnAuthenticationFailed = context =>
            //        {
            //            context.Response.Redirect("/Error");
            //            context.HandleResponse(); // Suppress the exception
            //            return Task.CompletedTask;
            //        },
            //        // If your application needs to do authenticate single users, add your user validation below.
            //        //OnTokenValidated = context =>
            //        //{
            //        //    return myUserValidationLogic(context.Ticket.Principal);
            //        //}
            //    };
            //});

            //services.AddMvc(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //})
            //.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
