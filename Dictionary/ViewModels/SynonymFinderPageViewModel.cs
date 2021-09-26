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
    // A szinonima kereső ViewModel-je
    public class SynonymFinderPageViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private Synonym _synonym;
        private string _word;
        private string _selectedLanguage;
        private List<string> _languageList;

        // A szinonima lekérdezés eredménye
        public Synonym Synonym
        {
            get { return _synonym; }
            set
            {
                _synonym = value;
                if(PropertyChanged != null)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Synonym)));
                }
            }
        }
        // A szó aminek a szinonimáját keressük
        public string Word
        {
            get { return _word; }
            set
            {
                _word = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Word)));
                }
            }
        }
        // A kiválasztott nyelv
        public string SelectedLanguage
        {
            get { return _selectedLanguage; }
            set
            {
                _selectedLanguage = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedLanguage)));
                }
            }
        }
        // Az elérhető nyelvek listája
        public List<string> LanguageList
        {
            get{ return _languageList;  }
            set
            {
                _languageList = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LanguageList)));
                }
            }
        }
        // inicializálja a támogatott nyelvek listáját
        private void InitSupportedLanguagesList()
        {
            List<string> langauges = new List<string>();
            langauges.Add("czech");
            langauges.Add("english");
            langauges.Add("french");
            langauges.Add("german");
            langauges.Add("italian");
            langauges.Add("hungarian");
            langauges.Add("polish");
            langauges.Add("russian");


            LanguageList = langauges;
        }

        public SynonymFinderPageViewModel()
        {
            InitSupportedLanguagesList();
        }
        // A fordításra használt függvény
        public async void FindSynonym()
        {
            string code;
            // megnézi mi a kiválasztott nyelv, majd ez alapján állítja be a code szövegét
            switch (_selectedLanguage)
            {
                case "czech": code = "cs_CZ"; break;
                case "english": code = "en_US"; break;
                case "french": code = "fr_FR"; break;
                case "german": code = "de_DE"; break;
                case "italian": code = "it_IT"; break;
                case "hungarian": code = "hu_HU";break;
                case "polish": code = "pl_PL"; break;
                case "russian": code = "ru_RU"; break;
                default: code = "";break;
            }
            var service = new SynonymService();
            try
            {
                Synonym = await service.GetSynonymAsync(code, _word);
                // A válaszban kapott szavakat vesszővel választjuk el
                for (int i = 0; i < _synonym.response.Length; i++)
                {
                    string sym = _synonym.response[i].list.synonyms;
                    sym = sym.Replace("|", ", ");
                    Synonym.response[i].list.synonyms = sym;
                }
            }
            catch(HttpRequestException e)
            {
                switch (e.Message) {
                    case "NotFound":
                        await new MessageDialog("No synonyms found").ShowAsync();
                        break;
                    case "BadRequest":
                        await new MessageDialog("Please select a language first").ShowAsync();
                        break;
                    default:
                        await new MessageDialog("Something went wrong! Please try again later").ShowAsync(); 
                        break;
                }
            }
        }
    }
}
