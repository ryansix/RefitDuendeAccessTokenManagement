using Microsoft.AspNetCore.Authentication.JwtBearer;
using Refit;
using RefitDundenAuth;
using RefitDundenAuth.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var authority = "https://demo.duendesoftware.com/";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = authority;
        options.Audience = "api";
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateLifetime = true,
           
        };
    });

builder.Services.AddRefitClient<IWebApiService>()
    .ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri("https://localhost:7132");
    }).AddClientCredentialsTokenHandler("testHandler");

builder.Services.AddDistributedMemoryCache();
builder.Services.AddClientCredentialsTokenManagement()
    .AddClient("testHandler", client =>
    {
        client.TokenEndpoint = $"{authority}connect/token";
        client.ClientId = "m2m";
        client.ClientSecret = "secret";
    });

builder.Services.AddHostedService<MyBackgoundService>();
var app = builder.Build();

var service = app.Services.GetRequiredService<IWebApiService>();
//var res=await service.PostAsync();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
