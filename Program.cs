using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Context;
using SecretSanta.Services;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.EnableAnnotations();
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Secret Santa API",
        Version = "v1",
        Description = "API builded to manage groups and participants of Secret Santa's game.",
        Contact = new OpenApiContact
        {
            Name = "João Paulo",
            Email = "joaop401@outlook.com"
        }
    });
});
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
app.UseMiddleware<ExceptionMiddleware>();
app.UseRouting();
app.MapControllers();

app.Run();
