using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace WpfClient.Helpers
{
    public class ImageEncodingHelper
    {
        public string GetFileName()
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                DefaultExt = ".png",
                Filter = "PNG Files (*.png)|*.png"
            };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                return dlg.FileName;
            }

            return null;
        }

        public byte[] PngImageToByteArray(string fileName)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapSource bSource = new BitmapImage(new Uri(fileName));

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bSource));
                encoder.Save(stream);

                return stream.ToArray();
            }
        }
    }
}
