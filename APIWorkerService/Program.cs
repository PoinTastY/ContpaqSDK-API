using APIWorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(option =>
    {
        option.ServiceName = "Contpaqi SDK Service";
    })
    .ConfigureServices(services => { services.AddHostedService<ContpaqiSDKService>(); })
    .Build();

host.Run();
