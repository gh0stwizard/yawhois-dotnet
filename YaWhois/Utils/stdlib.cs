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
    internal static unsafe class stdlib
    {
        /// <summary>
        /// 0UL + LONG_MIN: no critic.
        /// </summary>
        private const ulong UL_LONG_MIN = 9223372036854775808;


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
                throw new ArgumentException("base");

            while (isspace(c = (byte)f.ReadByte())) ;

            if (c == '+' || c == '-')
            {
                neg = c == '-' ? -1 : 0;
                c = (byte)f.ReadByte();
            }

            if ((@base == 0 || @base == 16) && c == '0')
            {
                c = (byte)f.ReadByte();
                if ((c | 32) == 'x')
                {
                    c = (byte)f.ReadByte();
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
                    //errno = EINVAL;
                    return 0;
                }
            }

            if (@base == 10)
            {
                for (x = 0; c - '0' < 10u && x < uint.MaxValue / 10 - 1; c = (byte)f.ReadByte())
                    x = x * 10 + c - '0';
                for (y = x; c - '0' < 10u && y < ulong.MaxValue / 10 && 10 * y <= ulong.MaxValue - (uint)(c - '0'); c = (byte)f.ReadByte())
                    y = y * 10 + c - '0';
                if (c - '0' >= 10u) goto done;
            }
            else if ((@base & (@base - 1)) > 0)
            {
                int bs = "\0\x31\x32\x34\x37\x33\x36\x35"[(0x17 * (int)@base) >> 5 & 7];
                for (x = 0; table[c] < @base && x <= uint.MaxValue / 32; c = (byte)f.ReadByte())
                    x = x << bs | table[c];
                for (y = x; table[c] < @base && y <= ulong.MaxValue >> bs; c = (byte)f.ReadByte())
                    y = y << bs | table[c];
            }
            else
            {
                for (x = 0; table[c] < @base && x <= uint.MaxValue / 36 - 1; c = (byte)f.ReadByte())
                    x = x * @base + table[c];
                for (y = x; table[c] < @base && y <= ulong.MaxValue / @base && @base * y <= ulong.MaxValue - table[c]; c = (byte)f.ReadByte())
                    y = y * @base + table[c];
            }

            if (table[c] < @base)
            {
                for (; table[c] < @base; c = (byte)f.ReadByte()) ;
                //errno = ERANGE;
                y = lim;
                if ((lim & 1) > 0) neg = 0;
            }

        done:
            f.Position--;
            if (y >= lim)
            {
                if ((lim & 1) == 0 && neg != 0)
                {
                    //errno = ERANGE;
                    return lim - 1;
                }
                else if (y > lim)
                {
                    //errno = ERANGE;
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


        public static ulong strtox(string s, out long end, int @base, ulong lim)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                end = 0;
                return 0;
            }

            var bytes = Encoding.ASCII.GetBytes(s);
            using (var stream = new MemoryStream(bytes))
            {
                var y = intscan(stream, (uint)@base, 1, lim);
                end = stream.Position;
                return y;
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long strtol(string s, out long end, int @base)
        {
            return (long)strtox(s, out end, @base, UL_LONG_MIN);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long strtol(string s, int @base)
        {
            return (long)strtox(s, out long _, @base, UL_LONG_MIN);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint strtoul(string s, out long end, int @base)
        {
            return (uint)strtox(s, out end, @base, ulong.MaxValue);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint strtoul(string s, int @base)
        {
            return (uint)strtox(s, out long _, @base, ulong.MaxValue);
        }
    }
}
