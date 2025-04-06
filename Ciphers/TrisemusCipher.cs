using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciphers
{
    public class TrisemusCipher : Cipher
    {
        private string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        public TrisemusCipher(string message, string key) : base(message, key)
        {
            if (ValidateKey(Key))
            {
                Encrypt();
            }
        }

        protected override void Encrypt()
        {
            string encryptedAlphabet = encryptAlphabet();
            int sideSizeAlpabetTable = (int)Math.Ceiling(Math.Sqrt(alphabet.Length));

            foreach (char item in Message)
            {
                int currentIndex = encryptedAlphabet.IndexOf(item);
                currentIndex += sideSizeAlpabetTable;

                if (currentIndex > alphabet.Length)
                {
                    if (currentIndex < Math.Pow(sideSizeAlpabetTable, 2))
                    {
                        currentIndex += sideSizeAlpabetTable;
                    }
                    currentIndex -= alphabet.Length;
                }
                encryptedMessage += encryptedAlphabet[currentIndex];
            }
        }

        private string encryptAlphabet()
        {
            string encryptedAlphabet = new string(Key.Distinct().ToArray());
            foreach (char item in alphabet)
            {
                if (!encryptedAlphabet.Contains(item))
                {
                    encryptedAlphabet += item;
                }
            }
            return encryptedAlphabet;
        }

        protected override bool ValidateKey(string key)
        {
            //потом доделаю
            return true;
        }
    }
}
