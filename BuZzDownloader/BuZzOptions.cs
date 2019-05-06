using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using BuZzDownloader.Properties;

namespace BuZzDownloader
{
    /// <summary>
    /// Options for BuZzDownloader
    /// </summary>
    struct BuZzOptions
    {
        public bool SkipListDownload { get; set; }
        public string BasketUrl { get; set; }
        public string BasketLocalFile { get; set; }
        public string BasketLocalDir { get; set; }

        /// <summary>
        /// Retrieve and save user settings
        /// Also setup network configuration
        /// </summary>
        public static void HandleCredentials()
        {
            // Initial setup
            if (Settings.Default.BuZzLogin == Settings.Default.BuZzPwd)
            {
                Console.Write("Login: ");
                Settings.Default.BuZzLogin = Console.ReadLine();
                Console.Write("Password: ");
                Settings.Default.BuZzPwd = PasswordReadline();
                Settings.Default.Save();
                Console.WriteLine("Credentials saved");
            }
            else
            {
                Console.WriteLine("Credentials loaded, welcome {0}!", Settings.Default.BuZzLogin);
            }

            Console.WriteLine();
        }

        private static string PasswordReadline()
        {
            string pass = string.Empty;
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return pass;
        }

        public static ICredentials GetCredentials()
        {
            return new NetworkCredential(Settings.Default.BuZzLogin, Settings.Default.BuZzPwd);
        }

        /// <summary>
        /// Retrieve options (get-opt)
        /// </summary>
        public bool FillData(string[] args)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assemblyLocation);
            string basketFile = null;

            Console.WriteLine("=== BuZzDownloader v{0} ===", fvi.FileVersion);
            Console.WriteLine();

            switch (args.Length)
            {
                case 1:
                    var arg = args[0];

                    // Reset credentials
                    if (arg == "/reset")
                    {
                        Settings.Default.Reset();
                    }
                    else // Show usage
                    {
                        string curExe = Process.GetCurrentProcess().MainModule.FileName;

                        Console.WriteLine("Usage:\t{0} [/reset]", Path.GetFileName(curExe));
                        Console.WriteLine("\t/reset:\tReset credentials");
                        return true;
                    }

                    break;
            }

            // File system management
            BasketUrl = Settings.Default.BuZzUrl + "basket/list";
            basketFile = Path.Combine(Settings.Default.DownloadFolder, "basket.list");
            BasketLocalFile = Path.GetFullPath(basketFile);
            BasketLocalDir = Path.GetDirectoryName(basketFile);

            if (!Directory.Exists(BasketLocalDir))
                Directory.CreateDirectory(BasketLocalDir);

            if (File.Exists(BasketLocalFile))
            {
                Console.Write("List file exists, overwrite? [y/N] ");
                var input = Console.ReadLine();

                if (input == null || !input.Equals("y", StringComparison.InvariantCultureIgnoreCase))
                    SkipListDownload = true;

                Console.WriteLine("{0} list file download...", SkipListDownload ? "Skipping" : "Performing");
                Console.WriteLine();
            }

            return false;
        }

        /// <summary>
        /// Download a single file, with status
        /// </summary>
        public static void DownloadFile(string url, WebClient client)
        {
            var uri = new Uri(url);
            string fileLocalFilename = Path.GetFileName(uri.LocalPath);
            Console.WriteLine("Start downloading {0}...", fileLocalFilename);
            string localPath = Settings.Default.DownloadFolder + uri.LocalPath;
            string fileLocalFile = Path.GetFullPath(localPath);
            string fileLocalDir = Path.GetDirectoryName(localPath);
            int statusFrequency = Settings.Default.StatusFrequency;
            long distantSize, localSize = 0;
            FileInfo fi = null;

            if (!Directory.Exists(fileLocalDir))
                Directory.CreateDirectory(fileLocalDir);

            using (Stream s = client.OpenRead(url))
                distantSize = long.Parse(client.ResponseHeaders["Content-Length"]);

            // Check size
            if (File.Exists(fileLocalFile))
            {
                fi = new FileInfo(fileLocalFile);
                localSize = fi.Length;

                // Skip downloaded file
                if (distantSize == localSize)
                {
                    Console.WriteLine("File already completed, skipping...");
                    return;
                }

                localSize = 0;
            }

            // Status update
            bool shown = false;
            var t = new Timer(o =>
            {
                if (fi == null)
                    fi = new FileInfo(fileLocalFile);

                fi.Refresh();

                double percent = Math.Round(fi.Length / (double)distantSize * 100, 2);
                const string f = "Status:\t{0:#0.##}%\t{1:#0.##} kB/{2:#0.##} kB\t~{3:#0.##} kB/s\t\t\r";
                var speed = (fi.Length - localSize) / (statusFrequency * 1024d);

                localSize = fi.Length;
                Console.Write(f, percent, localSize / 1024d, distantSize / 1024d, speed);
                shown = true;
            }, null, statusFrequency * 1000, statusFrequency * 1000);

            client.DownloadFile(url, fileLocalFile);
            t.Change(Timeout.Infinite, Timeout.Infinite);

            if (shown)
                Console.WriteLine();

            Console.WriteLine("Download complete!");
        }
    }
}