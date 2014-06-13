using System;
using System.Collections.Generic;
using System.Text;

    /// <summary>
    /// Des算法源自Delphi Des算法改写 2012.08.23
    /// </summary>
public class Des
{
    #region 预定义数组

    static int[] BitIP ={ 57, 49, 41, 33, 25, 17,  9,  1,
                       59, 51, 43, 35, 27, 19, 11,  3,
                       61, 53, 45, 37, 29, 21, 13,  5,
                       63, 55, 47, 39, 31, 23, 15,  7,
                       56, 48, 40, 32, 24, 16,  8,  0,
                       58, 50, 42, 34, 26, 18, 10,  2,
                       60, 52, 44, 36, 28, 20, 12,  4,
                       62, 54, 46, 38, 30, 22, 14,  6
                     };

    static int[] BitCP = { 39,  7, 47, 15, 55, 23, 63, 31,
                        38,  6, 46, 14, 54, 22, 62, 30,
                        37,  5, 45, 13, 53, 21, 61, 29,
                        36,  4, 44, 12, 52, 20, 60, 28,
                        35,  3, 43, 11, 51, 19, 59, 27,
                        34,  2, 42, 10, 50, 18, 58, 26,
                        33,  1, 41,  9, 49, 17, 57, 25,
                        32,  0, 40,  8, 48, 16, 56, 24 
                      };

    static int[] BitExp ={ 31, 0, 1, 2, 3, 4, 3, 4, 5, 6, 7, 8, 7, 8, 9,10,
                        11,12,11,12,13,14,15,16,15,16,17,18,19,20,19,20,
                        21,22,23,24,23,24,25,26,27,28,27,28,29,30,31,0  
                      };

    static int[] BitPM ={ 15, 6,19,20,28,11,27,16, 0,14,22,25, 4,17,30, 9,
                        1, 7,23,13,31,26, 2, 8,18,12,29, 5,21,10, 3,24
                     };


    static int[] BitPMC1 = { 56, 48, 40, 32, 24, 16,  8,
                           0, 57, 49, 41, 33, 25, 17,
                           9,  1, 58, 50, 42, 34, 26,
                          18, 10,  2, 59, 51, 43, 35,
                          62, 54, 46, 38, 30, 22, 14,
                           6, 61, 53, 45, 37, 29, 21,
                          13,  5, 60, 52, 44, 36, 28,
                          20, 12,  4, 27, 19, 11,  3 
                        };
    static int[] BitPMC2 ={ 13, 16, 10, 23,  0,  4,
                          2, 27, 14,  5, 20,  9,
                         22, 18, 11,  3, 25,  7,
                         15,  6, 26, 19, 12,  1,
                         40, 51, 30, 36, 46, 54,
                         29, 39, 50, 44, 32, 47,
                         43, 48, 38, 55, 33, 52,
                         45, 41, 49, 35, 28, 31 
                       };

    static int[,] sBox = new int[8, 64]
                   {
                       { 14,  4, 13,  1,  2, 15, 11,  8,  3, 10,  6, 12,  5,  9,  0,  7,
                          0, 15,  7,  4, 14,  2, 13,  1, 10,  6, 12, 11,  9,  5,  3,  8,
                          4,  1, 14,  8, 13,  6,  2, 11, 15, 12,  9,  7,  3, 10,  5,  0,
                         15, 12,  8,  2,  4,  9,  1,  7,  5, 11,  3, 14, 10,  0,  6, 13 },
                           
                       { 15,  1,  8, 14,  6, 11,  3,  4,  9,  7,  2, 13, 12,  0,  5, 10,
                          3, 13,  4,  7, 15,  2,  8, 14, 12,  0,  1, 10,  6,  9, 11,  5,
                          0, 14,  7, 11, 10,  4, 13,  1,  5,  8, 12,  6,  9,  3,  2, 15,
                         13,  8, 10,  1,  3, 15,  4,  2, 11,  6,  7, 12,  0,  5, 14,  9 },

                       { 10,  0,  9, 14,  6,  3, 15,  5,  1, 13, 12,  7, 11,  4,  2,  8,
                         13,  7,  0,  9,  3,  4,  6, 10,  2,  8,  5, 14, 12, 11, 15,  1,
                         13,  6,  4,  9,  8, 15,  3,  0, 11,  1,  2, 12,  5, 10, 14,  7,
                          1, 10, 13,  0,  6,  9,  8,  7,  4, 15, 14,  3, 11,  5,  2, 12 },
                       
                       {  7, 13, 14,  3,  0,  6,  9, 10,  1,  2,  8,  5, 11, 12,  4, 15,
                         13,  8, 11,  5,  6, 15,  0,  3,  4,  7,  2, 12,  1, 10, 14,  9,
                         10,  6,  9,  0, 12, 11,  7, 13, 15,  1,  3, 14,  5,  2,  8,  4,
                          3, 15,  0,  6, 10,  1, 13,  8,  9,  4,  5, 11, 12,  7,  2, 14 },

                       {  2, 12,  4,  1,  7, 10, 11,  6,  8,  5,  3, 15, 13,  0, 14,  9,
                         14, 11,  2, 12,  4,  7, 13,  1,  5,  0, 15, 10,  3,  9,  8,  6,
                          4,  2,  1, 11, 10, 13,  7,  8, 15,  9, 12,  5,  6,  3,  0, 14,
                         11,  8, 12,  7,  1, 14,  2, 13,  6, 15,  0,  9, 10,  4,  5,  3 },

                       { 12,  1, 10, 15,  9,  2,  6,  8,  0, 13,  3,  4, 14,  7,  5, 11,
                         10, 15,  4,  2,  7, 12,  9,  5,  6,  1, 13, 14,  0, 11,  3,  8,
                          9, 14, 15,  5,  2,  8, 12,  3,  7,  0,  4, 10,  1, 13, 11,  6,
                          4,  3,  2, 12,  9,  5, 15, 10, 11, 14,  1,  7,  6,  0,  8, 13 },

                       {  4, 11,  2, 14, 15,  0,  8, 13,  3, 12,  9,  7,  5, 10,  6,  1,
                         13,  0, 11,  7,  4,  9,  1, 10, 14,  3,  5, 12,  2, 15,  8,  6,
                          1,  4, 11, 13, 12,  3,  7, 14, 10, 15,  6,  8,  0,  5,  9,  2,
                          6, 11, 13,  8,  1,  4, 10,  7,  9,  5,  0, 15, 14,  2,  3, 12 },

                       { 13,  2,  8,  4,  6, 15, 11,  1, 10,  9,  3, 14,  5,  0, 12,  7,
                          1, 15, 13,  8, 10,  3,  7,  4, 12,  5,  6, 11,  0, 14,  9,  2,
                          7, 11,  4,  1,  9, 12, 14,  2,  0,  6, 10, 13, 15,  3,  5,  8,
                          2,  1, 14,  7,  4, 10,  8, 13, 15, 12,  9,  0,  3,  5,  6, 11 }

                   };

    #endregion

    enum TDesMode { dmEncry, dmDecry };

    static byte[][] subKey = new byte[16][];

    static void initPermutation(ref byte[] inData)
    {
        byte[] newData = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < 64; i++)
        {
            if ((inData[BitIP[i] >> 3] & (1 << (7 - (BitIP[i] & 0x07)))) != 0)
            {
                newData[i >> 3] = (byte)(newData[i >> 3] | (1 << (7 - (i & 0x07))));
            }
        }
        for (int i = 0; i < 8; i++)
        {
            inData[i] = newData[i];
        }
    }

    static void conversePermutation(ref byte[] inData)
    {
        byte[] newData = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < 64; i++)
        {
            if ((inData[BitCP[i] >> 3] & (1 << (7 - (BitCP[i] & 0x07)))) != 0)
            {
                newData[i >> 3] = (byte)(newData[i >> 3] | (1 << (7 - (i & 0x07))));
            }
        }
        for (int i = 0; i < 8; i++)
        {
            inData[i] = newData[i];
        }
    }

    static void expand(byte[] inData, ref byte[] outData)
    {
        for (int i = 0; i < 6; i++)
        {
            outData[i] = 0;
        }
        for (int i = 0; i < 48; i++)
        {
            if ((inData[BitExp[i] >> 3] & (1 << (7 - (BitExp[i] & 0x07)))) != 0)
            {
                outData[i >> 3] = (byte)(outData[i >> 3] | (1 << (7 - (i & 0x07))));
            }
        }
    }

    static void permutation(ref byte[] inData)
    {
        byte[] newData = new byte[4] { 0, 0, 0, 0 };
        for (int i = 0; i < 32; i++)
        {
            if ((inData[BitPM[i] >> 3] & (1 << (7 - (BitPM[i] & 0x07)))) != 0)
            {
                newData[i >> 3] = (byte)(newData[i >> 3] | (1 << (7 - (i & 0x07))));
            }
        }
        for (int i = 0; i < 4; i++)
        {
            inData[i] = newData[i];
        }
    }

    static byte si(byte s, byte inByte)
    {
        byte c;
        byte t;
        c = (byte)((inByte & 0x20) | ((inByte & 0x1e) >> 1) | ((inByte & 0x01) << 4));
        t = (byte)(sBox[s, c] & 0x0f);
        return t;
    }

    static void permutationChoose1(byte[] inData, ref byte[] outData)
    {
        for (int i = 0; i < 7; i++)
        {
            outData[i] = 0;
        }
        for (int i = 0; i < 56; i++)
        {
            if ((inData[BitPMC1[i] >> 3] & (1 << (7 - (BitPMC1[i] & 0x07)))) != 0)
            {
                outData[i >> 3] = (byte)(outData[i >> 3] | (1 << (7 - (i & 0x07))));
            }
        }
    }

    static void permutationChoose2(byte[] inData, ref byte[] outData)
    {
        if (outData == null)
        {
            outData = new byte[6] { 0, 0, 0, 0, 0, 0 };
        }
        for (int i = 0; i < 6; i++)
        {
            outData[i] = 0;
        }
        for (int i = 0; i < 48; i++)
        {
            if ((inData[BitPMC2[i] >> 3] & (1 << (7 - (BitPMC2[i] & 0x07)))) != 0)
            {
                outData[i >> 3] = (byte)(outData[i >> 3] | (1 << (7 - (i & 0x07))));
            }
        }
    }

    static void cycleMove(ref byte[] inData, byte bitMove)
    {
        for (int i = 0; i < bitMove; i++)
        {
            inData[0] = (byte)((inData[0] << 1) | (inData[1] >> 7));
            inData[1] = (byte)((inData[1] << 1) | (inData[2] >> 7));
            inData[2] = (byte)((inData[2] << 1) | (inData[3] >> 7));
            inData[3] = (byte)((inData[3] << 1) | ((inData[0] & 0x10) >> 4));
            inData[0] = (byte)(inData[0] & 0x0f);
        }
    }

    static void makeKey(byte[] inKey, ref byte[][] outKey)
    {
        byte[] bitDisplace = new byte[16] { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };
        byte[] outData56 = new byte[7];
        byte[] key28l = new byte[4];
        byte[] key28r = new byte[4];
        byte[] key56o = new byte[7];

        permutationChoose1(inKey, ref outData56);

        key28l[0] = (byte)(outData56[0] >> 4);
        key28l[1] = (byte)((outData56[0] << 4) | (outData56[1] >> 4));
        key28l[2] = (byte)((outData56[1] << 4) | (outData56[2] >> 4));
        key28l[3] = (byte)((outData56[2] << 4) | (outData56[3] >> 4));
        key28r[0] = (byte)(outData56[3] & 0x0f);
        key28r[1] = outData56[4];
        key28r[2] = outData56[5];
        key28r[3] = outData56[6];

        for (int i = 0; i < 16; i++)
        {
            cycleMove(ref key28l, bitDisplace[i]);
            cycleMove(ref key28r, bitDisplace[i]);
            key56o[0] = (byte)((key28l[0] << 4) | (key28l[1] >> 4));
            key56o[1] = (byte)((key28l[1] << 4) | (key28l[2] >> 4));
            key56o[2] = (byte)((key28l[2] << 4) | (key28l[3] >> 4));
            key56o[3] = (byte)((key28l[3] << 4) | (key28r[0]));
            key56o[4] = key28r[1];
            key56o[5] = key28r[2];
            key56o[6] = key28r[3];
            permutationChoose2(key56o, ref outKey[i]);
        }
    }

    static void encry(byte[] inData, byte[] insubKey, ref byte[] outData)
    {
        byte[] outBuf = new byte[6];
        byte[] buf = new byte[8];

        expand(inData, ref outBuf);
        for (int i = 0; i < 6; i++)
        {
            outBuf[i] = (byte)(outBuf[i] ^ insubKey[i]);
        }
        // outBuf       xxxxxxxx xxxxxxxx xxxxxxxx xxxxxxxx xxxxxxxx xxxxxxxx
        buf[0] = (byte)(outBuf[0] >> 2);                                  //xxxxxx -> 2
        buf[1] = (byte)(((outBuf[0] & 0x03) << 4) | (outBuf[1] >> 4));    // 4 <- xx xxxx -> 4
        buf[2] = (byte)(((outBuf[1] & 0x0f) << 2) | (outBuf[2] >> 6));    //        2 <- xxxx xx -> 6
        buf[3] = (byte)(outBuf[2] & 0x3f);                                //                    xxxxxx
        buf[4] = (byte)(outBuf[3] >> 2);                                  //                           xxxxxx
        buf[5] = (byte)(((outBuf[3] & 0x03) << 4) | (outBuf[4] >> 4));    //                                 xx xxxx
        buf[6] = (byte)(((outBuf[4] & 0x0f) << 2) | (outBuf[5] >> 6));    //                                        xxxx xx
        buf[7] = (byte)(outBuf[5] & 0x3f);                                //                                               xxxxxx

        for (int i = 0; i < 8; i++)
        {
            buf[i] = si((byte)i, buf[i]);
        }
        for (int i = 0; i < 4; i++)
        {
            outBuf[i] = (byte)((buf[i * 2] << 4) | buf[i * 2 + 1]);
        }
        permutation(ref outBuf);
        for (int i = 0; i < 4; i++)
        {
            outData[i] = outBuf[i];
        }
    }

    /// <summary>
    /// inData, outData 都为8Bytes，否则出错
    /// </summary>
    /// <param name="desMode"></param>
    /// <param name="inData"></param>
    /// <param name="outData"></param>
    static void desData(TDesMode desMode, byte[] inData, ref byte[] outData)
    {
        byte[] temp = new byte[4];
        byte[] buf = new byte[4];
        for (int i = 0; i < 8; i++)
        {
            outData[i] = inData[i];
        }
        initPermutation(ref outData);
        if (desMode == TDesMode.dmEncry)
        {
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    temp[j] = outData[j];                 //temp = Ln
                }
                for (int j = 0; j < 4; j++)
                {
                    outData[j] = outData[j + 4];	        //Ln+1 = Rn
                }
                encry(outData, subKey[i], ref buf);        //Rn ==Kn==> buf   //subKey[i]
                for (int j = 0; j < 4; j++)
                {
                    outData[j + 4] = (byte)(temp[j] ^ buf[j]);  //Rn+1 = Ln^buf
                }
            }
            for (int j = 0; j < 4; j++)
            {
                temp[j] = outData[j + 4];
            }
            for (int j = 0; j < 4; j++)
            {
                outData[j + 4] = outData[j];
            }
            for (int j = 0; j < 4; j++)
            {
                outData[j] = temp[j];
            }
        }
        else if (desMode == TDesMode.dmDecry)
        {
            for (int i = 15; i >= 0; i--)
            {
                for (int j = 0; j < 4; j++)
                {
                    temp[j] = outData[j];
                }
                for (int j = 0; j < 4; j++)
                {
                    outData[j] = outData[j + 4];
                }
                encry(outData, subKey[i], ref buf);
                for (int j = 0; j < 4; j++)
                {
                    outData[j + 4] = (byte)(temp[j] ^ buf[j]);
                }
            }
            for (int j = 0; j < 4; j++) 
            {
                temp[j] = outData[j + 4];
            }
            for (int j = 0; j < 4; j++)
            { 
                outData[j + 4] = outData[j];
            }
            for (int j = 0; j < 4; j++) 
            { 
                outData[j] = temp[j];
            }
        }
        conversePermutation(ref outData);
    }

    public static string EncryStr(string Str, string Key)
    {
        byte[] StrByte = new byte[8];
        byte[] OutByte = new byte[8];
        byte[] KeyByte = new byte[8];
        string StrResult = "";

        if ((Str.Length > 0) && ((short)Str[Str.Length - 1] == 0))
        {
            throw new Exception("Error: the last char is NULL char.");
        }
        if (Key.Length < 8)
        {
            while (Key.Length < 8)
            {
                Key = Key + Convert.ToChar(0);
            }
        }
        while ((Str.Length % 8) != 0)
        {
            Str = Str + Convert.ToChar(0);
        }

        for (int j = 0; j < 8; j++)
        {
            KeyByte[j] = (byte)Key[j];
        }
        makeKey(KeyByte, ref subKey);

        for (int i = 0; i < (Str.Length / 8); i++)
        {
            for (int j = 0; j < 8; j++)
            {
                StrByte[j] = (byte)(Str[i * 8 + j]);
            }
            desData(TDesMode.dmEncry, StrByte, ref OutByte);
            for (int j = 0; j < 8; j++)
            {
                StrResult = StrResult + Convert.ToChar(OutByte[j]);
            }
        }
        return StrResult;
    }

    public static string DecryStr(string Str, string Key)
    {
        byte[] StrByte = new byte[8];
        byte[] OutByte = new byte[8];
        byte[] KeyByte = new byte[8];
        string StrResult = "";

        if (Key.Length < 8)
        {
            while (Key.Length < 8)
            {
                Key = Key + Convert.ToChar(0);
            }
        }

        for (int j = 0; j < 8; j++)
        {
            KeyByte[j] = (byte)Key[j];
        }
        makeKey(KeyByte, ref subKey);

        for (int i = 0; i < (Str.Length / 8); i++)
        {
            for (int j = 0; j < 8; j++)
            {
                StrByte[j] = (byte)(Str[i * 8 + j]);
            }
            desData(TDesMode.dmDecry, StrByte, ref OutByte);
            for (int j = 0; j < 8; j++)
            {
                StrResult = StrResult + Convert.ToChar(OutByte[j]);
            }
        }

        while ((StrResult.Length > 0) && ((short)StrResult[StrResult.Length - 1] == 0))
        {
            StrResult = StrResult.Substring(0, StrResult.Length - 1);
        }

        return StrResult;
    }

    public static string EncryStrHex(string Str, string Key)
    {
        string StrResult = "";
        string TempResult = "";
        string Temp = "";

        try
        {
            TempResult = EncryStr(Str, Key);
            for (int i = 0; i < TempResult.Length; i++)
            {
                Temp = String.Format("{0,0:X}", (short)TempResult[i]);
                if (Temp.Length == 1)
                {
                    Temp = '0' + Temp;
                }
                StrResult = StrResult + Temp;
            }
        }
        catch (Exception e)
        {
            StrResult = "";
        }
        return StrResult;
    }

    public static string DecryStrHex(string StrHex, string Key)
    {
        string StrResult = "";
        string Str = "";
        string Temp = "";
        int k = 0;

        try
        {
            for (int i = 0; i < StrHex.Length / 2; i++)
            {
                Temp = StrHex.Substring(i * 2, 2);
                k = Int32.Parse(Temp, System.Globalization.NumberStyles.HexNumber);
                Str = Str + Convert.ToChar(k);
            }
            StrResult = DecryStr(Str, Key);
        }
        catch (Exception e)
        {
            StrResult = "";
        }
        return StrResult;
    }


}