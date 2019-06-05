using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DecryptEncryptionApplication
{

    /// <summary>
    /// 加密解密类
    /// </summary>
   public class DecryptEncryption
    {
       private const string program_key = "5w,-evP}g}x|!9O{Dsi<'Rag+eef;2:l";
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
        public static string StringEncryption(string EncryptionContent)
        {

            string ddd=Security.AES.Encrypt(EncryptionContent, program_key);
            return ddd;
        
        }
        #endregion
    }
}
