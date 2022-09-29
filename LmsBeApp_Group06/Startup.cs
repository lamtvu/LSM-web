using System;
using System.IO;
using System.Reflection;
using Azure.Storage.Blobs;
using LmsBeApp_Group06.Data;
using LmsBeApp_Group06.Data.Repositories.AnswerRepo;
using LmsBeApp_Group06.Data.Repositories.ClassRepo;
using LmsBeApp_Group06.Data.Repositories.ContentRepo;
using LmsBeApp_Group06.Data.Repositories.CourseRepos;
using LmsBeApp_Group06.Data.Repositories.ExerciseRepo;
using LmsBeApp_Group06.Data.Repositories.InformationRepo;
using LmsBeApp_Group06.Data.Repositories.QuestionRepo;
using LmsBeApp_Group06.Data.Repositories.QuizRepo;
using LmsBeApp_Group06.Data.Repositories.RequestTeacherRepo;
using LmsBeApp_Group06.Data.Repositories.SectionRepo;
using LmsBeApp_Group06.Data.Repositories.SubmissionQuizRepo;
using LmsBeApp_Group06.Data.Repositories.SubmisstionExerciseRepo;
using LmsBeApp_Group06.Data.Repositories.AnnouncementRepos;
using LmsBeApp_Group06.Data.Repositories.InvitationRepos;
using LmsBeApp_Group06.Data.Repositories.ReportsRepo;
using LmsBeApp_Group06.Data.Repositories.ReviewRepos;
using LmsBeApp_Group06.Data.Repositories.UserRepos;
using LmsBeApp_Group06.Filters;
using LmsBeApp_Group06.Options;
using LmsBeApp_Group06.Services;
using LmsBeApp_Group06.Services.BlobService;
using LmsBeApp_Group06.Services.TimeService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace LmsBeApp_Group06
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LmsBeApp_Group06", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //Dbcontext
            services.AddDbContext<LmsAppContext>(option =>
            {
                option.UseSqlServer(Configuration.GetConnectionString("LmsAppConnectionString"));
            });

            // Get Identity Default Options
            IConfigurationSection identityDefaultOptionsConfigurationSection = Configuration.GetSection("IdentityDefaultOptions");

            services.Configure<IdentityDefaultOptions>(identityDefaultOptionsConfigurationSection);

            var identityDefaultOptions = identityDefaultOptionsConfigurationSection.Get<IdentityDefaultOptions>();
            
            //Configure cors
            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            // cookie settings
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = identityDefaultOptions.CookieHttpOnly;
                options.ExpireTimeSpan = TimeSpan.FromDays(identityDefaultOptions.CookieExpiration);
                options.AccessDeniedPath = identityDefaultOptions.AccessDeniedPath; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = identityDefaultOptions.SlidingExpiration;
            });

            //Configure
            services.Configure<JwtOption>(Configuration.GetSection("TokenOption"));

            services.Configure<MailOption>(Configuration.GetSection("Gmail"));

            //AddService
            services.AddScoped<IUserRepo, SQLUserRepo>();

            services.AddScoped<ISectionRepo, SQLSectionRepo>();

            services.AddScoped<IExerciseRepo, SQLExerciseRepo>();

            services.AddScoped<ISubmissionExerciseRepo, SQLSubmissionExerciseRepo>();

            services.AddScoped<IUserRepo, SQLUserRepo>();

            services.AddScoped<ISectionRepo, SQLSectionRepo>();

            services.AddScoped<IClassRepo, SQLClassRepo>();

            services.AddScoped<IContentRepo, SQLContentRepo>();

            services.AddScoped<IRequestTeacherRepo, SQLRequestTeacherRepo>();

            services.AddScoped<IInformationRepo, SQLInformationRepo>();

            services.AddScoped<ICourseRepo, SQLCourseRepo>();

            services.AddScoped<IQuestionRepo, SQLQuestionRepo>();

            services.AddScoped<IAnswerRepo, SQLAnswerRepo>();

            services.AddScoped<IQuizRepo, SQLQuizRepo>();

            services.AddScoped<ISubmissionQuizRepo, SQLSubmissionSQLRepo>();

            services.AddScoped<IRequestStudentRepos, SQLRequestStudentRepos>();

            services.AddScoped<IReportsRepo, SQLReportsRepo>();

            services.AddScoped<IInvitationRepos, SQLInvitationRepos>();

            services.AddScoped<IReviewRepo, SQLReviewRepo>();

            services.AddScoped<IAnnouncementRepos, SQLAnnouncementRepos>();

            services.AddScoped<OwnedClassCheck>();

            services.AddScoped<OwnedCourseCheck>();

            services.AddScoped<OwnedExerciseCheck>();

            services.AddScoped<UserCheck>();

            services.AddScoped<MailService>();

            services.AddSingleton<ITimeService, TimeService>();

            services.AddSingleton(X => new BlobServiceClient(Configuration.GetValue<string>("AzureBlobStorageConnectionString")));

            services.AddSingleton<IBlobService, BlobService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton<JwtService>();

            services.AddScoped<AuthorizeService>();

            //authentication
            var tokenSecretKey = Configuration.GetSection("TokenOption").Get<JwtOption>();
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(tokenSecretKey.TokenSecretKey))
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LmsBeApp_Group06 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
