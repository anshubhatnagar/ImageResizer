using ImageEditor.Entities;
using Microsoft.WindowsAPICodePack.Dialogs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FolderBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;

            var directoryPath = string.Empty;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                directoryPath = dialog.FileName;
            }

            var directoryInfo = new DirectoryInfo(directoryPath);
            var imageFileInfoList = directoryInfo.GetFiles("*.jpg");

            var captureItImages = from f in imageFileInfoList
                                  select new EditableImage
                                  {
                                      FullPath = f.FullName
                                  };

            FolderPathLabel.Content = directoryInfo.FullName;
            Thumbnails.DataContext = captureItImages.ToList();
        }

        private void Thumbnails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedImage = (e.AddedItems[0] as EditableImage);
            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(selectedImage.FullPath);
            bitmap.EndInit();
            SelectedImage.Source = bitmap;
        }

        private void RotateLeft_Click(object sender, RoutedEventArgs e)
        {
            var selectedBitmap = SelectedImage.Source as BitmapImage;
            int currentRotation = (int)selectedBitmap.Rotation;
            int requiredRotation = currentRotation - 1;

            if (requiredRotation < 0)
            {
                requiredRotation = 3;
            }

            if (selectedBitmap != null)
            {
                RotateImage(selectedBitmap, (Rotation)requiredRotation);
            }
        }

        private void RotateRight_Click(object sender, RoutedEventArgs e)
        {
            var selectedBitmap = SelectedImage.Source as BitmapImage;
            int currentRotation = (int)selectedBitmap.Rotation;
            int requiredRotation = currentRotation + 1;

            if (requiredRotation > 3)
            {
                requiredRotation = 0;
            }

            if (selectedBitmap != null)
            {
                RotateImage(selectedBitmap, (Rotation)requiredRotation);
            }
        }

        private void RotateImage(BitmapImage selectedBitmap, Rotation rotation)
        {
            var newBitmap = new BitmapImage();
            newBitmap.BeginInit();
            newBitmap.UriSource = new Uri(selectedBitmap.UriSource.AbsolutePath);
            newBitmap.Rotation = rotation;
            newBitmap.EndInit();
            SelectedImage.Source = newBitmap;
        }
    }
}
