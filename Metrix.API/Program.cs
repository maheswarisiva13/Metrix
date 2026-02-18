using Metrix.API;

var builder = WebApplication.CreateBuilder(args);

// -------------------- SERVICES --------------------
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// -------------------- PIPELINE --------------------
app.UseApplicationPipeline();

app.Run();
