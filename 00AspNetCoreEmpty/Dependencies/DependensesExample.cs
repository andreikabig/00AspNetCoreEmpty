/*
    Класс Message зависит от класса Logger
    Dependence Injection - внедрение зависимостей - механизм, который позволяет сделать взаимодействующие в приложении объекты слабосвязаными
            Релазиация: через интерфейсы, чтобы отвязать зависимость класса от конкретной реализации
 */

interface ILogger
{
    void Log(string message);
}

class Logger : ILogger
{
    public void Log(string message) => Console.WriteLine(message);
}

class Message
{
    ILogger logger;
    public Message(ILogger logger)
    { 
        this.logger = logger;
    }

    public string Text { get; set; } = "";
    public void Print() => logger.Log(Text);
}

