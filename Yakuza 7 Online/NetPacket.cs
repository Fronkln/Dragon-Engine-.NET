using DragonEngineLibrary;
using System;
using System.IO;


namespace Y7MP
{
    public struct NetPacket
    {
        private const float FLOAT_PRECISION_MULT = 32767.0f;

        public BinaryReader Reader;
        public BinaryWriter Writer;

        public MemoryStream Stream;

        public NetPacket(bool readMode = false)
        {
            Stream = new MemoryStream();

            if(readMode)
            {
                Reader = new BinaryReader(Stream);
                Writer = null;
            }
            else
            {
                Writer = new BinaryWriter(Stream);
                Reader = null;
            }
        }

        public NetPacket(byte[] buffer)
        {
            Stream = new MemoryStream(buffer);
            Reader = new BinaryReader(Stream);
            Writer = null;
        }

        public Quaternion ReadCompressedQuaternion()
        {
            // Read the index of the omitted field from the stream.
            var maxIndex = Reader.ReadByte();

            // Values between 4 and 7 indicate that only the index of the single field whose value is 1f was
            // sent, and (maxIndex - 4) is the correct index for that field.
            if (maxIndex >= 4 && maxIndex <= 7)
            {
                var x = (maxIndex == 4) ? 1f : 0f;
                var y = (maxIndex == 5) ? 1f : 0f;
                var z = (maxIndex == 6) ? 1f : 0f;
                var w = (maxIndex == 7) ? 1f : 0f;

                Reader.ReadByte();
                Reader.ReadByte();
                Reader.ReadByte();
                return new Quaternion(x, y, z, w);
            }

            // Read the other three fields and derive the value of the omitted field
            var a = Reader.ReadInt16() / FLOAT_PRECISION_MULT;
            var b = Reader.ReadInt16() / FLOAT_PRECISION_MULT;
            var c = Reader.ReadInt16() / FLOAT_PRECISION_MULT;
            var d = (float)Math.Sqrt(1f - (a * a + b * b + c * c));

            if (maxIndex == 0)
                return new Quaternion(d, a, b, c);
            else if (maxIndex == 1)
                return new Quaternion(a, d, b, c);
            else if (maxIndex == 2)
                return new Quaternion(a, b, d, c);

            return new Quaternion(a, b, c, d);
        }

        public Quaternion ReadSmallerCompressedQuaternion()
        {
            return new Quaternion(Reader.ReadSByte() / 127.0f, Reader.ReadSByte() / 127.0f, Reader.ReadSByte() / 127.0f, Reader.ReadSByte() / 127.0f);
        }
    }
}
