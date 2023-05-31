using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.ComponentModel;

namespace передача_файлов
{
    class Client
    {
        static readonly object sync = new object();
        static readonly ConsoleColor[] Gradient =
        {
    ConsoleColor.Red,
    ConsoleColor.DarkYellow,
    ConsoleColor.Yellow,
    ConsoleColor.DarkGreen,
    ConsoleColor.Green
    };

        static readonly string[] Suffixes =
        {
    "B",
    "KB",
    "MB",
    "TB"
         };

        static void Main(string[] args)
        {
            var http = new WebClient();
            http.DownloadProgressChanged += ProgressCallback;

            Console.CursorVisible = false;
            Console.WriteLine("качаем");
            http.DownloadFileTaskAsync("https://vk.com/video_ext.php?oid=750406305&id=456239983&hash=f990a4baccee1eac&hd=2", @"d:\").Wait();

            Console.ReadLine();
        }

        private static void CompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            Console.SetCursorPosition(0, Console.CursorTop + 1);
            Console.ResetColor();
            Console.WriteLine("Done!");
        }

        private static void ProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            var color = Gradient[(int)(e.ProgressPercentage / 100.0 * (Gradient.Length - 1))];
            var progress = string.Format("|{0,-30}| {1,3}% {2,9} / {3,-9}",
                new string((char)0x2592, e.ProgressPercentage * 30 / 100),
                e.ProgressPercentage,
                FormatSize(e.BytesReceived),
                FormatSize(e.TotalBytesToReceive));

            lock (sync)
            {
                Console.SetCursorPosition(0, 1);
                Console.ForegroundColor = color;
                Console.Write(progress);
            }
        }

        static string FormatSize(double size)
        {
            int grade = (int)Math.Log(size, 1024);
            grade = Math.Min(grade, Suffixes.Length);

            return string.Format("{0:N2}{1}", size / Math.Pow(1024, grade), Suffixes[grade]);
        }
    }
}
