﻿using Autofac;
using ParserRobot.UI.ViewModels;
using ParserRobot.UI.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserRobot.UI.Container
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            //var connectionString = ConfigurationManager.ConnectionStrings["sqlServerDB"].ConnectionString;

            //builder.RegisterType<ApplicationDbContext>()/*.As<DbContext>()*/
            //                .WithParameter("options", new DbContextOptionsBuilder<ApplicationDbContext>()
            //                                .UseSqlServer(connectionString)
            //                                .Options)
            //                .SingleInstance();

            //builder.RegisterType<AdminRepository>().As<IRepository<AdminEntity>>();
            //builder.RegisterType<CoinRepository>().As<IRepository<CoinEntity>>();
            //builder.RegisterType<DrinkRepository>().As<IRepository<DrinkEntity>>();

            //builder.RegisterType<DrinkManager>()
            //           .WithParameter((pi, c) => pi.ParameterType == typeof(IRepository<DrinkEntity>),
            //                           (pi, c) => c.Resolve<IRepository<DrinkEntity>>());

            //builder.RegisterType<CoinManager>()
            //           .WithParameter((pi, c) => pi.ParameterType == typeof(IRepository<CoinEntity>),
            //                           (pi, c) => c.Resolve<IRepository<CoinEntity>>());

            //builder.RegisterType<AutorizationManager>().AsSelf()
            //            .WithParameter((pi, c) => pi.ParameterType == typeof(IRepository<AdminEntity>),
            //                           (pi, c) => c.Resolve<IRepository<AdminEntity>>());

            //builder.RegisterType<AdminManager>()
            //           .WithParameter((pi, c) => pi.ParameterType == typeof(IRepository<DrinkEntity>),
            //                          (pi, c) => c.Resolve<IRepository<DrinkEntity>>())
            //           .WithParameter((pi, c) => pi.ParameterType == typeof(IRepository<CoinEntity>),
            //                          (pi, c) => c.Resolve<IRepository<CoinEntity>>());


            //builder.RegisterType<MainWindow>().AsSelf();
            ////builder.RegisterType<MainViewModel>().AsSelf();
            //builder.RegisterType<MainViewModel>()
            //           .WithParameter((pi, c) => pi.ParameterType == typeof(CoinManager),
            //                          (pi, c) => c.Resolve<CoinManager>())
            //           .WithParameter((pi, c) => pi.ParameterType == typeof(DrinkManager),
            //                                      (pi, c) => c.Resolve<DrinkManager>());

            //builder.RegisterType<AutorizationWindow>().AsSelf();
            ////builder.RegisterType<AutorizationViewModel>().AsSelf();
            //builder.RegisterType<AutorizationViewModel>()
            //           .WithParameter((pi, c) => pi.ParameterType == typeof(AutorizationManager),
            //                          (pi, c) => c.Resolve<AutorizationManager>());

            //builder.RegisterType<AdminWindow>().AsSelf();
            ////builder.RegisterType<AdminViewModel>().AsSelf();
            //builder.RegisterType<AdminViewModel>()
            //           .WithParameter((pi, c) => pi.ParameterType == typeof(AdminManager),
            //                          (pi, c) => c.Resolve<AdminManager>());

            return builder.Build();
        }
    }
}
