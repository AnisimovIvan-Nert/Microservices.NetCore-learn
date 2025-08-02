using Microservices.NetCore.LoyaltyProgram.Services.LoyaltyProgramUser;
using Microservices.NetCore.Shared.EventFeed;
using Microservices.NetCore.Shared.EventFeed.Implementation.Store;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddEventFeed<InMemoryEventStore>()
    .AddUsers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();