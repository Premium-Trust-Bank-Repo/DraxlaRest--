using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.IO;


namespace DraxlaRest.Models
{
    public class Encryption
    {
        //private string public_key = "<RSAKeyValue><Modulus>yFP9D0KDMgszvbpt443hQtJPZNceMmYL/MJ47a9CTC/DUFF4rE27v2u9ZgRI+N8ImSKH3H+E9gEmV4Q5YB0bbJkCPxCRb+TqsmSgv0b7Es/vQcQXs4izLVxMTxUYOd1i51mDuewowYcUZ4p4M+t4fj+zol/FEj2GAnlMk9Z1ansvDx98ANFJP0Ipo/o2OREwk8wMfVGgNIzcLWQoNJRIZdJdehQBfkWgER2pdmSGYUc1bYijPB05OgrAJAvSAjIeMoQlR9aeWkNFUfSS5lw9R8Pn8NSVPjtYZUpVeE+4Nr5jnxw/WYjYtc+RgM3w7aTo/iCQvENLB826nbcSXYbxpQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private string public_key = "<RSAKeyValue><Modulus>y+cvggc8Hm5Mi52CgE+IhoO7YQga9cG9kiIi05G0/tpbETt7VdIQ8kRv651Bv7j6F3imYtIjVexElBYl8rC8/tjgeBzRWmFBrDOZhg4snPeKnY6SIsJuARGV+w6+6EuHHXplQ4VXqXvujAcsa6wtGDku9Dkhgr8d+aqw3Row0sc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        private static readonly int keySize = 1024;

       // private string public_key = "<RSAKeyValue><Modulus>y+cvggc8Hm5Mi52CgE+IhoO7YQga9cG9kiIi05G0/tpbETt7VdIQ8kRv651Bv7j6F3imYtIjVexElBYl8rC8/tjgeBzRWmFBrDOZhg4snPeKnY6SIsJuARGV+w6+6EuHHXplQ4VXqXvujAcsa6wtGDku9Dkhgr8d+aqw3Row0sc=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
        //private static readonly int keySize = 1024;




        public string EncryptPayload(string content)
        {
            string encryptionKey = "zVFlC0MGJjDI0NjgtWQxZmIzNzJM3NDt";
            byte[] bytes = Encoding.UTF8.GetBytes(encryptionKey);
            byte[] iV = new byte[bytes.Length / 2];
            byte[] inArray;
            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.KeySize = 256;
                aesManaged.Key = bytes;
                aesManaged.IV = iV;
                //aesManaged.GenerateIV();
                ICryptoTransform transform = aesManaged.CreateEncryptor(aesManaged.Key, aesManaged.IV);
                MemoryStream memoryStream = new MemoryStream();
                CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(content);
                }

                inArray = memoryStream.ToArray();
            }

            // string kk = inArray.ToString();

            return Convert.ToBase64String(inArray);
        }


        public string Encrypt(string textToBeEncrypted)
        {
            var testString = Encoding.UTF8.GetBytes(textToBeEncrypted);

            using (var rsa = new RSACryptoServiceProvider(keySize))
            {
                try
                {
                    rsa.FromXmlString(public_key);

                    var encryptedData = rsa.Encrypt(testString, false);

                    var base64EncryptedString = Convert.ToBase64String(encryptedData);

                    return base64EncryptedString;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }


        //public string Encrypt(string textToBeEncrypted)
        //{
        //    // create an instance of the library
        //    PGPLib pgp = new PGPLib();
        //    String encryptedString =
        //    pgp.EncryptString(textToBeEncrypted, new FileInfo(@"C:\TokenErr\0x110B6865-pub.asc"));

        //    return encryptedString;

        //}

        //public string Decrypt(string textToBeDecrypted)
        //{
        //    // create an instance of the library
        //    PGPLib pgp = new PGPLib();
        //    string seckey = GetKeyFromFile();
        //    String decryptedString =
        //    pgp.DecryptString(textToBeDecrypted, seckey, null);
        //    return decryptedString;

        //}

        //protected string GetKeyFromFile()
        //{
        //    string resp = "";
        //    using (StreamReader streamReader = File.OpenText(@"C:\TokenErr\0x110B6865-sec.asc"))
        //    {
        //        string mresp = streamReader.ReadLine();

        //        while(mresp != null)
        //        {
        //            resp = resp + Environment.NewLine +  mresp;
        //            mresp = streamReader.ReadLine();
        //        }
        //    }

        //    return resp;
        //}

    }
}