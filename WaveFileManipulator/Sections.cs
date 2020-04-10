using System.Collections.Generic;

namespace WaveFileManipulator
{
    public class Section<T>
    {
        public T Value { get; protected set; }
        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class ExpectedValueSection<T> : Section<T>
    {
        public ExpectedValueSection(T value, T expectedValue)
        {
            Value = value;
            ExpectedValue = expectedValue;
        }
        public T ExpectedValue { get; protected set; }
        public bool IsValueExpected
        {
            get
            {
                return EqualityComparer<T>.Default.Equals(Value, ExpectedValue);
            }
        }
    }

    public class ChunkId : Section<string>
    {
        public ChunkId(string value)
        {
            Value = value;
        }
        public const int StartIndex = 0;
        public const int Length = 4;
    }

    public class ChunkSize : Section<uint>
    {
        public ChunkSize(uint value)
        {
            Value = value;
        }
        public const int StartIndex = 4;
        public const int Length = 4;
    }

    public class Format : Section<string>
    {
        public Format(string value)
        {
            Value = value;
        }
        public const int StartIndex = 8;
        public const int Length = 4;
    }

    public class SubChunk1Id : Section<string>
    {
        public SubChunk1Id(string value)
        {
            Value = value;
        }
        public const int StartIndex = 12;
        public const int Length = 4;
    }

    public class SubChunk1Size : Section<uint>
    {
        public SubChunk1Size(uint value)
        {
            Value = value;
        }
        public const int StartIndex = 16;
        public const int Length = 4;
    }

    public class AudioFormat : Section<ushort>
    {
        public AudioFormat(ushort value)
        {
            Value = value;
        }
        public const int StartIndex = 20;
        public const int Length = 2;
    }

    public class NumOfChannels : Section<ushort>
    {
        public NumOfChannels(ushort value)
        {
            Value = value;
        }
        public const int StartIndex = 22;
        public const int Length = 2;
    }

    public class SampleRate : Section<uint>
    {
        public SampleRate(uint value)
        {
            Value = value;
        }
        public const int StartIndex = 24;
        public const int Length = 4;
    }

    public class ByteRate : ExpectedValueSection<uint>
    {
        public ByteRate(uint value, uint expectedValue) : base(value, expectedValue) { }        
        public const int StartIndex = 28;
        public const int Length = 4;
    }

    public class BlockAlign : Section<ushort>
    {
        public BlockAlign(ushort value)
        {
            Value = value;
        }
        public const int StartIndex = 32;
        public const int Length = 2;
    }

    public class BitsPerSample : Section<ushort>
    {
        public BitsPerSample(ushort value)
        {
            Value = value;
        }
        public const int StartIndex = 34;
        public const int Length = 2;
    }

    public class SubChunk2Id : Section<string>
    {
        public SubChunk2Id(string value)
        {
            Value = value;
        }
        public const int StartIndex = 36;
        public const int Length = 4;
    }

    public class SubChunk2Size : Section<uint>
    {
        public SubChunk2Size(uint value)
        {
            Value = value;
        }
        public const int StartIndex = 40;
        public const int Length = 4;
    }
}