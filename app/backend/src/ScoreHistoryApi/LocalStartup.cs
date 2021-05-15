using System;
using System.Security.Claims;
using Amazon.Runtime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ScoreHistoryApi.Factories;
using ScoreHistoryApi.Logics;

namespace ScoreHistoryApi
{
    public class LocalStartup
    {
        public LocalStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IScoreQuota>(x => new ScoreQuota());
            services.AddSingleton(x =>
                new DynamoDbClientFactory()
                    .SetEndpointUrl(new Uri(Configuration[EnvironmentNames.ScoreDynamoDbEndpointUrl]))
                    .Create());
            services.AddSingleton(x =>
                new S3ClientFactory()
                    .SetEndpointUrl(new Uri(Configuration[EnvironmentNames.ScoreS3EndpointUrl]))
                    .SetCredentials(new BasicAWSCredentials(Configuration[EnvironmentNames.ScoreS3AccessKey],Configuration[EnvironmentNames.ScoreS3SecretKey]))
                    .Create());
            services.AddScoped<ScoreLogicFactory>();

            services.AddControllers();

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("dev",new OpenApiInfo()
                {
                    Version = "dev"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                context.User.AddIdentity(new ClaimsIdentity(new[]
                {
                    new Claim("sub", "00000000-0000-0000-0000-000000000000"),
                    new Claim("principalId", "00000000-0000-0000-0000-000000000000"),
                    new Claim("cognito:username", "test-user"),
                    new Claim("email", "test-user@example.com"),
                }));
                await next.Invoke();
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/dev/swagger.json", "Ura-Kata Score History API");
            });
        }
    }
}
