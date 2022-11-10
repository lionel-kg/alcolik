using alcolik;
using alcolik.Controllers;
using alcolik.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.CodeAnalysis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AlcolikDbContext>();

//Versioning
builder.Services.AddApiVersioning
    (options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        //options.DefaultApiVersion = new ApiVersion(1, 1);
        //options.DefaultApiVersion = ApiVersion.Default;


    }
    );

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'Alcolik v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

/* options.Conventions.Controller<AlcoolsController>()
     .HasDeprecatedApiVersion(1, 0)
     .HasApiVersion(2, 0)
     .Action(typeof(BrandsController).GetMethod(nameof(BrandsController.GetAllV2))!)
     .MapToApiVersion(2, 0);

 options.ReportApiVersions = true;*/

builder.Services.AddSwaggerGen(c => {
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    c.CustomSchemaIds(type => type.FullName);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(
            options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
                }
            }
    );
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
