﻿using AForge.Video.DirectShow;
using AForge.Video;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using System.IO;
namespace Gym_Membership
{
    public partial class Camera : Window
    {
        private VideoCaptureDevice videoSource;
        public string CapturedImagePath { get; private set; }

        public Camera()
        {
            InitializeComponent();
            InitializeCamera();
        }

        private void InitializeCamera()
        {
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                MessageBox.Show("No video devices found");
                return;
            }

            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += VideoSource_NewFrame;
            videoSource.Start();
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                BitmapImage bitmapImage = ConvertToBitmapImage(eventArgs.Frame);
                Dispatcher.Invoke(() => cameraImage.Source = bitmapImage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying frame: {ex.Message}");
            }
        }


        private BitmapImage ConvertToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            using (var memory = new System.IO.MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }

        private void CaptureImage_Click(object sender, RoutedEventArgs e)
        {
            if (cameraImage.Source is BitmapSource bitmapSource)
            {
                CapturedImagePath = SaveBitmapSource(bitmapSource);
                this.DialogResult = true;
                this.Close();
            }
        }

        private string SaveBitmapSource(BitmapSource bitmapSource)
        {
            string targetFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CapturedImages");
            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
            }

            string filePath = System.IO.Path.Combine(targetFolderPath, $"{Guid.NewGuid()}.png");
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(stream);
            }
            return filePath;
        }

        private void StopCamera()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.NewFrame -= VideoSource_NewFrame;
                videoSource = null;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopCamera();
        }
    }
}
