using Microsoft.Extensions.Options;
using Producer.Services;
using Scalar.AspNetCore;
using Shared.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddOptions<RabbitMQOptions>()
    .BindConfiguration(RabbitMQOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

//var rabbitMqSetting = builder.Configuration.GetSection(RabbitMQOptions.SectionName).Get<RabbitMQOptions>(); 

builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<RabbitMQOptions>>().Value);

builder.Services.AddScoped<IMessageProducer, MessageProducer>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
