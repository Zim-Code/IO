# ZimCode.IO

ZimCode.IO is an importer library to make handling files easy.

For an example on how to create your own importer take a look at the
implementation of the [XmlImporter](https://github.com/Zim-Code/IO/blob/master/IO/XmlImporter.cs) and [XmlLoader](https://github.com/Zim-Code/IO/blob/master/IO/XmlLoader.cs) classes.


## Operations

Operations are used internaly to pass params between loading operations and handle exceptions for you, providing you with a simple async method that can be awaited, and
if any exceptions occur, are avaliable through the [ProgressReporter](https://github.com/Zim-Code/IO/blob/master/IO/ProgressReporter.cs) class.

You can also chain Operations for an automatic progress report.

```C#
// Load the XDocument. The XDocument returned from the OpenDocument
// method will be passed to the next operation if it is a Consume or
// ConsumeGenerate. Here we use ConsumeGenerate because we need a
// result as the last operation in a loader.
yield return Operation.Generate<XDocument>(OpenDocument);

// Here we consume the XDocument that was loaded from the previous
// operation and add a new first element to it.
yield return Operation.ConsumeGenerate<XDocument, XDocument>(d =>
{
    d.AddFirst(new XElement("AddedByLoader"));
    return d;
});
```