using System;
using System.Security.Cryptography;
using System.Text;

public class CryptoUtil
{
    //JustINBeaver 전용 AES Key (32 bytes)
    private static readonly byte[] Key =
        Encoding.UTF8.GetBytes("JUSTINBEAVER_AES_KEY_32BYTE!");

    //JustINBeaver 전용 AES IV (16 bytes)
    private static readonly byte[] IV =
        Encoding.UTF8.GetBytes("JIB_AES_IV_16B");

    /// <summary>
    /// 문자열 -> AES 암호화 -> Base64 문자열 변환
    /// </summary>
    /// <param name="plainText"></param>
    /// <returns></returns>
    public static string Encrypt(string plainText)
    {
        using var aes = Aes.Create();

        //암호화 키, IV 설정
        aes.Key = Key;
        aes.IV = IV;

        using var encryptor = aes.CreateEncryptor(); // 암호화 변환기 생성
        byte[] inputBytes = Encoding.UTF8.GetBytes(plainText); // 바이트로 변환
        byte[] encrypted = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length); // 실제 암호화 수행

        //파일 저장을 위해 Base64 문자열로 변환       
        return Convert.ToBase64String(encrypted);
    }

    /// <summary>
    /// 리버스 변환
    /// </summary>
    /// <param name="cipherText"></param>
    /// <returns></returns>
    public static string Decrypt(string cipherText)
    {
        using var aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;

        using var decryptor = aes.CreateDecryptor();
        byte[] inputBytes = Convert.FromBase64String(cipherText);
        byte[] decrypted = decryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

        return Encoding.UTF8.GetString(decrypted);
    }
}
