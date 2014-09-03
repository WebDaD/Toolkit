using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebDaD.Toolkit.Database;
using WebDaD.Toolkit.Helper;

namespace WebDaD.Toolkit.Licensing
{
    public class License : CRUDable
    {
        private WebDaD.Toolkit.Database.Database db;
        public WebDaD.Toolkit.Database.Database DB { get { return db; } }

        private string tablename;
        public string Tablename { get { return tablename; } }

        private string id;
        public string ID { get { return id; } set { this.id = value; } }

        private string dataname;

        private string username;
        public string UserName { get { return username; } set { username = value; } }

        private string hash;
        public string Hash { get { return hash; } set { hash = value; } }

        private string application;

        private DateTime expirationDate;


        private string publickey_file;
        private string privatekey_file;
        private RSAEncryption enc;

        public RSAEncryption Enc
        {
            get
            {
                return this.enc;
            }
        }

        public static void CreateKeys(string baseout)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string privateKey = rsa.ToXmlString(true);
            File.WriteAllText(baseout + "\\private_key.xml", privateKey);
            string publicKey = rsa.ToXmlString(false);
            File.WriteAllText(baseout + "\\public_key.xml", publicKey);
        }

        /// <summary>
        /// This is the Method used by Applications
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="publickey_file"></param>
        /// <returns></returns>
        public static bool IsLicensed(string app,string user, string encrypted_hash)
        {
            //use public key to decrypt encrypted_hash and compare user and app
            //app|user
            throw new NotImplementedException();
        }

        public Dictionary<string, string> FieldSet()
        {
            Dictionary<string,string> r = new Dictionary<string,string>();
            r.Add("username", this.username);
            r.Add("application", this.application);
            r.Add("expirationDate", this.expirationDate.ToString("yyyy-MM-dd"));
            r.Add("hash", Encrypt(this.hash));
            r.Add("active","1");
            return r;
        }

        public DataTable GetLicenses()
        {
            //TODO: get List for gridview
            throw new NotImplementedException();
        }

        public License(WebDaD.Toolkit.Database.Database db, string tablename, string id, string public_key, string private_key)
        {
            this.db = db;
            this.tablename = tablename;
            this.id = id;
            this.publickey_file = public_key;
            this.privatekey_file = private_key;
            enc = new RSAEncryption();
            enc.LoadPrivateFromXml(private_key);
            enc.LoadPublicFromXml(public_key);
        }

        public string Encrypt(string target)
        {
            byte[] message = Encoding.UTF8.GetBytes(target);
            byte[] encMessage = null;
            encMessage = this.enc.PrivateEncryption(message);
            return Convert.ToBase64String(encMessage);
        }
        public string Decrypt(string source)
        {
            byte[] decMessage = Convert.FromBase64String(source);
            byte[] message = null;
            message = this.enc.PublicDecryption(decMessage);
            return Encoding.UTF8.GetString(message);
        }
        public bool Save()
        {
            throw new NotImplementedException();
        }

        public bool Delete()
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetIDList()
        {
            throw new NotImplementedException();
        }

        public List<CRUDable> GetFullList()
        {
            throw new NotImplementedException();
        }

        internal static IEnumerable<ComboBoxItem> GetApplications(WebDaD.Toolkit.Database.Database db)
        {
            throw new NotImplementedException();
        }

        internal static string CreateHash(string app, string user)
        {
            //app|user
            throw new NotImplementedException();
        }
    }
}
