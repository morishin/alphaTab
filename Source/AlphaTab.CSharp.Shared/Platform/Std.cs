﻿/*
 * This file is part of alphaTab.
 * Copyright (c) 2014, Daniel Kuschny and Contributors, All rights reserved.
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or at your option any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using AlphaTab.IO;
using AlphaTab.Util;
using AlphaTab.Xml;

namespace AlphaTab.Platform
{
    public static partial class Std
    {
        /// <summary>
        /// gen
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T As<T>(this object s)
        {
            return (T) s;
        }

        public static string GetCallerName()
        {
            var frame = new StackFrame(3, true);
            var method = frame.GetMethod();
            var lineNumber = frame.GetFileLineNumber();
            return string.Format("{0}.{1}:{2}", method.DeclaringType.FullName, method.Name, lineNumber);
        }


        public static void Log(string msg, LogLevel logLevel)
        {
            var color = Console.ForegroundColor;
            switch (logLevel)
            {
                case LogLevel.None:
                    break;
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }

            Console.WriteLine(msg);
            Debug.WriteLine(msg);
            Console.ForegroundColor = color;
        }

        public static float ParseFloat(string s)
        {
            float f;
            if (!Single.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out f))
            {
                f = Single.NaN;
            }
            return f;
        }

        public static float Log2(float s)
        {
            return (float)Math.Log(s, 2);
        }


        public static int ParseInt(int s)
        {
            return s;
        }

        public static int ParseInt(string s)
        {
            float f;
            if (!float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out f))
            {
                return int.MinValue;
            }
            return (int) f;
        }

        public static int[] CloneArray(int[] array)
        {
            return (int[]) array.Clone();
        }

        public static void BlockCopy(byte[] src, int srcOffset, byte[] dst, int dstOffset, int count)
        {
            Buffer.BlockCopy(src, srcOffset, dst, dstOffset, count);
        }

        public static bool IsNullOrWhiteSpace(this string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }

        public static string StringFromCharCode(int c)
        {
            return ((char) c).ToString();
        }

        public static void Foreach<T>(IEnumerable<T> e, Action<T> c)
        {
            foreach (var t in e)
            {
                c(t);
            }
        }

        public static sbyte ReadSignedByte(this IReadable readable)
        {
            return unchecked((sbyte) (byte) readable.ReadByte());
        }

        public static string ToString(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        public static bool InstanceOf<T>(object value)
        {
            return value is T;
        }

        public static string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static bool IsException<T>(Exception e)
        {
            return e is T;
        }


        private static readonly Random Rnd = new Random();
        public static int Random(int max)
        {
            return Rnd.Next(max);
        }
    }
}