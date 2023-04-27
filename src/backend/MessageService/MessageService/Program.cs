using MessageService.Configuration;
using MessageService.Domain.Persistence;
using MessageService.Persistence;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//var hosts = new string[] { "127.0.0.1" };
var hosts = new string[] { "cassandra-node1" };

builder.Services.AddScoped<IMessageRepository, MessageRepository>(_ => new MessageRepository(hosts));

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseRouteParameterTransformer()));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope()){
    var repo = scope.ServiceProvider.GetRequiredService<IMessageRepository>();
    repo.EnsureTableCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
