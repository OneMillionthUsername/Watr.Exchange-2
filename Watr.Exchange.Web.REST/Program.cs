using ExRam.Gremlinq.Core.AspNet;
using ExRam.Gremlinq.Providers.CosmosDb.AspNet;
using ExRam.Gremlinq.Support.NewtonsoftJson.AspNet;
using Gremlin.Net.Structure;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Watr.Exchange.Business;
using Watr.Exchange.Business.Core;
using Watr.Exchange.Core;
using Watr.Exchange.Data.Commands;
using Watr.Exchange.Data.Commands.Core;
using Watr.Exchange.Data.Core.Actors;
using Watr.Exchange.Data.Queries;
using Watr.Exchange.Data.Queries.Core;
using Watr.Exchange.Mapping.Core;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ActorMappingProfile>();
});
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterGenericHandlers = true;
    cfg.RegisterServicesFromAssembly(typeof(WatrExchangeQueryHandlers).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(WatrExchangeCommandHandlers).Assembly);
});

builder.Services
    .AddGremlinq(setup => setup
        .UseCosmosDb<Watr.Exchange.Data.Core.Vertex, Watr.Exchange.Data.Core.Edge>()
        .UseNewtonsoftJson())
    .AddControllers();
builder.Services.AddTransient<UpdateGenericActorTypeConverter<UpdateGenericActorDTO>>();
builder.Services.AddTransient<IRequestHandler<UpdateVertex<Actor>, Unit>, UpdateCommandHandler<UpdateVertex<Actor>, Actor>>();
builder.Services.AddTransient<IRequestHandler<CreateAdmin, string>, CreateCommandHandler<CreateAdmin, Admin>>();
builder.Services.AddTransient<IRequestHandler<UpdateVertex<Admin>, Unit>, UpdateCommandHandler<UpdateVertex<Admin>, Admin>>();
builder.Services.AddTransient<IRequestHandler<DeleteVertex, Unit>, DeleteCommandHandler<DeleteVertex>>();
builder.Services.AddTransient<IRequestHandler<GetVertexById<Actor>, Actor?>, GetVertexByIdHandler<GetVertexById<Actor>, Actor>>();
builder.Services.AddTransient<IStreamRequestHandler<GetVertexes<Actor>,Actor>, GetVertexesHandler<GetVertexes<Actor>, Actor>>();
builder.Services.AddTransient<IRequestHandler<GetQueryPageCount<Actor>, long>, GetQueryPageCountHandler<GetQueryPageCount<Actor>, Actor>>();
builder.Services.AddTransient<IRequestHandler<GetVertexById<Admin>, Admin?>, GetVertexByIdHandler<GetVertexById<Admin>, Admin>>();
builder.Services.AddTransient<IStreamRequestHandler<GetVertexes<Admin>, Admin>, GetVertexesHandler<GetVertexes<Admin>, Admin>>();
builder.Services.AddTransient<IRequestHandler<GetQueryPageCount<Admin>, long>, GetQueryPageCountHandler<GetQueryPageCount<Admin>, Admin>>();
builder.Services.AddScoped<IActorReadActivity, ReadActorActivity>();
builder.Services.AddScoped<IUpdateActorActivity, UpdateActorAcitivity>();
builder.Services.AddScoped<IDeleteActorActivity, DeleteActorActivity>();
builder.Services.AddScoped<IAdminReadActivity, ReadAdminActivity>();
builder.Services.AddScoped<ICreateAdminActivity, CreateAdminActivity>();
builder.Services.AddScoped<IUpdateAdminActivity, UpdateAdminActivity>();
builder.Services.AddScoped<IDeleteAdminActivity, DeleteAdminActivity>();
builder.Services.AddScoped<IUpdateActorProcessor, UpdateActorProcessor>();
builder.Services.AddScoped<IReadActorProcessor, ReadActorProcessor>();
builder.Services.AddScoped<IDeleteActorProcessor, DeleteActorProcessor>();
builder.Services.AddScoped<ICreateAdminProcessor, CreateAdminProcessor>();
builder.Services.AddScoped<IReadAdminProcessor, ReadAdminProcessor>();
builder.Services.AddScoped<IUpdateAdminProcessor, UpdateAdminProcessor>();
builder.Services.AddScoped<IDeleteAdminProcessor, DeleteAdminProcessor>();
builder.Services.AddScoped<IReadActivity<ReadActorDTO, Guid, IActor>, ReadActorActivity>();
builder.Services.AddScoped<IReadActivity<ReadAdminDTO, Guid, IAdmin>, ReadAdminActivity>();
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
