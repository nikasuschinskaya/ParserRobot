using System.Collections.Generic;

namespace ParserRobot.DAL.Readers.Base
{
    public interface IReader<T>
    {
        bool IsCorrectData { get; set; }
        List<T> Read(string text);
    }
}
