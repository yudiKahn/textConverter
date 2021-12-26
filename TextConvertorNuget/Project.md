# Text Converter Description
- __Short description__: A developer will use the Converter class to convert JSON to XML and vice versa. By defining the input format, the wanted output format, and the input text/file, all he has left is to call the convert method.
- __The purpose__: To make the lives of developers who want to convert text easier!
- __Targeted SW users__: C# developers/programmers all over the world.
- __Use cases__:
    - The purpose of our package is to convert text, so, as a user, the developer will be able to create an instance of the Converter class and invoke on it an indexer method that receives a Tuple of two Formats (of type enum), one of the input format, and the other of the output. The indexer - in its turn - will return a Convert method that - as the name implies - converts the input parameter. In code, it will look something like that:
    ```
    IConverter converter = Factory.GetOfType<IConverter>();
    string jsonStr = "...";
    string xmlStr = converter[(Format.JSON, Format.XML)](jsonStr);
    ```
    - If there is a problem in the input string, the developer will get an exception with relevant info. The errors that could occur in the input string are:
        * The string is not in the same format as the deserializer format in the indexer.
        * It is in the same format, but it's not written conventionally.
        * A timeout error. Like if the string is too long (or heavy, in the case of a file).
        * If the input and output format is the same, we should - maybe - warn the developer of a possible error.
    - 
- __Required platforms__: .NET 6
- __Design inputs__: The NuGet package will be built on .NET 6 platform using C#.
- __Algorithm description__: Our project is mainly an algorithm project, so our code will do those steps:
    1. The Converter class will have instances of an IDeserializer and ISerializer interfaces and will use them - in the Convert method - according to the input and output format.
    2. The first thing the method will do is to clean the input text from the unnecessary characters. (like whitespace, \n, etc.)
    3. We will call GetTopNodes to get all the top (most left) key-value pairs and parse them into an abstract node. In the meantime, the value will be everything after the key.
    4. Then we will call GetTopNodes to get all the top (most left) key-value pairs and parse them into an abstract node. In the meantime, the value will be everything after the key.
    5. After that, we will iterate over each key-value pair and check if the value is of a complex type. If so, we will iterate over it again.
    6. At that point, we will pass all the returned nodes - from the deserializer method - to the serialize method.
- __Non-functional requirements__:
    - __Privacy__: Our code will not save any input data.  Just I/O
    - __Performance__: The conversion process will take as long as 10,000ms. Longer than that, we'll throw a Timeout error.
