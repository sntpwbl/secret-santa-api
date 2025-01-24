using Microsoft.EntityFrameworkCore;
using SecretSanta.Context;
using SecretSanta.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<SecretSantaContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("SecretSanta"))
);

builder.Services.AddScoped<IGroupsService, GroupsService>();
builder.Services.AddScoped<IPeopleService, PeopleService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.MapControllers();

app.Run();
