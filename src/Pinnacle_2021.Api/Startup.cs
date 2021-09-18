using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Newtonsoft.Json.Serialization;

using Pinnacle_2021.Api.DAL;
using Pinnacle_2021.Api.Services.Domain;

namespace Pinnacle_2021.Api
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
			ConfigureControllers(services);
			ConfigurePackages(services);
			ConfigureDataBase(services);
			ConfigureDomainServices(services);
		}

		private void ConfigureDomainServices(IServiceCollection services)
		{
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IInventoryService, InventoryService>();
			services.AddScoped<IItemService, ItemService>();

			services.AddSingleton<IUpcApiClient, UpcApiClient>();
		}

		#region Configure
		private void ConfigureControllers(IServiceCollection services)
		{
			services.AddControllers(setupAction =>
			{
				setupAction.ReturnHttpNotAcceptable = true;
			})
			.AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			})
			.ConfigureApiBehaviorOptions(setupAction => // Returning the correct error code
			{
				setupAction.InvalidModelStateResponseFactory = context =>
				{
					var problemDetailsFactory = context.HttpContext.RequestServices.GetRequiredService<ProblemDetailsFactory>();
					var problemDetails = problemDetailsFactory.CreateValidationProblemDetails(context.HttpContext, context.ModelState);

					problemDetails.Detail = "See the errors fields for details.";
					problemDetails.Instance = context.HttpContext.Request.Path;

					var actionExecutingContext = context as ActionExecutingContext;

					problemDetails.Title = "One or more validation errors occured.";

					// If validation failed
					if ((context.ModelState.ErrorCount > 0) && (actionExecutingContext?.ActionArguments.Count == context.ActionDescriptor.Parameters.Count))
					{
						problemDetails.Type = ""; //TODO: Add custom explication on error type when building a public version of the api.
						problemDetails.Status = StatusCodes.Status422UnprocessableEntity;

						return new UnprocessableEntityObjectResult(problemDetails)
						{
							ContentTypes = { "application/problem+json" }
						};
					}

					// If a field is missing or couldn't be parsed
					problemDetails.Status = StatusCodes.Status400BadRequest;
					return new BadRequestObjectResult(problemDetails)
					{
						ContentTypes = { "application/problem+json" }
					};
				};
			});
		}

		private void ConfigureDataBase(IServiceCollection services)
		{
			services.AddDbContext<PinnacleContext>(options =>
			{
				options.UseSqlServer(Configuration.GetConnectionString("pinnacleDB"))
				.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);

				//TODO: Change this before MEP (PROD)
				options.EnableDetailedErrors(true);
				options.EnableSensitiveDataLogging(true);
			});
		}

		private void ConfigurePackages(IServiceCollection services)
		{
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pinnacle_2021.Api", Version = "v1" });
			});
		}
		#endregion

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler(appBuilder =>
				{
					appBuilder.Run(async context =>
					{
						context.Response.StatusCode = 500;
						await context.Response.WriteAsync("An unexpected fault happened. Try again later.");
					});
				});
			}
			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pinnacle_2021.Api v1"));

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors(x => x
				.AllowAnyMethod()
				.AllowAnyHeader()
				.SetIsOriginAllowed(origin => true) // allow any origin
				.AllowCredentials()); // allow credentials

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}