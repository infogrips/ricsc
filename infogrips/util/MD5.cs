using System;
using System.IO;
using System.Security.Cryptography;

namespace infogrips.util
{
   public class MD5
   {
      public static String getFileDigest(String file)
      {
         using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
         {
            using (FileStream stream = File.OpenRead(file))
            {
               var hash = md5.ComputeHash(stream);
               return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
         }
      }
   }
}