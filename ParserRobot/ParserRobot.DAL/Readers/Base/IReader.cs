using System.Collections.Generic;

namespace ParserRobot.DAL.Readers.Base
{
    public interface IReader<T>
    {
        bool IsCorrectData { get; set; }
        T Read(string text);
    }
}
