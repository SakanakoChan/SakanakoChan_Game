using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour, ISaveManager
{
    public static LanguageManager instance;

    //0 for english, 1 for chinese
    public int localeID { get; set; }

    public Dictionary<string, string> EnglishToChineseKeybindsDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        EnglishToChineseKeybindsDictionary = new Dictionary<string, string>();
        SetupEnglishToChineseKeybindsDictionary();
    }

    public void SetTextLanguageByLocaleID(int _localeID)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
    }

    private void SetupEnglishToChineseKeybindsDictionary()
    {
        EnglishToChineseKeybindsDictionary.Add("Attack", "����");
        EnglishToChineseKeybindsDictionary.Add("Aim", "��׼");
        EnglishToChineseKeybindsDictionary.Add("Flask", "Ԫ��ƿ");
        EnglishToChineseKeybindsDictionary.Add("Dash", "���");
        EnglishToChineseKeybindsDictionary.Add("Parry", "����");
        EnglishToChineseKeybindsDictionary.Add("Crystal", "ˮ��");
        EnglishToChineseKeybindsDictionary.Add("Blackhole", "�ڶ�");
        EnglishToChineseKeybindsDictionary.Add("Character", "��ɫ���");
        EnglishToChineseKeybindsDictionary.Add("Craft", "�������");
        EnglishToChineseKeybindsDictionary.Add("Skill", "�������");
    }

    public string GetEnglishNameByChinese(string _chineseName)
    {
        string result = string.Empty;
        
        foreach (var search in EnglishToChineseKeybindsDictionary)
        {
            if (search.Value == _chineseName)
            {
                result = search.Key;
            }
        }

        return result;
    }

    void ISaveManager.LoadData(GameData _data)
    {
        localeID = _data.localeID;

        SetTextLanguageByLocaleID(localeID);
    }

    void ISaveManager.SaveData(ref GameData _data)
    {
        _data.localeID = localeID;
    }
}
