using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace WpfCustomerService
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Ctor 
        public MainWindow()
        {
            InitializeComponent();
            BtnHome_OnClick(null, null);
            _counter = 0;
            string deskTop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string SavingFolder = deskTop + Path.DirectorySeparatorChar + "SplitImager";
            Directory.CreateDirectory(SavingFolder);
            InputSavePathImagePanel.Text = InputSaveFolderImagePathPanelImages.Text = SavingFolder;
        }

        // Feild
        private int _counter;

        // All Methods ................................................
        private void BtnHome_OnClick(object sender, RoutedEventArgs e)
        {
            CollapseVisbilePanell("Home");
        }

        #region Methods Of PanelImage

        /// <summary>
        ///     Show Image Panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnImage_OnClick(object sender, RoutedEventArgs e)
        {
            CollapseVisbilePanell("Image");
        }

        /// <summary>
        ///     Split Selected Image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnSplitImagePanelImage_OnClick(object sender, RoutedEventArgs e)
        {
            //   BtnSplitImagePanelImage.IsEnabled = false;
            string row = InputRowInputImagePanel.Text;
            string column = InputColumnInputImagePanel.Text;
            string saveFolder = InputSavePathImagePanel.Text;
            string imgPath = InputImagePathImagePanel.Text;
            try
            {
                if (imgPath != "" && column != "" &&
                    row != "" && int.Parse(row) > 0 &&
                    int.Parse(column) > 0)
                {
                    txtStatusImgPanel.Text = "Please Wating We Are Working On Your Image ...";
                    await WorkOnImage(imgPath, saveFolder, column, row);
                    txtStatusImgPanel.Text = "Finished ...";
                    txtStatusImgPanel.Foreground = new SolidColorBrush(Colors.DarkGreen);
                    MessageBox.Show("Operation Successfully ...", "Success", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                row = column = saveFolder = imgPath = null;
            }
        }

        private void BtnSelectSaveFolderPanelImage_OnClick(object sender, RoutedEventArgs e)
        {
            string result = methodFolderBrowserDialog();
            if (result != null) InputSavePathImagePanel.Text = result;
        }

        private void BtnOpenSaveFolderPanelImage_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFolderWithPath(InputSavePathImagePanel.Text);
        }

        private void BtnSelectImagePanelImage_OnClick(object sender, RoutedEventArgs e)
        {
            string result = methodOpenFileDialog();
            if (result != null) InputImagePathImagePanel.Text = result;
        }

        #endregion

        #region Methods Of PanelImages

        private void BtnImages_OnClickges_OnClick(object sender, RoutedEventArgs e)
        {
            CollapseVisbilePanell("Images");
        }

        private void BtnShowSaveFolderIms_PanelImages_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFolderWithPath(InputSaveFolderImagePathPanelImages.Text);
        }

        private void BtnSelectSaveFolderIms_PanelImages_OnClick(object sender, RoutedEventArgs e)
        {
            string result = methodFolderBrowserDialog();
            if (result != null) InputSaveFolderImagePathPanelImages.Text = result;
        }

        private void BtnSelectFolderIms_PanelImages_OnClick(object sender, RoutedEventArgs e)
        {
            string result = methodFolderBrowserDialog();
            if (result != null) InputFolderImagePathPanelImages.Text = result;
        }

        private async void BtnSplitIms_PanelImages_OnClick(object sender, RoutedEventArgs e)
        {
            string row = InputRowImagePanelImages.Text;
            string column = InputColumnImagePanelImages.Text;
            string saveFolder = InputSaveFolderImagePathPanelImages.Text;
            string folderImages = InputFolderImagePathPanelImages.Text;
            if (!string.IsNullOrEmpty(row) && !string.IsNullOrEmpty(column) && !string.IsNullOrEmpty(saveFolder) &&
                !string.IsNullOrEmpty(folderImages))
                await forEachImageInFolder(row, column, saveFolder, folderImages);
            else
                MessageBox.Show("Please Fill Input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            row = column = saveFolder = folderImages = null;
        }

        #endregion

        // ------------------------
        /// <summary>
        ///     Collaspse Unwanted Panel
        /// </summary>
        /// <param name="panel">Enter The Panel You Want To See</param>
        // **** Methods of Program ****

        #region Methods

        private async Task forEachImageInFolder(string row, string column, string saveFolder, string folderImages)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(folderImages);
            if (directoryInfo.Exists)
                try
                {
                    txtStatusPanelImages.Text = "We Are Working On Your Image";
                    List<FileInfo> files = new List<FileInfo>();
                    string[] diretoryFilePath = Directory.GetFiles(folderImages);
                    foreach (string file in diretoryFilePath) files.Add(new FileInfo(file));

                    foreach (FileInfo file in files)
                        if (file.Extension == ".jpg" || file.Extension == ".png" || file.Extension == ".webp")
                            await WorkOnImage(file.FullName, saveFolder, column, row);
                    BtnSplitIms_PanelImages.IsEnabled = true;
                    txtStatusPanelImages.Text = "Finished ...";
                    txtStatusPanelImages.Foreground = new SolidColorBrush(Colors.DarkGreen);
                    MessageBox.Show("Operation Successfully ...", "Success", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
                catch (Exception e)
                {
                    txtStatusPanelImages.Text = "Operation Failed ...";
                    MessageBox.Show(e.Message);
                }
            else
                MessageBox.Show("Directory Not Exit !", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    List<Image> list = new List<Image>();
            //    List<Image> list1 = new List<Image>();
            //    var str = Directory.GetFiles(txtFilePath.Text);
            //    foreach (string item in str)
            //    {
            //        list1.Add(new Bitmap(item));
            //    }
        }

        /// <summary>
        ///     This Method Can Create Image With Row And Column The User Entered ...
        ///     Then Save In Folder User Entered Or Program Created
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="savePath"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private async Task WorkOnImage(string imagePath, string savePath, string column, string row)
        {
            try
            {
                BtnSplitImagePanelImage.IsEnabled = false;
                BtnSplitIms_PanelImages.IsEnabled = false;
                Image img = new Bitmap(imagePath);

                for (int i = 0; i < int.Parse(column); i++)
                for (int y = 0; y < int.Parse(row); y++)
                {
                    Rectangle r = new Rectangle(i * (img.Width / int.Parse(column)),
                        y * (img.Height / int.Parse(row)),
                        img.Width / int.Parse(column),
                        img.Height / int.Parse(row));


                    await Task.Run(() =>
                    {
                        Dispatcher.Invoke(() =>
                        {
                            Image crop = CropImage(img, r).Result;
                            crop.Save(savePath +
                                      string.Format(@"/SplitImager {0}.jpg", _counter));
                            crop.Dispose();
                        });
                    });
                    _counter++;
                }

                img.Dispose();
                BtnSplitImagePanelImage.IsEnabled = true;
            }
            catch (Exception e)
            {
                BtnSplitImagePanelImage.IsEnabled = true;
            }
        }

        //  Create A Bit Map For Rectalce Area
        private static async Task<Bitmap> CropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea, PixelFormat.DontCare);
            return bmpCrop;
        }

        // Show Hide Panel
        private void CollapseVisbilePanell(string panel)
        {
            HowUsePanel.Visibility =
                HomePanel.Visibility = ImagePanel.Visibility = ImagesPanel.Visibility = Visibility.Collapsed;
            if (panel == "Home")
                HomePanel.Visibility = Visibility.Visible;
            else if (panel == "Image")
                ImagePanel.Visibility = Visibility.Visible;
            else if (panel == "Images") ImagesPanel.Visibility = Visibility.Visible;
            else if (panel == "HowUseProgram") HowUsePanel.Visibility = Visibility.Visible;
        }

        private void OpenFolderWithPath(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                    Process.Start("explorer.exe", path);
                else
                    MessageBox.Show("Invalid Path", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region DialogMethod

        /// <summary>
        ///     Create FolderBrowserDialog And Return Path Select Folder
        /// </summary>
        /// <returns>Return Path Selected Folder</returns>
        private string methodFolderBrowserDialog()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            DialogResult dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK) return folderBrowserDialog.SelectedPath;
            return null;
        }

        /// <summary>
        ///     OpenFileDialog And Return Selected File Path , If Dont Select Any File It Return Null
        /// </summary>
        /// <returns>Return Path File Or not Return Null</returns>
        private string methodOpenFileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Reset();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Jpg - Webp - png Format |*.jpg;*.png;*.webp;";
            openFileDialog.Title = "Select Image";
            bool? result = openFileDialog.ShowDialog();
            if (result == true)
                return openFileDialog.FileName;
            return null;
        }

        #endregion

        #endregion

        private void BtnHowUse_OnClick(object sender, RoutedEventArgs e)
        {
            CollapseVisbilePanell("HowUseProgram");
        }
    }
}