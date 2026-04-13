using System;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// JustINBeaver 전용 AES 암호화 유틸
/// - AES-256 (Key 32 bytes)
/// - IV 16 bytes
/// </summary>
public static class CryptoUtil
{
    //byte[]로 직접 길이 보장
    private static readonly byte[] Key = new byte[32]
    {
        0x4A,0x55,0x53,0x54,0x49,0x4E,0x42,0x45,
        0x41,0x56,0x45,0x52,0x5F,0x4B,0x45,0x59,
        0x5F,0x33,0x32,0x5F,0x42,0x59,0x54,0x45,
        0x5F,0x46,0x49,0x58,0x45,0x44,0x21,0x21
    };

    private static readonly byte[] IV = new byte[16]
    {
        0x4A,0x49,0x42,0x5F,0x41,0x45,0x53,0x5F,
        0x49,0x56,0x5F,0x31,0x36,0x42,0x59,0x54
    };

    /// <summary>
    /// 문자열 -> AES 암호화 -> Base64
    /// </summary>
    public static string Encrypt(string plainText)
    {
        using Aes aes = Aes.Create();

        //암호화 Key, IV 설정
        aes.Key = Key;
        aes.IV = IV;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using ICryptoTransform encryptor = aes.CreateEncryptor(); // 암호화 변환기 생성
        byte[] input = Encoding.UTF8.GetBytes(plainText); // 바이트로 변환
        byte[] encrypted = encryptor.TransformFinalBlock(input, 0, input.Length); // 실제 암호화 수행

        return Convert.ToBase64String(encrypted); //파일 저장을 위해 Base64 문자열로 변환
    }

    /// <summary>
    /// 리버스
    /// </summary>
    public static string Decrypt(string cipherText)
    {
        using Aes aes = Aes.Create();
        aes.Key = Key;
        aes.IV = IV;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using ICryptoTransform decryptor = aes.CreateDecryptor();
        byte[] buffer = Convert.FromBase64String(cipherText);
        byte[] decrypted = decryptor.TransformFinalBlock(buffer, 0, buffer.Length);

        return Encoding.UTF8.GetString(decrypted);
    }
}