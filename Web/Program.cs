using Data;
using Data.DataSource;
using Domain.Services;
using EVoting6.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<InMemoryDataSource, InMemoryDataSource>();
builder.Services.AddSingleton<DataProviderService, DataProviderService>();
builder.Services.AddSingleton<RegistrationBureau, RegistrationBureau>();
builder.Services.AddSingleton<VotingCenter, VotingCenter>();
builder.Services.AddSingleton<ClientVotingService, ClientVotingService>();
builder.Services.AddSingleton<UserService, UserService>();

var app = builder.Build();

app.MapGet("/", () => "Lab 6");

app.UseRouting();
app.MapControllers();

app.Run();