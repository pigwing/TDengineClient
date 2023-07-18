using TDengine.WebClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTDengineWebClient(connectionConfiguration =>
{
    connectionConfiguration.Host = "http://192.168.1.18:6041";
    connectionConfiguration.Database = "eco_pro_predict";
}, new List<ConnectionConfiguration>()
{
    new ConnectionConfiguration()
    {
        Host = "http://192.168.1.18:6041",
        //Password = "a"
    }
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
