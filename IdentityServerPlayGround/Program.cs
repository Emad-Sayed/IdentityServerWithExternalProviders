using IdentityServer4.Services;
using IdentityServerPlayGround;
using IdentityServerPlayGround.EF;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("IdentityServerDatabase");
var migrationAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

builder.Services.AddControllers();
builder.Services.AddRazorPages();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddAspNetIdentity<ApplicationUser>()
    .AddProfileService<ProfileService>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssembly));
    });


builder.Services.AddAuthentication(options =>
{
    // Store the session to cookies
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    // Which Default Redirection (Challange) if the user not authenticated on API
    options.DefaultChallengeScheme = "NafathOpenId";
})
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOpenIdConnect("NafathOpenId", "Nafath", options =>
    {
        options.Authority = "http://localhost:8080/realms/playGround";
        options.MetadataAddress = "http://localhost:8080/realms/playGround/.well-known/openid-configuration";
        // Client configured in the Keycloak

        // For testing we disable https (should be true for production)
        options.RequireHttpsMetadata = false;
        options.SaveTokens = true;
        options.ClientId = "test-client";
        // Client secret shared with Keycloak
        //options.ClientSecret = "nrnsK6od01BpzJ0NTzvdC2DLt0ta72zp";
        options.GetClaimsFromUserInfoEndpoint = true;

        options.Scope.Add("openid");
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//DatabaseInitializer.PopulateIdentityServer(app);

app.UseHttpsRedirection();
app.UseAuthentication(); ;

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

app.UseIdentityServer();

app.Run();
