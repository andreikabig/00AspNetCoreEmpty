/*
    *********************************************************************************
    Program.cs - запускает и настраивает веб-хост, содержит логику обработки запросов
    *********************************************************************************
    . В ASP.NET Core 6 добавили Minimal API - упрощенная минимализированная модель для запуска веб-приложения
 */


using System.Text.Json;
using System.Text.Json.Serialization;

internal class PersonConverter : JsonConverter<Person>  // JsonConverter<T> - абстрактный класс, типизируется типом объетка, для которого необходимо выполнить десериализацию/сериализацию
{
    // Десериализация: форма -> объект
    // Результат Read() - десериализованный объект (в данном случае Person)
    public override Person? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        /*
            * reader - объект, который читает данные из JSON
            * typeToConvert - тип, в который необходимо выполнить конвертацию
            * options - дополнительные параметры сериализации
         */
    {
        var personName = "Undefined";
        var personAge = 0;

        while (reader.Read()) // Считываем каждый токен в строке JSON
        {
            // Если считанный токен - название свойства
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                // то считываем его
                var propertyName = reader.GetString();

                // и считываем следующий токен
                reader.Read();

                // Проверяем название свойства
                switch (propertyName)
                {
                    // Если свойство age/Age и оно содержит число
                    case "age" or "Age" when reader.TokenType == JsonTokenType.Number:
                        personAge = reader.GetInt32();  // Считываем число из json
                        break;
                    case "age" or "Age" when reader.TokenType == JsonTokenType.String:
                        string? stringValue = reader.GetString();
                        // Пытаемся конвертировать строку в число
                        if (int.TryParse(stringValue, out int value))
                        {
                            personAge = value;
                        }
                        break;
                    case "Name" or "name":
                        string? name = reader.GetString();
                        if (name != null)
                            personName = name;
                        break;
                }
            }
        }
        return new Person(personName, personAge);
    }

    // Сериализация: объект -> форма
    public override void Write(Utf8JsonWriter writer, Person value, JsonSerializerOptions options)
        /*
            * writer - объект, который записывает данные в JSON
            * value - объект, который нужно сериализовать
            * options - дополнительные параметры сериализации
         */
    {
        // Открываем запись объекта в json
        writer.WriteStartObject();

        // Записываем данные объекта в json
        writer.WriteString("name", value.Name);
        writer.WriteNumber("age", value.Age);

        // Завершаем запись объекта
        writer.WriteEndObject();
    }
}