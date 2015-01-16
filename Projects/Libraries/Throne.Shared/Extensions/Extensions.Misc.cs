using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using JetBrains.Annotations;

namespace Throne.Framework
{
    public static partial class Extensions
    {
        /// <summary>
        ///     Checks if an IntPtr object is null (0).
        /// </summary>
        /// <param name="ptr">The IntPtr object.</param>
        public static bool Null(this IntPtr ptr)
        {
            return ptr == IntPtr.Zero;
        }

        /// <summary>
        ///     Checks if an UIntPtr object is null (0).
        /// </summary>
        /// <param name="ptr">The IntPtr object.</param>
        public static bool Null(this UIntPtr ptr)
        {
            return ptr == UIntPtr.Zero;
        }

        public static Color Lerp(this Color colour, Color to, float amount)
        {
            float sr = colour.R, sg = colour.G, sb = colour.B;
            float er = to.R, eg = to.G, eb = to.B;
            byte r = (byte)sr.Lerp(er, amount),
                 g = (byte)sg.Lerp(eg, amount),
                 b = (byte)sb.Lerp(eb, amount);
            return Color.FromArgb(r, g, b);
        }

        public static float Lerp(this float start, float end, float amount)
        {
            float difference = end - start;
            float adjusted = difference * amount;
            return start + adjusted;
        }

        public static string WordWrap(this string str, int width)
        {
            int pos, next;
            var sb = new StringBuilder();

            // Lucidity check
            if (width < 1)
                return str;

            // Parse each line of text
            for (pos = 0; pos < str.Length; pos = next)
            {
                // Find end of line
                int eol = str.IndexOf(Environment.NewLine, pos);

                if (eol == -1)
                    next = eol = str.Length;
                else
                    next = eol + Environment.NewLine.Length;

                // Copy this line of text, breaking into smaller lines as needed
                if (eol > pos)
                {
                    do
                    {
                        int len = eol - pos;

                        if (len > width)
                            len = BreakLine(str, pos, width);

                        sb.Append(str, pos, len);
                        sb.Append(Environment.NewLine);

                        // Trim whitespace following break
                        pos += len;

                        while (pos < eol && Char.IsWhiteSpace(str[pos]))
                            pos++;

                    } while (eol > pos);
                }
                else sb.Append(Environment.NewLine); // Empty line
            }

            return sb.ToString();
        }

        /// <summary>
        /// Locates position to break the given line so as to avoid
        /// breaking words.
        /// </summary>
        /// <param name="text">String that contains line of text</param>
        /// <param name="pos">Index where line of text starts</param>
        /// <param name="max">Maximum line length</param>
        /// <returns>The modified line length</returns>
        public static int BreakLine(this string text, int pos, int max)
        {
            // Find last whitespace in line
            int i = max - 1;
            while (i >= 0 && !Char.IsWhiteSpace(text[pos + i]))
                i--;
            if (i < 0)
                return max; // No whitespace found; break at maximum length
            // Find start of whitespace
            while (i >= 0 && Char.IsWhiteSpace(text[pos + i]))
                i--;
            // Return length of text before whitespace
            return i + 1;
        }

        public static string ToHexString(this IntPtr pointer)
        {
            string stringRep = "0x" + pointer.ToString("X");
            return stringRep;
        }

        [StringFormatMethod("str")]
        public static string Interpolate(this string str, params object[] args)
        {
            return String.Format(CultureInfo.InvariantCulture, str, args);
        }

        public static bool IsBetween<T>(this T comparable, T lower, T upper)
            where T : IComparable<T>
        {
            return comparable.CompareTo(lower) >= 0 && comparable.CompareTo(upper) < 0;
        }

        /// <summary>
        ///     Performs an action with one parameter on a single object.
        /// </summary>
        /// <typeparam name="T">Type same as object to use on the action.</typeparam>
        /// <param name="obj"></param>
        /// <param name="act"></param>
        /// <returns>Object after the action.</returns>
        public static T With<T>(this T obj, Action<T> act)
            where T : class
        {
            act(obj);
            return obj;
        }


        public static string Format(this TimeSpan time)
        {
            return
                "{0}{1:00}h {2:00}m {3:00}s {4:00}ms".Interpolate(time.TotalDays > 0 ? (int) time.TotalDays + "d " : "",
                    time.Hours, time.Minutes, time.Seconds, time.Milliseconds);
        }

        public static string FormatMillis(this DateTime time)
        {
            return "{0:00}h {1:00}m {2:00}s {3:00}ms".Interpolate(time.Hour, time.Minute,
                time.Second, time.Millisecond);
        }


        /// <summary>
        ///     Raises event with thread and null-ref safety.
        /// </summary>
        public static void Raise<T>(this Action<T> handler, T args)
        {
            if (handler != null)
                handler(args);
        }

        /// <summary>
        ///     Raises event with thread and null-ref safety.
        /// </summary>
        public static void Raise<T1, T2>(this Action<T1, T2> handler, T1 args1, T2 args2)
        {
            if (handler != null)
                handler(args1, args2);
        }

        /// <summary>
        ///     Raises event with thread and null-ref safety.
        /// </summary>
        public static void Raise<T1, T2, T3>(this Action<T1, T2, T3> handler, T1 args1, T2 args2, T3 args3)
        {
            if (handler != null)
                handler(args1, args2, args3);
        }

        public static String ToTQHex(this Color color)
        {
            return "0x{0}{1}{2}".Interpolate(color.R.ToString("X2"), color.G.ToString("X2"), color.B.ToString("X2"));
        }
    }
}