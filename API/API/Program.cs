using API.Middlewares;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using API.BusinessLogic;
using API.BusinessLogic.Interfaces;
using API.BusinessLogic.Security;
using API.DataAccess.UnitOfWork;
using API.EFCore;
using API.Interface.UnitOfWork;
using API.DataAccess.Initializer;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(workerApplication =>
    {
        workerApplication.UseMiddleware<AuthenticationMiddleware>();
        workerApplication.UseMiddleware<AuthorizationMiddleware>();
        workerApplication.UseMiddleware<ExceptionMiddleware>();
    }
    )
    .ConfigureServices(services =>
    {
        var tokenOptions = new TokenOptions();
        var signingConfigurations = new SigningConfigurations(tokenOptions.Secret);
        services.AddSingleton(tokenOptions);
        services.AddSingleton(signingConfigurations);

        string dbConnexion = Environment.GetEnvironmentVariable("SqlConnectionString");
        services.AddScoped(_ => new IdentityServerDbContext(dbConnexion));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ICertificateService, CertificateService>();
        services.AddScoped<IConnectionInformationService, ConnectionInformationService>();
    })
    .Build();

host.Run();



//void SeedDatabase(IFunctionsHostBuilder builder)
//{
//    var context = builder.Services.BuildServiceProvider().GetService<IdentityServerDbContext>();

//    DbInitializer dbInitializer = new DbInitializer(context);
//    dbInitializer.Initialize();
//}