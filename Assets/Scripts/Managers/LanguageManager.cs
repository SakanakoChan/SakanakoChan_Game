using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour, ISettingsSaveManager
{
    public static LanguageManager instance;

    //0 for english, 1 for chinese
    public int localeID { get; set; }

    public Dictionary<string, string> EnglishToChineseKeybindsDictionary;
    public Dictionary<string, string> EnglishToChineseEquipmentTypeDictionary;
    public Dictionary<string, string> EnglishToChineseStatDictionary;

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
        EnglishToChineseEquipmentTypeDictionary = new Dictionary<string, string>();
        EnglishToChineseStatDictionary = new Dictionary<string, string>();

        SetupEnglishToChineseKeybindsDictionary();
        SetupEnglishToChineseEquipmentTypeDictionary();
        SetupEnglishToChineseStatDictionary();
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

    private void SetupEnglishToChineseEquipmentTypeDictionary()
    {
        EnglishToChineseEquipmentTypeDictionary.Add("Weapon", "����");
        EnglishToChineseEquipmentTypeDictionary.Add("Armor", "����");
        EnglishToChineseEquipmentTypeDictionary.Add("Charm", "�����");
        EnglishToChineseEquipmentTypeDictionary.Add("Flask", "Ԫ��ƿ");
    }

    private void SetupEnglishToChineseStatDictionary()
    {
        EnglishToChineseStatDictionary.Add("Strength", "����");
        EnglishToChineseStatDictionary.Add("Agility", "����");
        EnglishToChineseStatDictionary.Add("Intelligence", "����");
        EnglishToChineseStatDictionary.Add("Vitality", "����");
        EnglishToChineseStatDictionary.Add("Damage", "�˺�");
        EnglishToChineseStatDictionary.Add("Crit Chance", "������");
        EnglishToChineseStatDictionary.Add("Crit Power", "�����˺�");
        EnglishToChineseStatDictionary.Add("Max HP", "�������ֵ");
        EnglishToChineseStatDictionary.Add("Evasion", "����");
        EnglishToChineseStatDictionary.Add("Armor", "����");
        EnglishToChineseStatDictionary.Add("Magic Resist", "ħ������");
        EnglishToChineseStatDictionary.Add("Fire Dmg", "�����˺�");
        EnglishToChineseStatDictionary.Add("Ice Dmg", "�����˺�");
        EnglishToChineseStatDictionary.Add("Lightning Dmg", "�׵��˺�");
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

    public string TranslateItemStatInfoFromEnglishToChinese(string _itemStatInfo)
    {
        foreach (var search in EnglishToChineseStatDictionary)
        {
            string stat_English = search.Key;

            _itemStatInfo = _itemStatInfo.Replace(stat_English, EnglishToChineseStatDictionary[stat_English]);
        }

        return _itemStatInfo;
    }

    public void LoadData(SettingsData _data)
    {
        localeID = _data.localeID;

        SetTextLanguageByLocaleID(localeID);
    }

    public void SaveData(ref SettingsData _data)
    {
        _data.localeID = localeID;
    }
}
