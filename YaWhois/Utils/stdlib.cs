﻿/*
Copyright © 2005-2020 Rich Felker, et al.

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace YaWhois.Utils
{
    // Port of C code from musl. See details here:
    // strtol:
    //   1. http://git.musl-libc.org/cgit/musl/tree/src/stdlib/strtol.c
    //   2. http://git.musl-libc.org/cgit/musl/tree/src/internal/intscan.c
    public static unsafe class stdlib
    {
        // TODO: thread-safety?
        public static int errno;
        public static Encoding encoding = Encoding.ASCII;


        public enum ErrorCodes
        {
            NONE,
            EINVAL,
            ERANGE
        }


        // table moved forward by 1 due C# lacks support of things like that:
        //      const unsigned char *val = table+1;
        private static ReadOnlySpan<byte> table => new byte[256]
        {
            /**/255,255,255, 255,255,255,255, 255,255,255,255, 255,255,255,255,
            255,255,255,255, 255,255,255,255, 255,255,255,255, 255,255,255,255,
            255,255,255,255, 255,255,255,255, 255,255,255,255, 255,255,255,255,
            255,  0,  1,  2,   3,  4,  5,  6,   7,  8,  9,255, 255,255,255,255,

            255,255, 10, 11,  12, 13, 14, 15,  16, 17, 18, 19,  20, 21, 22, 23,
             24, 25, 26, 27,  28, 29, 30, 31,  32, 33, 34, 35, 255,255,255,255,
            255,255, 10, 11,  12, 13, 14, 15,  16, 17, 18, 19,  20, 21, 22, 23,
             24, 25, 26, 27,  28, 29, 30, 31,  32, 33, 34, 35, 255,255,255,255,

            255,255,255,255, 255,255,255,255, 255,255,255,255, 255,255,255,255,
            255,255,255,255, 255,255,255,255, 255,255,255,255, 255,255,255,255,
            255,255,255,255, 255,255,255,255, 255,255,255,255, 255,255,255,255,
            255,255,255,255, 255,255,255,255, 255,255,255,255, 255,255,255,255,

            255,255,255,255, 255,255,255,255, 255,255,255,255, 255,255,255,255,
            255,255,255,255, 255,255,255,255, 255,255,255,255, 255,255,255,255,
            255,255,255,255, 255,255,255,255, 255,255,255,255, 255,255,255,255,
            255,255,255,255, 255,255,255,255, 255,255,255,255, 255,255,255,255
            ,255 // moved forward
        };


        // all errors are skip silently because we don't need `errno` support.
        private static ulong intscan(Stream f, uint @base, int pok, ulong lim)
        {
            byte c;
            int neg = 0;
            uint x;
            ulong y;

            if (@base > 36 || @base == 1)
            {
                errno = (int)ErrorCodes.EINVAL;
                return 0;
            }

            byte ReadChar()
            {
                int b = f.ReadByte();
                if (b >= 0 && b <= 0x7f) return (byte)b;
                return 0xff;
            }

            while (isspace(c = ReadChar())) ;

            if (c == '+' || c == '-')
            {
                neg = c == '-' ? -1 : 0;
                c = ReadChar();
            }

            if ((@base == 0 || @base == 16) && c == '0')
            {
                c = ReadChar();
                if ((c | 32) == 'x')
                {
                    c = ReadChar();
                    if (table[c] >= 16)
                    {
                        f.Position--;
                        if (pok != 0) f.Position--;
                        else f.Seek(0, SeekOrigin.Begin);
                        return 0;
                    }
                    @base = 16;
                }
                else if (@base == 0)
                {
                    @base = 8;
                }
            }
            else
            {
                if (@base == 0) @base = 10;
                if (table[c] >= @base)
                {
                    f.Position--;
                    f.Seek(0, SeekOrigin.Begin);
                    errno = (int)ErrorCodes.EINVAL;
                    return 0;
                }
            }

            if (@base == 10)
            {
                for (x = 0; ((uint)c - '0') < 10u && x <= uint.MaxValue / 10 - 1; c = ReadChar())
                    x = x * 10 + (uint)(c - '0');
                for (y = x; ((uint)c - '0') < 10u && y <= ulong.MaxValue / 10 && 10 * y <= ulong.MaxValue - (uint)(c - '0'); c = ReadChar())
                    y = y * 10 + (uint)(c - '0');
                if (((uint)c - '0') >= 10u) goto done;
            }
            else if ((@base & @base - 1) == 0) // bases: 2, 4, 8, 16, 32
            {
                int bs = "\0\x01\x02\x04\x07\x03\x06\x05"[(0x17 * (int)@base) >> 5 & 7];
                for (x = 0; table[c] < @base && x <= uint.MaxValue / 32; c = ReadChar())
                    x = x << bs | table[c];
                for (y = x; table[c] < @base && y <= ulong.MaxValue >> bs; c = ReadChar())
                    y = y << bs | table[c];
            }
            else
            {
                for (x = 0; table[c] < @base && x <= uint.MaxValue / 36 - 1; c = ReadChar())
                    x = x * @base + table[c];
                for (y = x; table[c] < @base && y <= ulong.MaxValue / @base && @base * y <= ulong.MaxValue - table[c]; c = ReadChar())
                    y = y * @base + table[c];
            }

            if (table[c] < @base)
            {
                for (; table[c] < @base; c = ReadChar()) ;
                errno = (int)ErrorCodes.ERANGE;
                y = lim;
                if ((lim & 1) > 0) neg = 0;
            }

        done:
            f.Position--;
            if (y >= lim)
            {
                if ((lim & 1) == 0 && neg != 0)
                {
                    errno = (int)ErrorCodes.ERANGE;
                    return lim - 1;
                }
                else if (y > lim)
                {
                    errno = (int)ErrorCodes.ERANGE;
                    return lim;
                }
            }

            return (y ^ (uint)neg) - (uint)neg;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool isspace(int c)
        {
            return c == ' ' || c - '\t' < 5;
        }


        public static ulong strtox(string s, out int end, int @base, ulong lim)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                end = 0;
                return 0;
            }

            var bytes = encoding.GetBytes(s);
            using (var stream = new MemoryStream(bytes))
            {
                var y = intscan(stream, (uint)@base, 1, lim);

                if (Encoding.ASCII.Equals(encoding))
                    end = Convert.ToInt32(stream.Position);
                else
                    end = encoding.GetCharCount(bytes, 0, Convert.ToInt32(stream.Position));

                return y;
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int strtol(string s, out int end, int @base)
        {
            return (int)strtox(s, out end, @base, int.MaxValue);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int strtol(string s, int @base)
        {
            return (int)strtox(s, out int _, @base, int.MaxValue);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint strtoul(string s, out int end, int @base)
        {
            return (uint)strtox(s, out end, @base, uint.MaxValue);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint strtoul(string s, int @base)
        {
            return (uint)strtox(s, out int _, @base, uint.MaxValue);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long strtoll(string s, out int end, int @base)
        {
            return (long)strtox(s, out end, @base, long.MaxValue);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long strtoll(string s, int @base)
        {
            return (long)strtox(s, out int _, @base, long.MaxValue);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong strtoull(string s, out int end, int @base)
        {
            return strtox(s, out end, @base, ulong.MaxValue);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong strtoull(string s, int @base)
        {
            return strtox(s, out int _, @base, ulong.MaxValue);
        }
    }
}
