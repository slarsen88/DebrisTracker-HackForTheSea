using DebrisCollector.Models;
using Microsoft.WindowsAzure.Storage;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DebrisCollector.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UploadImage : ContentPage
	{
        string imageUrl = string.Empty;
		public UploadImage ()
		{
			InitializeComponent ();
		}

        private async void BtnSubmit_OnClicked(object sender, EventArgs e)
        {
            AddPost addPost = new AddPost()
            {
                ImageURL = imageUrl,
                Size = pickerSize.Items[pickerSize.SelectedIndex],
                Color = pickerColor.Items[pickerColor.SelectedIndex],
                Material = pickerMaterialType.Items[pickerMaterialType.SelectedIndex]
            };

            await App.MobileService.GetTable<AddPost>().InsertAsync(addPost);

            await DisplayAlert("Success", "Upload successful", "OK");

            /* Using SQLite DB on local machine
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            { 
                conn.CreateTable<AddPost>();
                int rows = conn.Insert(addPost);
                conn.Close();

                if (rows > 0)
                {
                    await DisplayAlert("Success", "Upload successful", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Upload failed", "OK");
                }
            }
            */
        }

        private async void CameraButton_OnClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {


                /*
                Directory = "Sample",
                Name = "test.jpg"
                */
            });

            image.Source = ImageSource.FromStream(() => file.GetStream());
            UploadImageFromDevice(file.GetStream());
         
            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }

        //Selecting and uploading photos from gallery to cloud
        private async void SelectImageButton_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await DisplayAlert("Error", "This is not a supported feature on the device", "OK");
                return;
            }

            var mediaOptions = new PickMediaOptions()
            {
                PhotoSize = PhotoSize.Medium
            };

            var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);
            if (selectedImageFile == null)
            {
                await DisplayAlert("Error", "There was an error retrieving this image", "OK");
                return; 
            }

            image.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
            UploadImageFromDevice(selectedImageFile.GetStream());

        }

        private async void UploadImageFromDevice(Stream stream)
        {
            var account = CloudStorageAccount.Parse("");
            var client = account.CreateCloudBlobClient();
            var container = client.GetContainerReference("imagecontainer");
            await container.CreateIfNotExistsAsync();

            var name = Guid.NewGuid().ToString();
            var blockBlob = container.GetBlockBlobReference($"{name}.jpg");
            await blockBlob.UploadFromStreamAsync(stream);
            imageUrl = blockBlob.Uri.OriginalString;
        }
    }
}