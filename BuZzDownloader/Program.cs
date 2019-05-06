using System;
using System.IO;
using System.Net;

namespace BuZzDownloader
{
    /// <summary>
    /// Entry point class
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point method
        /// </summary>
        private static void Main(string[] args)
        {
            var buZzOptions = new BuZzOptions();

            if (buZzOptions.FillData(args))
            {
                return; // Usage shown
            }

            BuZzOptions.HandleCredentials();

            // Connect to BuZz
            try
            {
                using (var client = new WebClient {Credentials = BuZzOptions.GetCredentials()})
                {
                    if (!buZzOptions.SkipListDownload)
                        client.DownloadFile(buZzOptions.BasketUrl, buZzOptions.BasketLocalFile);

                    string[] toDownload = File.ReadAllLines(buZzOptions.BasketLocalFile);

                    foreach (var url in toDownload)
                        BuZzOptions.DownloadFile(url, client);

					var basketFolder = Path.Combine(buZzOptions.BasketLocalDir, "basket");

					File.Delete(buZzOptions.BasketLocalFile);
					Directory.Delete(basketFolder, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("Done, press enter to exit.");
            Console.ReadLine();
        }
    }
}
