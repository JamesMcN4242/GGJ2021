////////////////////////////////////////////////////////////
/////   LocalisationService.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

using static UnityEngine.Debug;

namespace PersonalFramework
{
    [Serializable]
    public struct LocalisationItem
    {
        public string m_key;
        public string m_entry;
    }

    [Serializable]
    public struct LocalisationList
    {
        public LocalisationItem[] m_localisationItems;
    }

    public class LocalisationService
    {
        public enum LocalisableCultures
        {
            UNLOADED = -1,
            ENGLISH = 0,
            DEUTSCH,
            COUNT
        }

        private static readonly string[] k_countryCodes = new string[]
        {
            "en-GB", //English (United Kingdom)
            "de-AT" //German (Austrian)
        };


        private LocalisableCultures m_currentLocalised = LocalisableCultures.UNLOADED;
        private Dictionary<string, string> m_localisationDictionary = null;

        public void LoadLocalisation(LocalisableCultures cultureToLoad, string resourceFormat)
        {
            if (cultureToLoad == m_currentLocalised) return;

            m_currentLocalised = cultureToLoad;
            string cultureName = cultureToLoad.ToString().ToLower();
            string json = Resources.Load<TextAsset>(string.Format(resourceFormat, cultureName)).text;
            LocalisationList localisationList = JsonUtility.FromJson<LocalisationList>(json);

            m_localisationDictionary = new Dictionary<string, string>(localisationList.m_localisationItems.Length);
            for(int i = 0; i < localisationList.m_localisationItems.Length; i++)
            {
#if DEBUG
                //Force an error log if in debug to show this key is being entered twice
                if(m_localisationDictionary.ContainsKey(localisationList.m_localisationItems[i].m_key))
                {
                    LogError($"Trying to enter same localisation key {localisationList.m_localisationItems[i].m_key} more than once. " +
                        $"Previous value: \"{m_localisationDictionary[localisationList.m_localisationItems[i].m_key]}\". " +
                        $"New value: \"{localisationList.m_localisationItems[i].m_entry}\"");
                }
#endif
                m_localisationDictionary[localisationList.m_localisationItems[i].m_key] = localisationList.m_localisationItems[i].m_entry;
            }

            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(k_countryCodes[(int)cultureToLoad]);
        }

        public string GetLocalised(string key)
        {
            Assert(m_localisationDictionary != null, $"Trying to get localised string with key {key} before dictionary is created");
            Assert(m_localisationDictionary.ContainsKey(key), $"Trying to get localised string with key {key} that doesn't exist");
            return m_localisationDictionary[key];
        }

        public string GetLocalisedFormat(string key, params object[] toInclude)
        {
            Assert(m_localisationDictionary != null, $"Trying to get localised string with key {key} before dictionary is created");
            return string.Format(GetLocalised(key), toInclude);
        }
    }
}