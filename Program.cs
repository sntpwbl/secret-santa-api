using Microsoft.EntityFrameworkCore;
using SecretSanta.Context;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<SecretSantaContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("SecretSanta"))
);

var app = builder.Build();

// Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.MapControllers();

app.Run();
