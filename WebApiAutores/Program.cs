using WebApiAutores;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

//Parte del ejemplo de Middleware
var serviceLogger = (ILogger<Startup>) app.Services.GetService(typeof(ILogger<Startup>));

//startup.Configure(app, app.Environment);
startup.Configure(app, app.Environment, serviceLogger);

app.Run();
