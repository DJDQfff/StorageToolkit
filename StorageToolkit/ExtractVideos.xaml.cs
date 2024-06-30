// https://go.microsoft.com/fwlink/?LinkId=234238
// 上介绍了“空白页”项模板



namespace MyToolBox.Views
{
    /// <summary> 可用于自身或导航至 Frame 内部的空白页。 </summary>
    public sealed partial class ExtractVideos : Page
    {
        public ExtractVideos ()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 提取出blbl_merge的子文件夹中的视频，如果这个子文件夹中只有一个视屏，怎提取出来
        /// 这个方法一开始用来提取bili_merge文件夹里面的子视频的，现在不需要这个软件了。
        /// </summary>
        /// <returns></returns>
        private static async Task Bilibili_Merge_Special ()
        {
            var picker = new FolderPicker();
            picker.FileTypeFilter.Add("*");
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Current.MainWindow);
            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(picker , hwnd);

            StorageFolder storageFolder = await picker.PickSingleFolderAsync();

            if (storageFolder != null)
            {
                var folders = await storageFolder.GetFoldersAsync(); // 所有子文件夹
                foreach (var folder in folders)
                {
                    string foldername = folder.Name;

                    var files = await folder.GetFilesAsync();

                    if (files.Count == 1)
                    {
                        await files[0].MoveAsync(
                            storageFolder ,
                            foldername + ".mp4" ,
                            NameCollisionOption.ReplaceExisting
                        );
                        await folder.DeleteAsync();
                    }
                    if (files.Count == 0) // 空文件夹，删除
                    {
                        await folder.DeleteAsync();
                    }
                }
            }
        }

        private async void Button_Click (object sender , RoutedEventArgs e)
        {
            var picker = new FolderPicker();
            picker.FileTypeFilter.Add("*");
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(App.Current.MainWindow);
            // Associate the HWND with the file picker
            WinRT.Interop.InitializeWithWindow.Initialize(picker , hwnd);
            StorageFolder rootFolder = await picker.PickSingleFolderAsync();
            if (rootFolder != null)
            {
                ShowTextBlock.Text = rootFolder.Path;

                var folders = await rootFolder.GetFoldersAsync(); // 所有子文件夹
                foreach (var folder in folders)
                {
                    var files = await folder.GetFilesAsync();
                    foreach (var file in files)
                    {
                        await file.MoveAsync(
                            rootFolder ,
                            file.Name ,
                            NameCollisionOption.GenerateUniqueName
                        );
                    }
                    await folder.DeleteAsync();
                }
            }
        }
    }
}
