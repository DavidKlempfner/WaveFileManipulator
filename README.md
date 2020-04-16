WaveFileManipulator is an open source .NET .wav file manipulation library written by [David Klempfner](https://medium.com/@DavidKlempfner)

## Features
* Reverse the audio
* Get the metadata into a handy object model

![Metadata example](WaveFileManipulator/Metadata.JPG)

## Examples
* Reverse the audio

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
* View the metadata given a byte array
```c#
using WaveFileManipulator;

class Program
{
    static void Main(string[] args)
    {
        byte[] byteArray = GetBytesFromWaveFile();
        var metadata = new Metadata(byteArray);
    }
}
```
* View the metadata given a file path
```c#
using WaveFileManipulator;

class Program
{
    static void Main(string[] args)
    {
        var filePath = @"C:\File.wav";
        var manipulator = new Manipulator(filePath);
        var metadata = manipulator.Metadata;        
    }
}
```
