using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Dictionary.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TranslatorPage : Page
    {
        public TranslatorPage()
        {
            this.InitializeComponent();
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.OnNavigated();
        }

        private void ScourceLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SetUpTargetLanguages(sourceLanguage_cBox.SelectedItem.ToString());
            targetLanguage_cBox.IsEnabled = true;
            tBox.IsEnabled = false;
        }

        private void TargetLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tBox.IsEnabled = true;
            if(targetLanguage_cBox.SelectedItem != null)
                ViewModel.translate(sourceLanguage_cBox.SelectedItem.ToString(), targetLanguage_cBox.SelectedItem.ToString(), tBox.Text);
        }

        private void Tbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.translate(sourceLanguage_cBox.SelectedItem.ToString(), targetLanguage_cBox.SelectedItem.ToString(), tBox.Text);
        }
    }
}
