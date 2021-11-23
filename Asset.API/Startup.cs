using Asset.Core;
using Asset.Core.Services;
using Asset.Domain;
using Asset.Domain.Services;
using Asset.Models;
using Asset.ViewModels.UserVM;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asset.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfigurationVM>();
            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AssetConnStr")));
            services.AddScoped<IRoleCategoryService, RoleCategoryService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<ISubOrganizationService, SubOrganizationService>();
            services.AddScoped<IGovernorateService, GovernorateService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IHospitalService, HospitalService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IAssetDetailService, AssetDetailService>();
            services.AddScoped<IAssetPeriorityService, AssetPeriorityService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISubCategoryService, SubCategoryService>();
            services.AddScoped<IECRIService, ECRIService>();
            services.AddScoped<IAssetStatusService, AssetStatusService>();


            services.AddScoped<IHospitalApplicationService, HospitalApplicationService>();
            services.AddScoped<IHospitalExecludeReasonService, HospitalExecludeReasonService>();
            services.AddScoped<IHospitalHoldReasonService, HospitalHoldReasonService>();

            services.AddScoped<IApplicationTypeService, ApplicationTypeService>();


            services.AddScoped<IAssetStatusTransactionService, AssetStatusTransactionService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IMasterAssetService, MasterAssetService>();
            services.AddScoped<IOriginService, OriginService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<ICommetieeMemberService, CommetieeMemberService>();
            services.AddScoped<IAssetMovementService, AssetMovementService>();
            services.AddScoped<IMasterContractService, MasterContractService>();
            services.AddScoped<IContractDetailService, ContractDetailService>();
            services.AddScoped<IClassificationService, ClassificationService>();
            services.AddScoped<IAssetOwnerService, AssetOwnerService>();
            services.AddScoped<IBuildingService, BuildingService>();
            services.AddScoped<IFloorService, FloorService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IPMAssetTaskService, PMAssetTaskService>();
            services.AddScoped<IPMTimeService, PMTimeService>();
            services.AddScoped<IPMAssetTimeService, PMAssetTimeService>();
            services.AddScoped<IPMAssetTaskScheduleService, PMAssetTaskScheduleService>();

            services.AddScoped<ISupplierExecludeReasonService, SupplierExecludeReasonService>();
            services.AddScoped<ISupplierExecludeAssetService, SupplierExecludeAssetService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<IRequestModeService, RequestModeService>();
            services.AddTransient<IRequestPeriorityService, RequestPeriorityService>();
            services.AddTransient<IRequestDocumentService, RequestDocumentService>();
            services.AddTransient<IRequestTrackingService, RequestTrackingService>();
            services.AddTransient<IRequestStatusService, RequestStatusService>();
            services.AddTransient<IRequestTypeService, RequestTypeService>();
            services.AddTransient<IProblemService, ProblemService>();
            services.AddTransient<ISubProblemService, SubProblemService>();
            services.AddTransient<IAssetWorkOrderTaskService, AssetWorkOrderTaskService>();
            services.AddTransient<IWorkOrderService, WorkOrderService>();
            services.AddTransient<IWorkOrderPeriorityService, WorkOrderPeriorityService>();
            services.AddTransient<IWorkOrderStatusService, WorkOrderStatusService>();
            services.AddTransient<IWorkOrderTaskService, WorkOrderTaskService>();
            services.AddTransient<IWorkOrderTypeService, WorkOrderTypeService>();
            services.AddTransient<IWorkOrderTrackingService, WorkOrderTrackingService>();
            services.AddTransient<IWorkOrderAttachmentService, WorkOrderAttachmentService>();
            services.AddTransient<IWorkOrderAssignService, WorkOrderAssignService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IPagingService, PagingService>(); 



            services.AddIdentity<ApplicationUser, ApplicationRole>()
                 .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();



            // Adding Authentication  
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });





        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(
                 options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                 );
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });
            app.UseDefaultFiles();
            app.UseStaticFiles(); // For the wwwroot folder.

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "UploadedAttachments")),
                RequestPath = "/UploadedAttachments",
                EnableDirectoryBrowsing = true
            });


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
