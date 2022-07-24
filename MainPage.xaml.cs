using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&cl.. 

namespace Браузер
{
    [Serializable]
    public class Favorite
    {
        public string Name;
        public string Link;
    }

    public static class Theme1
    {
        public static ElementTheme theme = ElementTheme.Dark;
    }
    /// <summary> 
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма. 
    /// </summary> 
    public sealed partial class MainPage : Page
    {   string uri = "";
        public MainPage()
        {
            this.InitializeComponent();
            this.RequestedTheme = Theme1.theme;
            try
            {
                if (MainFavoriteLink.Link != null && MainFavoriteLink.Link.Trim() != "")
                {
                    TabViewItem item = new TabViewItem();
                    item.Header = MainFavoriteLink.Name;
                    WebView web = new WebView();
                    item.Content = web;
                    DFG.TabItems.Add(item);
                    DFG.SelectedItem = item;
                    web.NewWindowRequested += Web_NewWindowRequested;
                    web.DOMContentLoaded += Web_DOMContentLoaded;
                    web.Navigate(new Uri(MainFavoriteLink.Link));
                    MainFavoriteLink.Link = "";
                    MainFavoriteLink.Name = "";
                }
            }
            catch { }
            zapros.PreviewKeyDown += (s, e) =>
            {
                if (e.Key == Windows.System.VirtualKey.F3)
                {
                    var datapack = new DataPackage();
                    datapack.SetText(zapros.Text);
                    Clipboard.SetContent(datapack);
                }
            };
        }

        private void TabView_AddTabButtonClick(TabView sender, object args)
        {
            TabViewItem item = new TabViewItem();
            item.Header = "Новая вкладка";
            WebView web = new WebView();
            item.Content = web;
            DFG.TabItems.Add(item);
            DFG.SelectedItem = item;
            web.NewWindowRequested += Web_NewWindowRequested;
            web.DOMContentLoaded += Web_DOMContentLoaded;
            web.Navigate(new Uri("http://yandex.ru"));



        }

        private void Web_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            (DFG.SelectedItem as TabViewItem).Header = sender.DocumentTitle;
            zapros.Text = args.Uri.OriginalString;
            uri = args.Uri.OriginalString; 
        }
        private void Web_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            TabViewItem item = new TabViewItem();
            item.Header = "Новая вкладка";
            WebView web = new WebView();
            item.Content = web;
            DFG.TabItems.Add(item);
            DFG.SelectedItem = item;
            web.NewWindowRequested += Web_NewWindowRequested;
            web.DOMContentLoaded += Web_DOMContentLoaded;
            web.Navigate(args.Uri);
            args.Handled = true;
        }
        private void DFG_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
        {
            DFG.TabItems.Remove(args.Tab);
            //if(DFG.TabItems.Count == 0) 
            //{ 
            // Application.Current.Exit(); 
            //} 

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (((DFG.SelectedItem as TabViewItem).Content as WebView).CanGoBack)
            {
                ((DFG.SelectedItem as TabViewItem).Content as WebView).GoBack();
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            if (((DFG.SelectedItem as TabViewItem).Content as WebView).CanGoForward)
            {
                ((DFG.SelectedItem as TabViewItem).Content as WebView).GoForward();
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ((DFG.SelectedItem as TabViewItem).Content as WebView).Refresh();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ((DFG.SelectedItem as TabViewItem).Content as WebView).Navigate(new Uri("http://yandex.ru"));
        }

        //private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        //{
        //    {
        //        if (e.Key == Windows.System.VirtualKey.Enter)
        //        {
        //            var r = Getlink(zapros.Text);
        //            ((DFG.SelectedItem as TabViewItem).Content as WebView).Navigate(zapros.Text.Contains("https://") ? new Uri(zapros.Text) : new Uri("https://yandex.ru/search/?text=" + Getlink(zapros.Text)));
        //            zapros.Text = new Uri("https://yandex.ru/search/?text=") + Getlink(zapros.Text);
        //        }
        //    }
        //}
        public static string Getlink(string SearchString)
        {
            //метод словаря 
            var dir = new Dictionary<char, string>
{
{'!',"%21"},
{' ',"%20"},
{',',"%2C"},
{'+',"%2B"},
{'%',"%25"},
{'&',"%26"},
{'Ё',"%A8"},
{'ё',"%B8"},
{'А',"%C0"},
{'Б',"%C1"},
{'В',"%C2"},
{'Г',"%C3"},
{'Д',"%C4"},
{'Е',"%C5"},
{'Ж',"%C6"},
{'З',"%C7"},
{'И',"%C8"},
{'Й',"%C9"},
{'К',"%CA"},
{'Л',"%CB"},
{'М',"%CC"},
{'Н',"%CD"},
{'О',"%CE"},
{'П',"%CF"},
{'Р',"%D0"},
{'С',"%D1"},
{'Т',"%D2"},
{'У',"%D3"},
{'Ф',"%D4"},
{'Х',"%D5"},
{'Ц',"%D6"},
{'Ч',"%D7"},
{'Ш',"%D8"},
{'Щ',"%D9"},
{'Ъ',"%DA"},
{'Ы',"%DB"},
{'Ь',"%DC"},
{'Э',"%DD"},
{'Ю',"%DE"},
{'Я',"%DF"},
{'а',"%E0"},
{'б',"%E1"},
{'в',"%E2"},
{'г',"%E3"},
{'д',"%E4"},
{'е',"%E5"},
{'ж',"%E6"},
{'з',"%E7"},
{'и',"%E8"},
{'й',"%E9"},
{'к',"%EA"},
{'л',"%EB"},
{'м',"%EC"},
{'н',"%ED"},
{'о',"%EE"},
{'п',"%EF"},
{'р',"%F0"},
{'с',"%F1"},
{'т',"%F2"},
{'у',"%F3"},
{'ф',"%F4"},
{'х',"%F5"},
{'ц',"%F6"},
{'ч',"%F7"},
{'ш',"%F8"},
{'щ',"%F9"},
{'ъ',"%FA"},
{'ы',"%FB"},
{'ь',"%FC"},
{'э',"%FD"},
{'ю',"%FE"},
{'я',"%FF"},
{'a',"%61"},
{'b',"%62"},
{'c',"%63"},
{'d',"%64"},
{'e',"%65"},
{'f',"%66"},
{'g',"%67"},
{'h',"%68"},
{'i',"%69"},
{'j',"%6a"},
{'k',"%6b"},
{'l',"%6c"},
{'m',"%6d"},
{'n',"%6e"},
{'o',"%6f"},
{'p',"%70"},
{'q',"%71"},
{'r',"%72"},
{'s',"%73"},
{'t',"%74"},
{'u',"%75"},
{'v',"%76"},
{'w',"%77"},
{'x',"%78"},
{'y',"%79"},
{'z',"%7a"},
{'A',"%41"},
{'B',"%42"},
{'C',"%43"},
{'D',"%44"},
{'E',"%45"},
{'F',"%46"},
{'G',"%47"},
{'H',"%48"},
{'I',"%49"},
{'J',"%4a"},
{'K',"%4b"},
{'L',"%4c"},
{'M',"%4d"},
{'N',"%4e"},
{'O',"%4f"},
{'P',"%50"},
{'Q',"%51"},
{'R',"%52"},
{'S',"%53"},
{'T',"%54"},
{'U',"%55"},
{'V',"%56"},
{'W',"%57"},
{'X',"%58"},
{'Y',"%59"},
{'Z',"%5a"},

};


            //это в коде 

            if (Regex.IsMatch(SearchString, @"[А-Яа-я,Ё,ё]+$") || Regex.IsMatch(SearchString, @"[A-Za-z,Ё,ё]+$")) //ищем кирилицу 
            {
                var dictionary = dir;
                string CorrectedSearchString = "";
                for (int i = 0; i < SearchString.Length; i++)
                {
                    if (dictionary.Keys.Contains(SearchString[i]))
                    {
                        CorrectedSearchString += dictionary[SearchString[i]];
                    }
                    else
                    {
                        CorrectedSearchString += SearchString[i];
                    }
                }
                return CorrectedSearchString;
            }
            return "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(BlankPage1));
        }

        private void zapros_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {
            {
                if (e.Key == Windows.System.VirtualKey.Enter)
                {
                    var r = Getlink(zapros.Text);
                    ((DFG.SelectedItem as TabViewItem).Content as WebView).Navigate(zapros.Text.Contains("https://") ? new Uri(zapros.Text) : new Uri("https://yandex.ru/search/?text=" + Getlink(zapros.Text)));
                    zapros.Text = new Uri("https://yandex.ru/search/?text=") + Getlink(zapros.Text);
                }
            }
        }

        public void FF(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            zapros.Text = args.Uri.OriginalString;
        }
        private async void Favorite1_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).IsEnabled = false;
            List<Favorite> favorites = new List<Favorite>();
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync("users3.xml", CreationCollisionOption.OpenIfExists);
            Favorite favorite = new Favorite()
            {
                Name = (DFG.SelectedItem as TabViewItem).Header.ToString(),
                Link = uri

            };
            XmlSerializer xml = new XmlSerializer(typeof(List<Favorite>));
            Stream stream = await file.OpenStreamForReadAsync();
            try
            {
                favorites = (List<Favorite>)xml.Deserialize(stream);
            }
            catch
            {
                favorites = new List<Favorite>();
            }
            stream.Close();
            favorites.Add(favorite);
            stream = await file.OpenStreamForWriteAsync();
            xml.Serialize(stream, favorites);
            stream.Close();
            zapros.Text = folder.Path; 
            (sender as Button).IsEnabled = true;
        }
       
            // разработать приложение "браузер" с возможностью добавления различных сайтов в избранное, просмотра истории, перейти на эти сайти и отчиски истории, предусмотреть возможность изменения домашней страницы, предусмотреть тёмную тему 

               // разработать приложение "браузер" с возможностью добавления различных сайтов в избранное, просмотра истории, перейти на эти сайти и отчиски истории, предусмотреть возможность изменения домашней страницы, предусмотреть тёмную тему 
    }
}