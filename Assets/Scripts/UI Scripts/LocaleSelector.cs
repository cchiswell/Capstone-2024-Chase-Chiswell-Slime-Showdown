using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{
    private bool active = false;

    public void ToggleLocale()
    {
        if (active) return;
        
        int newLocaleID = LocalizationSettings.SelectedLocale.Identifier.Code == "en" ? 1 : 0;
        StartCoroutine(SetLocale(newLocaleID));
    }

    IEnumerator SetLocale(int localeID)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeID];
        active = false;
    }
}