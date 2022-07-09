using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ApiBase.Error;
using ApiBase.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiBase
{
    public abstract class ApiBase
    {
        private bool UseSwagger = false;
        private readonly BadRequestErrorHandler BadRequestErrorHandler;

        protected ApiBase(IConfiguration configuration)
        {
            Configuration = configuration;
            BadRequestErrorHandler = new BadRequestErrorHandler(GetBadRequestErrorProvider());
        }

        protected abstract bool ConfigureOpenAPI(IServiceCollection services);

        protected abstract void ConfigureApi(IServiceCollection services);

        protected abstract ApiVersion DefaultVersion();

        protected abstract IBadRequestErrorCodeProvider GetBadRequestErrorProvider();

        public IConfiguration Configuration { get; protected set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureVersioning(services);

            _ = services.AddCors(o => o.AddPolicy("AnyOrigin", b => b.AllowAnyOrigin().AllowAnyMethod()));
            _ = services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
            _ = services.AddAutoMapper(typeof(ApiBase));
            _ = services.AddMvc().ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = actionContext => BadRequestErrorHandler.Handle(actionContext));

            _ = services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            _ = services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                if (GetAPIXmlDescriptionsFileName() != null && System.IO.File.Exists(
                        $"{AppDomain.CurrentDomain.BaseDirectory}/{GetAPIXmlDescriptionsFileName()}"))
                {
                    options.IncludeXmlComments($"{AppDomain.CurrentDomain.BaseDirectory}/{GetAPIXmlDescriptionsFileName()}");
                }

                SetTags(options);
            });

            UseSwagger = ConfigureOpenAPI(services);
            ConfigureApi(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            var PathBase = GetPathBase();
            app.UseCors("AnyOrigin");

#if DEBUG
            app.UseDeveloperExceptionPage();
#endif
            if (UseSwagger)
            {
                app.UseSwagger(c =>
                {
                    if (!string.IsNullOrEmpty(PathBase))
                    {
                        c.RouteTemplate = PathBase.Trim('/') + "/swagger/{documentName}/swagger.json";
                        c.PreSerializeFilters.Add((swaggerDoc, httpReq) => 
                            swaggerDoc.Servers = new List<OpenApiServer>
                            {
                                new (){ Url = $"https://{httpReq.Host.Value}{PathBase}" }, 
                                new () { Url = $"http://{httpReq.Host.Value}{PathBase}" }
                            });
                    }
                });
                SetSwaggerEndpoint(app, env, provider);
            }

            if (!string.IsNullOrEmpty(PathBase))
            {
                app.UsePathBase(PathBase);
            }

            var options = new RewriteOptions();
            options.AddRedirect("^$", "swagger");
            if (!string.IsNullOrEmpty(PathBase))
            {
                options.AddRedirect($"^{PathBase}$", "swagger");
            }
            app.UseRewriter(options);

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        protected virtual void SetSwaggerEndpoint(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            _ = app.UseSwaggerUI(c =>
              {
                  var PathBase = GetPathBase();
                  if (!string.IsNullOrEmpty(PathBase))
                  {
                      c.RoutePrefix = PathBase.Trim('/') + "/swagger";
                  }
                  foreach (var description in provider.ApiVersionDescriptions)
                  {
                      c.SwaggerEndpoint($"{PathBase}/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                  }
              });
        }

        /// <summary>
        /// Adds support for changing the Tag the API controller is listed under by using the DisplayName class attribute
        /// If support for multiple for multiple tags is required this method can be overridden for a custom implementation
        /// </summary>
        /// <param name="options"></param>
        protected virtual void SetTags(SwaggerGenOptions options)
        {
            options.TagActionsBy(apiDesc =>
            {
                if (apiDesc.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
                {
                    return new List<string> {apiDesc.ActionDescriptor.RouteValues["controller"]};
                }

                var attr = (DisplayNameAttribute)Attribute.GetCustomAttribute(controllerActionDescriptor.ControllerTypeInfo, typeof(DisplayNameAttribute));
                if (attr != null)
                {
                    return new List<string> { attr.DisplayName };
                }
                return new List<string> { apiDesc.ActionDescriptor.RouteValues["controller"] };
            });
        }

        /// <summary>
        /// Returns the path where the  XML descriptions can be found for the  Swagger documentation
        /// See ApiBase documentation for more information
        /// </summary>
        /// <returns>null for disabled or the xml filename</returns>
        protected virtual string GetAPIXmlDescriptionsFileName()
        {
            return "API.xml";
        }

        private string GetPathBase()
        {
            return !string.IsNullOrEmpty(Configuration.GetValue<string>("PathBase")) ? Configuration.GetValue<string>("PathBase") : "";
        }

        private void ConfigureVersioning(IServiceCollection services)
        {
            _ = services.AddApiVersioning(c =>
            {
                c.DefaultApiVersion = DefaultVersion();
                c.AssumeDefaultVersionWhenUnspecified = true;
                c.ReportApiVersions = true;
                c.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("X-API-VERSION"), new QueryStringApiVersionReader("api-version"));
            });
            _ = services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by URL segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}
