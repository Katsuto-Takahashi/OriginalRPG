using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    /// <summary>セーブファイルのパス</summary>
    string m_dataPath;

    protected override void Awake()
    {
        m_dataPath = Application.persistentDataPath + "/JsonTest.json";
    }

    /// <summary>データを読み込みます</summary>
    public void DataRead()
    {
        ReadFile(m_dataPath);
    }

    /// <summary>データを保存します</summary>
    public void DataSave()
    {
        SaveFile();
    }

    /// <summary>キャラクターデータの保存</summary>
    /// <param name="num">キャラクター数</param>
    CharactersData SaveCharactersData(int num)
    {
        var chara = PartyManager.Instance.CharacterParty;
        CharactersData data = new CharactersData
        {
            datas = new ParamData[num]
        };
        for (int i = 0; i < num; i++)
        {
            data.datas[i] = new ParamData
            {
                name = chara[i].Name.Value,
                hp = chara[i].HP.Value,
                maxhp = chara[i].MaxHP.Value,
                ap = chara[i].AP.Value,
                maxap = chara[i].MaxAP.Value,
                strength = chara[i].Strength.Value,
                defence = chara[i].Defense.Value,
                magicpower = chara[i].MagicPower.Value,
                magicresist = chara[i].MagicResist.Value,
                luck = chara[i].Luck.Value,
                speed = chara[i].Speed.Value,
                level = chara[i].Level.Value,
                skillpoint = chara[i].SkillPoint.Value,
                nowexp = chara[i].NowExp.Value,
                totalexp = chara[i].TotalExp.Value,
                nextexp = chara[i].NextExp.Value,
                skillindex = new int[] { 0, 1, 2, 5, 7, 8, 11 }
            };
        }
        Debug.Log("保存完了");
        return data;
    }

    /// <summary>キャラクターデータの読み取り</summary>
    /// <param name="data">キャラクターデータ</param>
    /// <param name="num">キャラクター数</param>
    void ReadCharactersData(CharactersData data, int num)
    {
        var chara = PartyManager.Instance.CharacterParty;
        for (int i = 0; i < num; i++)
        {
            chara[i].Name.Value = data.datas[i].name;
            chara[i].HP.Value = data.datas[i].hp;
            chara[i].MaxHP.Value = data.datas[i].maxhp;
            chara[i].AP.Value = data.datas[i].ap;
            chara[i].MaxAP.Value = data.datas[i].maxap;
            chara[i].Strength.Value = data.datas[i].strength;
            chara[i].Defense.Value = data.datas[i].defence;
            chara[i].MagicPower.Value = data.datas[i].magicpower;
            chara[i].MagicResist.Value = data.datas[i].magicresist;
            chara[i].Luck.Value = data.datas[i].luck;
            chara[i].Speed.Value = data.datas[i].speed;
            chara[i].Level.Value = data.datas[i].level;
            chara[i].SkillPoint.Value = data.datas[i].skillpoint;
            chara[i].NowExp.Value = data.datas[i].nowexp;
            chara[i].TotalExp.Value = data.datas[i].totalexp;
            chara[i].NextExp.Value = data.datas[i].nextexp;
            //data.datas[i].skillindex = new int[] { 0, 1, 2, 5, 7, 8, 11 };
        }
        Debug.Log("読み込み完了");
    }

    /// <summary>ファイルのデータを読み込む</summary>
    /// <param name="path">ファイルのパス</param>
    void ReadFile(string path)
    {
        if (File.Exists(path))
        {
            Debug.Log("保存されているデータを読み込むよ");
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

            try
            {
                byte[] read = File.ReadAllBytes(path);
                byte[] decryption = AesDecryption(read);
                string decryptString = Encoding.UTF8.GetString(decryption);
                CharactersData jsonData = JsonUtility.FromJson<CharactersData>(decryptString);
                ReadCharactersData(jsonData, 2);
            }
            finally
            {
                fileStream.Close();
            }
        }
        else
        {
            Debug.Log("新しく作るよ");
        }
    }

    /// <summary>ファイルにデータを書き込む</summary>
    void SaveFile()
    {
        Debug.Log("データを保存するよ");
        CharactersData jsondata = SaveCharactersData(2);
        string jsonString = JsonUtility.ToJson(jsondata);
        byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
        byte[] arrEncrypted = AesEncryption(bytes);
        FileStream file = new FileStream(m_dataPath, FileMode.Create, FileAccess.Write);

        try
        {
            file.Write(arrEncrypted, 0, arrEncrypted.Length);
        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    /// <summary>AesManagedを取得</summary>
    /// <param name="keySize">共有キーのサイズ</param>
    /// <param name="blockSize">ブロックサイズ</param>
    /// <param name="iv">初期化ベクトル</param>
    /// <param name="key">共有キー</param>
    /// <returns>取得結果</returns>
    AesManaged GetAesManaged(int keySize = 128, int blockSize = 128, string iv = "1234567890123456", string key = "1234567890123456")
    {
        AesManaged aes = new AesManaged
        {
            KeySize = keySize,
            BlockSize = blockSize,
            Mode = CipherMode.CBC,
            IV = Encoding.UTF8.GetBytes(iv),
            Key = Encoding.UTF8.GetBytes(key),
            Padding = PaddingMode.PKCS7
        };
        return aes;
    }

    /// <summary>Aes暗号化</summary>
    /// <param name="bytes"></param>
    /// <returns>暗号化したもの</returns>
    byte[] AesEncryption(byte[] bytes)
    {
        var aes = GetAesManaged();
        var encrypt = aes.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
        return encrypt;
    }

    /// <summary>Aes複号化</summary>
    /// <param name="bytes"></param>
    /// <returns>複号化したもの</returns>
    byte[] AesDecryption(byte[] bytes)
    {
        var aes = GetAesManaged();
        var decrypt = aes.CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length);
        return decrypt;
    }
}
