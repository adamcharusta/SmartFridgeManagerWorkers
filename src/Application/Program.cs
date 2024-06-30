using SmartFridgeManagerWorkers.EmailWorker;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddEmailWorker(builder.Configuration);

IHost host = builder.Build();
host.Run();
