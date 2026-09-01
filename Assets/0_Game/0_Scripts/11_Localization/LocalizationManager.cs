using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocalizationManager : MonoBehaviour
{
   public void ToogleLanguage() {
        int locale = PlayerPrefs.GetInt("Locale");
        if (locale == 0) {
            int localeId = 1;
            PlayerPrefs.SetInt("Locale", localeId);
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
        }
        if (locale == 1) {
            int localeId = 0;
            PlayerPrefs.SetInt("Locale", localeId);
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
        }
    }
}
