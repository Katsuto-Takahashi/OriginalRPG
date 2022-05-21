using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class DataManager : MonoBehaviour
{
    string m_data;

    void Awake()
    {
        m_data = Application.persistentDataPath + "/JsonTest.json";
    }

    void Start()
    {
        JsonData data = new JsonData();

        //SaveJsonTest(data);

        //Debug.Log(JsonUtility.ToJson(LoadJsonTest(m_data)));

        ReadTest(m_data);
        SaveTest(data);
        Debug.Log(JsonUtility.ToJson(LoadJsonTest(m_data)));
    }

    /// <summary>データの保存</summary>
    /// <param name="data"></param>
    /// <param name="num"></param>
    void SaveDataTest(JsonData data, int num)
    {
        var chara = PartyManager.Instance.CharacterParty;
        data.datas = new ParamData[num];
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
    }

    /// <summary>データの読み取り</summary>
    /// <param name="data"></param>
    /// <param name="num"></param>
    void ReadDataTest(JsonData data, int num)
    {
        var chara = PartyManager.Instance.CharacterParty;
        data.datas = new ParamData[num];
        for (int i = 0; i < num; i++)
        {
            data.datas[i] = new ParamData();

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
    }

    void SaveJsonTest(JsonData jsondata)
    {
        string jsonstr = JsonUtility.ToJson(jsondata);//受け取ったJsonDataをJSONに変換
        StreamWriter writer = new StreamWriter(m_data, false);//指定したデータの保存先を開く(false 上書き,true 追記)
        writer.WriteLine(jsonstr);//JSONデータを書き込み
        writer.Flush();//バッファをクリアする
        writer.Close();//ファイルをクローズする
    }

    JsonData LoadJsonTest(string path)
    {
        StreamReader reader = new StreamReader(path); //受け取ったパスのファイルを読み込む
        string json = reader.ReadToEnd();//ファイルの中身をすべて読み込む
        reader.Close();//ファイルを閉じる

        return JsonUtility.FromJson<JsonData>(json);//読み込んだJSONファイルをJsonData型に変換して返す
    }

    void ReadTest(string path)
    {
        if (File.Exists(path))
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            try
            {
                byte[] read = File.ReadAllBytes(path);
                byte[] decryption = AesDecryption(read);
                string decryptString = Encoding.UTF8.GetString(decryption);
                JsonData jsonData = JsonUtility.FromJson<JsonData>(decryptString);
                ReadDataTest(jsonData, 1);
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

    void SaveTest(JsonData jsondata)
    {
        SaveDataTest(jsondata, 1);
        string jsonString = JsonUtility.ToJson(jsondata);
        byte[] bytes = Encoding.UTF8.GetBytes(jsonString);
        byte[] arrEncrypted = AesEncryption(bytes);
        FileStream file = new FileStream(m_data, FileMode.Create, FileAccess.Write);
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
