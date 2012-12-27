using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketParser;
using Xunit;

namespace PacketParserTests
{
    public class TestRawPacketParser
    {
        [Fact]
        public void HasName()
        {
            var parser = new RawPacketParser();
            Assert.NotEqual(parser.Name, string.Empty);
        }

        [Fact]
        public void ConvertsText()
        {
            string testString = "TestString019! ";
            var parser = new RawPacketParser();
            string result = parser.InterpretPacket((new UTF8Encoding()).GetBytes(testString));
            Assert.Equal(testString, result);
        }

        [Fact]
        public void ConvertsEmptyArray()
        {
            var parser = new RawPacketParser();
            string result = parser.InterpretPacket(new byte[0]);
            Assert.Equal(string.Empty, result);
        }
    }

    public class TestStringPacketParser
    {
        [Fact]
        public void HasName()
        {
            var parser = new StringPacketInterpreter();
            Assert.NotEqual(parser.Name, string.Empty);
        }

        [Fact]
        public void ConvertsText()
        {
            string testString = "TestString019! ";
            var parser = new StringPacketInterpreter();
            string result = parser.InterpretPacket((new UTF8Encoding()).GetBytes(testString));
            Assert.Equal(testString, result);
        }

        [Fact]
        public void ConvertsUnprintables()
        {
            var vals = new byte[0, 1, 4, 7, 0x7F];
            foreach (byte v in vals)
            {
                string testString = new string(new [] { (char)v });
                var parser = new StringPacketInterpreter();
                string result = parser.InterpretPacket((new UTF8Encoding()).GetBytes(testString));
                Assert.Equal(string.Format("[{0:X2}]", v), result);
            }
        }

        [Fact]
        public void ConvertsEmptyArray()
        {
            var parser = new StringPacketInterpreter();
            string result = parser.InterpretPacket(new byte[0]);
            Assert.Equal(string.Empty, result);
        }
    }

    public class TestPythonPacketParser
    {
        [Fact]
        public void CanConstruct()
        {
            Assert.DoesNotThrow(() => { new PythonPacketParser(); });
        }

        [Fact]
        public void HasName()
        {
            var parser = new PythonPacketParser();
            Assert.NotEqual(parser.Name, string.Empty);
        }
    }

}
