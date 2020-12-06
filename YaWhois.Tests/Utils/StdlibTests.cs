using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using YaWhois.Utils;
using NUnit.Framework;

namespace YaWhois.Tests.Utils
{
    public class StdlibTests
    {
        private const string Number = "12345";
        private const string OverflowNumber = "18446744073709551616";


        [TestCase(null)]
        [TestCase("")]
        [TestCase(" \t  ")]
        public void NullOrEmptyOrWhiteSpace(string value)
        {
            stdlib.errno = 0;
            var r = stdlib.strtol(value, out long end, 10);
            Assert.AreEqual(0, r);
            Assert.AreEqual(0, end);
            Assert.AreEqual((int)stdlib.ErrorCodes.NONE, stdlib.errno);
        }


        [TestCase(null)]
        [TestCase("")]
        [TestCase(" \t  ")]
        public void NullOrEmptyOrWhiteSpaceUnsinged(string value)
        {
            stdlib.errno = 0;
            var r = stdlib.strtoul(value, out long end, 10);
            Assert.AreEqual(0, r);
            Assert.AreEqual(0, end);
            Assert.AreEqual((int)stdlib.ErrorCodes.NONE, stdlib.errno);
        }


        [TestCase(37)]
        [TestCase(1)]
        public void InvalidBase(int @base)
        {
            stdlib.errno = 0;
            var r = stdlib.strtol(Number, out long end, @base);
            Assert.AreEqual(0, r);
            Assert.AreEqual(0, end);
            Assert.AreEqual((int)stdlib.ErrorCodes.EINVAL, stdlib.errno);
        }


        [TestCase(37)]
        [TestCase(1)]
        public void InvalidBaseUnsigned(int @base)
        {
            stdlib.errno = 0;
            var r = stdlib.strtoul(Number, out long end, @base);
            Assert.AreEqual(0, r);
            Assert.AreEqual(0, end);
            Assert.AreEqual((int)stdlib.ErrorCodes.EINVAL, stdlib.errno);
        }


        [Test]
        public void DefaultBase()
        {
            stdlib.errno = 0;
            var r = stdlib.strtol(Number, out long end, 0);
            Assert.AreEqual(long.Parse(Number), r);
            Assert.AreEqual(Number.Length - 1, end);
            Assert.AreEqual((int)stdlib.ErrorCodes.NONE, stdlib.errno);
        }


        [Test]
        public void DefaultBaseUnsigned()
        {
            stdlib.errno = 0;
            var r = stdlib.strtoul(Number, out long end, 0);
            Assert.AreEqual(ulong.Parse(Number), r);
            Assert.AreEqual(Number.Length - 1, end);
            Assert.AreEqual((int)stdlib.ErrorCodes.NONE, stdlib.errno);
        }


        [Test]
        public void NegativeNumber()
        {
            stdlib.errno = 0;
            var r = stdlib.strtol("-" + Number, 10);
            Assert.AreEqual(-1 * long.Parse(Number), r);
            Assert.AreEqual((int)stdlib.ErrorCodes.NONE, stdlib.errno);
        }


        [Test]
        public void NegativeNumberUnsignedLong()
        {
            stdlib.errno = 0;
            var r = stdlib.strtox("-" + Number, out long end, 10, ulong.MaxValue);
            var expected = ulong.MaxValue - ulong.Parse(Number) + 1; // 18446744073709539271
            Assert.AreEqual(expected, r);
            Assert.AreEqual(18446744073709539271, r);
            Assert.AreEqual((int)stdlib.ErrorCodes.NONE, stdlib.errno);
        }


        [Test]
        public void NegativeNumberUnsignedInt()
        {
            stdlib.errno = 0;
            var r = stdlib.strtoul("-" + Number, 10);
            var expected = uint.MaxValue - uint.Parse(Number) + 1; // 4294954951
            Assert.AreEqual(expected, r);
            Assert.AreEqual(4294954951, r);
            Assert.AreEqual((int)stdlib.ErrorCodes.NONE, stdlib.errno);
        }


        [TestCase("007")]
        [TestCase("011")]
        public void Base8(string value)
        {
            stdlib.errno = 0;
            var r = stdlib.strtol(value, 0);
            Assert.AreEqual(Convert.ToInt32(value, 8), r);
            Assert.AreEqual((int)stdlib.ErrorCodes.NONE, stdlib.errno);
        }


        [TestCase("0xFF")]
        [TestCase("0X0A")]
        public void Base16(string value)
        {
            stdlib.errno = 0;
            var r = stdlib.strtol(value, 0);
            Assert.AreEqual(Convert.ToInt32(value, 16), r);
            Assert.AreEqual((int)stdlib.ErrorCodes.NONE, stdlib.errno);
        }


        [TestCase("a")]
        [TestCase("10a")]
        public void BaseOverflow(string value)
        {
            stdlib.errno = 0;
            var r = stdlib.strtol(value, 10);
            var numstr = Regex.Replace(value, "[^0-9]", "");
            var expected = numstr.Length == 0 ? 0 : int.Parse(numstr);
            Assert.AreEqual(expected, r);

            if (expected == 0)
                Assert.AreEqual((int)stdlib.ErrorCodes.EINVAL, stdlib.errno);
            else
                Assert.AreEqual((int)stdlib.ErrorCodes.NONE, stdlib.errno);
        }


        [TestCase("JJ", 20 * 20 - 1)]
        public void Base20(string value, int expected)
        {
            stdlib.errno = 0;
            var r = stdlib.strtol(value, 20);
            Assert.AreEqual(expected, r);
            Assert.AreEqual((int)stdlib.ErrorCodes.NONE, stdlib.errno);
        }


        [TestCase("VV", 32 * 32 - 1)]
        public void Base32(string value, int expected)
        {
            stdlib.errno = 0;
            var r = stdlib.strtol(value, 32);
            Assert.AreEqual(expected, r);
            Assert.AreEqual((int)stdlib.ErrorCodes.NONE, stdlib.errno);
        }


        [Test]
        public void Overflow()
        {
            stdlib.errno = 0;
            var r = stdlib.strtol(OverflowNumber, 10);
            Assert.AreEqual(long.MaxValue, r);
            Assert.AreEqual((int)stdlib.ErrorCodes.ERANGE, stdlib.errno);
        }


        [Test]
        public void OverflowUnsigned()
        {
            stdlib.errno = 0;
            var r = stdlib.strtoul(OverflowNumber, 10);
            Assert.AreEqual(uint.MaxValue, r);
            Assert.AreEqual((int)stdlib.ErrorCodes.ERANGE, stdlib.errno);
        }
    }
}
