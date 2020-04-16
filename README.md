WaveFileManipulator is an open source .NET .wav file manipulation library written by [David Klempfner](https://medium.com/@DavidKlempfner)

## Features
* Reverse the audio
* Get the metadata into a handy object model

![Metadata example](WaveFileManipulator/Metadata.JPG)

## Examples
This is how you can reverse a .wav file:

```c#
using WaveFileManipulator;

class Program
{
    static void Main(string[] args)
    {
        var filePath = @"C:\File.wav";
        var manipulator = new Manipulator(filePath);
        var reversedByteArray = manipulator.Reverse();
    }
}
```
