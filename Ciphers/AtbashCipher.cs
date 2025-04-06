using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciphers
{
    public class AtbashCipher : Cipher
    {
        private string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        public AtbashCipher(string message, string key) : base(message, key)
        {
            if (ValidateKey(Key))// для шифра Атбаш не нужен ключ
            {
                Encrypt();
            }
        }

        protected override void Encrypt()
        {
            foreach (char item in Message)
            {
                encryptedMessage += alphabet[alphabet.Length - alphabet.IndexOf(item) - 1];
            }
        }

        protected override bool ValidateKey(string key)
        {
            return true;
        }
    }
}
