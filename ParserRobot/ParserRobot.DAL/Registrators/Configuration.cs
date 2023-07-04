using Autofac;
using ParserRobot.DAL.ModelsDAO;
using ParserRobot.DAL.Readers;
using ParserRobot.DAL.Readers.Base;
using ParserRobot.DAL.Writers;
using ParserRobot.DAL.Writers.Base;

namespace ParserRobot.DAL.Registrators
{
    public static class Configuration
    {
        public static ContainerBuilder RegisterDAL(this ContainerBuilder builder)
        {
            builder.RegisterType<IAReader>().As<IReader<InternetAcquiring>>();
            builder.RegisterType<MAReader>().As<IReader<MerchantAcquiring>>();

            builder.RegisterType<IAWriter>().As<IWriter<InternetAcquiring>>();
            builder.RegisterType<MAWriter>().As<IWriter<MerchantAcquiring>>();

            return builder;
        }
    }
}
