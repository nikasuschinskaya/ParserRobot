using System.Collections.Generic;

namespace ParserRobot.DAL.Writers.Base
{
    public interface IWriter<T>
    {
        void Write(List<T> models);
    }
}
