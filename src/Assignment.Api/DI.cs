﻿using Amazon.CloudWatchLogs.Model;
using Microsoft.Extensions.Configuration;
using Assignment.Api.Controllers;
using Assignment.Api.Interfaces;
using Assignment.Core.Interfaces;
using Assignment.Infrastructure.AuditLog;
using Assignment.Infrastructure.Interfaces;
using Assignment.Infrastructure.Notification;
using Assignment.Infrastructure.Raiden;
using Assignment.Infrastructure.Repository;
using Assignment.Service.Services;
using Serilog.Core;
using System.ComponentModel;
using static Assignment.Api.Controllers.GoogleWorkSpaceController;
using Assignment.Api.Interfaces.PlayerPulseInterfaces;
using Assignment.Infrastructure.Repository.PlayerPulseRepository;
using Assignment.Service.Services.PlayerPulseServices;

namespace Assignment.Api
{
    internal static partial class DI
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var logger = Logger.CreateLogger(configuration);
            CS_AppServices(services, configuration, logger);
        }
        private static void CS_AppServices(IServiceCollection services, IConfiguration configuration, Serilog.Core.Logger logger)
        {
            services.AddHttpClient<LoggingService>();
            services.AddTransient(typeof(IDBGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient(typeof(IDBRolesPermissionsRepository), typeof(RolesPermissionsRepository));
            services.AddTransient(typeof(RolesPermissionService), typeof(RolesPermissionService));
            services.AddScoped(typeof(IDBOrganization), typeof(OrganizationRepository));
            services.AddTransient(typeof(AuthService), typeof(AuthService));
            services.AddTransient(typeof(IDBAuthRepository), typeof(AuthRepository));
            services.AddTransient(typeof(IDBUserRepository), typeof(UserRepository));
            services.AddTransient(typeof(UserService), typeof(UserService));
            services.AddScoped(typeof(ApplicationsService), typeof(ApplicationsService));
            services.AddScoped(typeof(IDBApplicationRepository), typeof(ApplicationRepository));
            services.AddScoped(typeof(AuthController), typeof(AuthController));
            services.AddScoped(typeof(GoogleWorkSpaceService), typeof(GoogleWorkSpaceService));
            services.AddScoped(typeof(IDBGoogleWorkSpaceRepository), typeof(GoogleWorkSpaceRepository));
            services.AddScoped(typeof(CustomAuthorizationService), typeof(CustomAuthorizationService));
            services.AddScoped(typeof(IDBProductsRepository), typeof(ProductsRepository));
            services.AddScoped(typeof(ProductsService), typeof(ProductsService));
            services.AddScoped(typeof(INotificationService), typeof(NotificationService));
            services.AddScoped(typeof(IRaidenService), typeof(RaidenService));
            services.AddHttpClient<GoogleWorkSpaceController>();
            services.AddHttpClient<NotificationService>();
            services.AddHttpClient<RaidenService>();
            services.AddScoped(typeof(OrganizationService), typeof(OrganizationService));
            services.AddScoped(typeof(ILoggingService), typeof(LoggingService));
            services.AddSingleton<Serilog.Core.Logger>(logger);

            //PlayerPulse
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(PPIDBUserRepository), typeof(PPUserRepository));
            services.AddScoped(typeof(PPUserService), typeof(PPUserService));
            services.AddScoped(typeof(PPRoleService), typeof(PPRoleService));
            services.AddScoped(typeof(PPIDBRoleRepository), typeof(PPRoleRepository));
            services.AddScoped(typeof(PPPlayerService), typeof(PPPlayerService));
            services.AddScoped(typeof(PPIDBPlayerRepository), typeof(PPPlayerRepository));
            services.AddScoped(typeof(PPSportService), typeof(PPSportService));
            services.AddScoped(typeof(PPIDBSportRepository), typeof(PPSportRepository));
            services.AddScoped(typeof(PPTeamService), typeof(PPTeamService));
            services.AddScoped(typeof(PPIDBTeamRepository), typeof(PPTeamRepository));
            services.AddScoped(typeof(PPAuctionService), typeof(PPAuctionService));
            services.AddScoped(typeof(PPIDBAuctionRepository), typeof(PPAuctionRepository));
            services.AddScoped(typeof(PPAuctionBidService), typeof(PPAuctionBidService));
            services.AddScoped(typeof(PPIDBAuctionBidRepository), typeof(PPAuctionBidRepository));

        }
    }
}