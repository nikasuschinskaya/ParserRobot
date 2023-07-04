using Autofac;
using ParserRobot.BLL.Workers;
using ParserRobot.DAL.ModelsDAO;
using ParserRobot.DAL.Readers.Base;
using ParserRobot.DAL.Registrators;
using ParserRobot.DAL.Writers.Base;

namespace ParserRobot.BLL.Registrators
{
    public static class Configuration
    {
        public static ContainerBuilder RegisterBLL(this ContainerBuilder builder)
        {
            builder = builder.RegisterDAL();

            builder.RegisterType<Worker>()
                      .WithParameter((pi, c) => pi.ParameterType == typeof(IReader<InternetAcquiring>),
                                     (pi, c) => c.Resolve<IReader<InternetAcquiring>>())
                      .WithParameter((pi, c) => pi.ParameterType == typeof(IReader<MerchantAcquiring>),
                                     (pi, c) => c.Resolve<IReader<MerchantAcquiring>>())
                      .WithParameter((pi, c) => pi.ParameterType == typeof(IWriter<InternetAcquiring>),
                                     (pi, c) => c.Resolve<IWriter<InternetAcquiring>>())
                      .WithParameter((pi, c) => pi.ParameterType == typeof(IWriter<MerchantAcquiring>),
                                     (pi, c) => c.Resolve<IWriter<MerchantAcquiring>>());

            return builder;
        }
    }
}
