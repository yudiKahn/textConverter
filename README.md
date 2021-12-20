# TextConverter
This project is divided into 5.
 - __TextConverterNuGet__: a class library project that have all the code to convert string to xml, json, yml and c# classes
 - __performance compare__: a class library project that compare functions performance (to test my code vs newtonsoft soft.)
 - __consoleTester__: a console app that test the code in class libraries
 - __text convertor__: .net core 6 mvc web application that uses the class library
 - __utest__: unit test

## example
```
var converter = Factory.GetOfType<IConverter>();
var xmlStr = converter[(Format.JSON,Format.XML)](jsonStr);
Console.WriteLine(xmlStr);
```

[@yudiKahn](https://github.com/yudiKahn/).
