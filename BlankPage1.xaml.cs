using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System;
using System.Threading.Tasks;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Браузер
{

    public static class MainFavoriteLink
    {
        public static string Link;
        public static string Name;
    }

    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {

    //    List<Favorite> favorites = new List<Favorite>(new Favorite[] { new Favorite() { Name = "Google", Link = "https://www.google.com/" }, new Favorite() { Name = "Yandex", Link = "https://www.yandex.ru/" }});
       
    public BlankPage1()
    {
            this.InitializeComponent();
            this.RequestedTheme = Theme1.theme;
            if (Theme1.theme == ElementTheme.Light)
            {
                Theme.IsOn = true;
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (Theme.IsOn == true)
            {
                Theme1.theme = ElementTheme.Light;
            }
            else
            {
                Theme1.theme = ElementTheme.Dark;
            }
            this.RequestedTheme = Theme1.theme;
        }
      List<Favorite> favorites = new List<Favorite>();
        private async Task<List<Favorite>> GetFavorites()
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync("users3.xml", CreationCollisionOption.OpenIfExists);
            //XmlSerializer xml = new XmlSerializer(typeof(List<User>));

            Stream stream = await file.OpenStreamForReadAsync();
            
            return (List<Favorite>)new XmlSerializer(typeof(List<Favorite>)).Deserialize(stream);
            stream.Close();
        }


        private async void ListView_Tapped(object sender, TappedRoutedEventArgs e)
        {

            //StorageFolder folder = ApplicationData.Current.LocalFolder;
            //StorageFile file = await folder.CreateFileAsync("users3.xml", CreationCollisionOption.OpenIfExists);
            ////XmlSerializer xml = new XmlSerializer(typeof(List<User>));

            //Stream stream = await file.OpenStreamForReadAsync();
            //favorites = (List<Favorite>)new XmlSerializer(typeof(List<Favorite>)).Deserialize(stream);
            favorites = await GetFavorites();
            Bindings.Update();
            ListViewFavorite.UpdateLayout();
            if (ListViewFavorite.Items.Count != 0)
            {
                MainFavoriteLink.Link = (ListViewFavorite.SelectedItem as Favorite).Link.ToString();
                MainFavoriteLink.Name = (ListViewFavorite.SelectedItem as Favorite).Name.ToString();
                Bindings.Update();

                Frame.GoBack();
            }
        }

    
    }
}
