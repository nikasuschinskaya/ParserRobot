using System.Collections.Generic;

namespace ParserRobot.DAL.Readers.Base
{
    public interface IReader<T>
    {
        T Read(string text);
    }
}
