using MagazineManager.Models;
using MagazineManager.Server.Controllers.CustomExceptions;
using MagazineManager.Server.Controllers.Filters;
using MagazineManager.Server.Data.Repositories.Abstraction;
using MagazineManager.Server.Data.Repositories.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

// Action Filters
builder.Services.AddControllers(options =>
{
    options.Filters.Add<AuthenticateActionFilter>();
});

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

builder.Services.AddEndpointsApiExplorer();

//JWT to Swagger also
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Protected access using the Token goten from API"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddTransient<BaseRepository<Magazine>>(provider =>
    new MagazineRepository(connectionString)
);

builder.Services.AddTransient<BaseRepository<ApplicationUser>>(provider =>
    new ApplicationUserRepository(connectionString)
);

var app = builder.Build();

app.UseStatusCodePages();

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        //MimeType plain text 
        context.Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Plain;
        await context.Response.WriteAsync("An exception was thrown.");
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

        var ex = exceptionHandlerPathFeature?.Error;
        if (ex is FileNotFoundException)
        {
            await context.Response.WriteAsync(" The file was not found.");
        }
        if (ex is Microsoft.Data.SqlClient.SqlException)
        {
            await context.Response.WriteAsync(" Database exception: " + ex.Message);
        }
        if (ex is CustomException)
        {
            await context.Response.WriteAsync(" Known exception: " + ex.Message);
        }
        if (exceptionHandlerPathFeature?.Path == "/")
        {
            await context.Response.WriteAsync(" Page: Home.");
        }
    });
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();
app.UseCors(x =>x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
