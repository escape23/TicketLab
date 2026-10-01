using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using TicketLab.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TicketLabDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TicketLab")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        // Enums as text in JSON ("High", not 2). allowIntegerValues: false rejects
        // numbers like 99 that are not a valid enum value.
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
