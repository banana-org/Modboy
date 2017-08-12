// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <WebService.cs>
//  Created By: Alexey Golub
//  Date: 14/02/2016
// ------------------------------------------------------------------ 

using NegativeLayer.WebToolkit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using Modboy.Models.EventArgs;

namespace Modboy.Services
{
    public class WebService
    {
        private static readonly HttpClient.RetryPolicy RetryPolicy = new HttpClient.RetryPolicy(10,
            TimeSpan.FromMilliseconds(200));

        private double _progress;
        public double Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(value));
            }
        }

        /// <summary>
        /// Event that triggers when the progress changes
        /// </summary>
        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        /// <summary>
        /// Function, that will be periodically executed to determine if an abort is issued
        /// </summary>
        public Func<bool> AbortChecker { get; set; } = () => false;

        /// <summary>
        /// Creates an HttpClient, appropriate for the current application
        /// </summary>
        private HttpClient CreateClient()
        {
            var client = new HttpClient {UserAgent = Constants.UserAgent};
            client.OnProgressChanged += d => Progress = d;
            client.AbortChecker += AbortChecker;
            return client;
        }

        /// <summary>
        /// Performs a get request and returns the page as string or null in case of fail
        /// </summary>
        public string Get(string url)
        {
            var client = CreateClient();
            return client.Get(url, RetryPolicy);
        }

        /// <summary>
        /// Performs a post request and returns the page as string or null in case of fail
        /// </summary>
        public string Post(string url, Dictionary<string, string> data)
        {
            var client = CreateClient();
            return client.Post(url, data, RetryPolicy);
        }

        /// <summary>
        /// Downloads a file to the destination, returning the destination FileInfo or null in case of fail
        /// </summary>
        public FileInfo Download(string url, string destination)
        {
            var client = CreateClient();
            return client.Download(url, destination, RetryPolicy);
        }

        /// <summary>
        /// Downloads a file to memory stream, returning null in case of fail
        /// </summary>
        public MemoryStream Download(string url)
        {
            var client = CreateClient();
            return client.Download(url, RetryPolicy);
        }

        /// <summary>
        /// Downloads an image to memory
        /// </summary>
        public BitmapImage DownloadImage(string url)
        {
            using (var imageStream = Download(url))
            {
                if (imageStream == null)
                    return null;

                try
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = imageStream;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();
                    image.Freeze();

                    return image;
                }
                catch (Exception ex)
                {
                    Logger.Record("Could not download an image");
                    Logger.Record(ex);

                    return null;
                }
            }
        }
    }
}