using dotnet_ai_with_aspnet.Extensions;
using dotnet_ai_with_aspnet.Service;
using Microsoft.AspNetCore.OpenApi.Generated;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddOpenAI();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<ChatService>();
builder.Services.AddSingleton<ImageService>();
builder.Services.AddSingleton<TranscriptionService>();

// Aqui foi feito dessa forma somenta para fins didaticos, as permissoes precisam ser validadas
builder.Services.AddCors(options => options.AddDefaultPolicy(
    builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();       
    }
));

builder.Services.AddOpenApi(options => 
{
    options.AddDocumentTransformer((document, context, _) =>
    {
        document.Info = new()
        {
            Title = ".NET AI API",
            Version = "v1",
            Description = "Essa API prove funcionalidades como chat, geracao de imagens... ",
            Contact = new()
            {
                Name = "Kelvim Rodrigues",
                Email = "kelvimrodrigues1@gmail.com",
                Url = new Uri("https://www.linkedin.com/in/kelvim-rodrigues-dev/")
            },
            License = new ()
            {
                Name = "Apache 2 license",
                Url = new Uri("https://www.linkedin.com/in/kelvim-rodrigues-dev/")
            },
            TermsOfService = new Uri("https://www.linkedin.com/in/kelvim-rodrigues-dev/")
        };

        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = ".NET AI API";
        options.Theme = ScalarTheme.Default;
        options.DefaultHttpClient = new(ScalarTarget.Http, ScalarClient.Http11);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
