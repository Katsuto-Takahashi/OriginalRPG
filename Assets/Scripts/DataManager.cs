using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    /// <summary>�Z�[�u�t�@�C���̃p�X</summary>
    string m_dataPath;

    protected override void Awake()
    {
        m_dataPath = Application.persistentDataPath + "/JsonTest.json";
    }

    /// <summary>�f�[�^��ǂݍ��݂܂�</summary>
    public void DataRead()
    {
        ReadFile(m_dataPath);
    }

    /// <summary>�f�[�^��ۑ����܂�</summary>
    public void DataSave()
    {
        SaveFile();
    }

    /// <summary>�L�����N�^�[�f�[�^�̕ۑ�</summary>
    /// <param name="num">�L�����N�^�[��</param>
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
        Debug.Log("�ۑ�����");
        return data;
    }

    /// <summary>�L�����N�^�[�f�[�^�̓ǂݎ��</summary>
    /// <param name="data">�L�����N�^�[�f�[�^</param>
    /// <param name="num">�L�����N�^�[��</param>
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
        Debug.Log("�ǂݍ��݊���");
    }

    /// <summary>�t�@�C���̃f�[�^��ǂݍ���</summary>
    /// <param name="path">�t�@�C���̃p�X</param>
    void ReadFile(string path)
    {
        if (File.Exists(path))
        {
            Debug.Log("�ۑ�����Ă���f�[�^��ǂݍ��ނ�");
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
            Debug.Log("�V��������");
        }
    }

    /// <summary>�t�@�C���Ƀf�[�^����������</summary>
    void SaveFile()
    {
        Debug.Log("�f�[�^��ۑ������");
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

    /// <summary>AesManaged���擾</summary>
    /// <param name="keySize">���L�L�[�̃T�C�Y</param>
    /// <param name="blockSize">�u���b�N�T�C�Y</param>
    /// <param name="iv">�������x�N�g��</param>
    /// <param name="key">���L�L�[</param>
    /// <returns>�擾����</returns>
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

    /// <summary>Aes�Í���</summary>
    /// <param name="bytes"></param>
    /// <returns>�Í�����������</returns>
    byte[] AesEncryption(byte[] bytes)
    {
        var aes = GetAesManaged();
        var encrypt = aes.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
        return encrypt;
    }

    /// <summary>Aes������</summary>
    /// <param name="bytes"></param>
    /// <returns>��������������</returns>
    byte[] AesDecryption(byte[] bytes)
    {
        var aes = GetAesManaged();
        var decrypt = aes.CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length);
        return decrypt;
    }
}
