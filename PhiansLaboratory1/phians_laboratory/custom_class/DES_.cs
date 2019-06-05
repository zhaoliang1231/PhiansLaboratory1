using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Security;

namespace phians_laboratory.custom_class
{
    public class DES_
    {

        
     
         private const string program_key = "vw,-evP}g}x|!9O{Dsi<'Rag+eef;2:Q";
        #region 解密字符串
        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="DecryptContent">待解密内容</param>
        /// <returns> 返回解密字符串</returns>
         public static string StringDecrypt(string DecryptContent)
        {

            return Security.AES.Decrypt (DecryptContent, program_key);
        }
        #endregion

        #region 加密字符串
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="EncryptionContent">待加密内容</param>
        /// <returns>返回加密字符串</returns>
         public static string stringEncrypt(string EncryptionContent)
        {

            return Security.AES.Encrypt(EncryptionContent, program_key);
        
        }
        #endregion
    

    }
}