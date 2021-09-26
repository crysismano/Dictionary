using Dictionary.Models;
using Dictionary.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Dictionary.ViewModels
{
    public class TranslatorPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private List<string> _languagePairs;
        private List<string> _sourceLanguages;
        private List<string> _targetLanguages;
        private Translation _translation;

        // Az forrásnyelv és a célnyelv párokba szedve
        public List<string> LanguagePairs
        {
            get { return _languagePairs; }
            set
            {
                _languagePairs = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LanguagePairs)));
                }
            }
        }
        // A forrásnyelv
        public List<string> SourceLanguages
        {
            get { return _sourceLanguages; }
            set
            {
                _sourceLanguages = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SourceLanguages)));
                }
            }
        }
        // A célnyelv
        public List<string> TargetLanguages
        {
            get { return _targetLanguages; }
            set
            {
                _targetLanguages = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TargetLanguages)));
                }
            }
        }
        // A fordítás eredménye
        public Translation Translation
        {
            get { return _translation; }
            set
            {
                _translation = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Translation)));
                }
            }
        }
        // A támogatott forrás nyelvek lekérdezése
        public async void OnNavigated()
        {
            try
            {
                TranslatorService service = new TranslatorService();
                LanguagePairs = await service.GetSupportedLanguagesAsync();

                List<string> temp = new List<string>();

                // Beállítjuk, hogy egy nyelv csak egyszer szerepelhessen
                foreach (var p in LanguagePairs)
                {
                    string[] array = p.Split('-');
                    if (!temp.Contains(array[0]))
                        temp.Add(array[0]);
                }

                SourceLanguages = temp;
            }
            catch(HttpRequestException)
            {
                await new MessageDialog("The supported languages API is not available right now. Try again later").ShowAsync();
            }
        }
        // A célnyelvek lehetőségeinek beállítása
        public void SetUpTargetLanguages(string sourceLanguage)
        {
            List<string> temp = new List<String>();
            
            foreach(var p in LanguagePairs)
            {
                // A betű párok szétválasztása és annak ellenőrzése
                string[] array = p.Split('-');
                if (array[0] == sourceLanguage && array[1] != sourceLanguage)
                    temp.Add(array[1]);
            }
            TargetLanguages = temp;
        }
        // A fordításra használt függvény
        public async void translate(string sourceLanguage, string targetLanguage, string text)
        {
            try
            {
                if (text != "")
                {
                    TranslatorService service = new TranslatorService();
                    Translation = await service.GetTranslationAsync(sourceLanguage, targetLanguage, text);
                }
                else
                {
                    Translation = null;
                }
            }
            catch (HttpRequestException e)
            {
                switch (e.Message)
                {
                    case "BadRequest":
                        await new MessageDialog("Please select a target langague first").ShowAsync();
                        break;
                    default:
                        await new MessageDialog("Something went wrong! Please try again later").ShowAsync();
                        break;
                }
            }
        }
    }
}
