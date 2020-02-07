using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using SaaSApi.Data;
using SaaSApi.Common;
using SaaSApi.Logic.Services;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using SaaSApi.Logic.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace SaaSApi
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
            services.AddCors();
            services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("TestDb"));
            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Configure pre-controller model validation to conform to ProblemDetails standard
            var statusCodeUrl = $"https://httpstatuses.com/";
            var defaultErrorMessage = "One or more errors occurred.";

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = 400,
                        Type = $"{statusCodeUrl}{400}",
                        Title = ReasonPhrases.GetReasonPhrase(400),
                        Detail = defaultErrorMessage
                    };

                    return new BadRequestObjectResult(problemDetails);
                };
            });

            // Configure service validation/error responses to conform to ProblemDetails standard
            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (context) => false;

                options.Map<ServiceException>(ex =>
                {
                    var problemDetails = ex.Errors != null ? new ValidationProblemDetails(ex.Errors) : new ProblemDetails();
                    problemDetails.Status = ex.Status;
                    problemDetails.Type = $"{statusCodeUrl}{ex.Status}";
                    problemDetails.Title = ReasonPhrases.GetReasonPhrase(ex.Status);
                    problemDetails.Detail = ex.Message ?? defaultErrorMessage;
                    return problemDetails;
                });
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.GetById(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Send complient error to client
            app.UseProblemDetails();
            app.UseRouting();

            // global cors policy
            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(options =>
            {
                options.MapControllers();
            });
        }
    }
}
