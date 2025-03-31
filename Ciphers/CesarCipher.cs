using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciphers
{
    class CesarCipher : Cipher
    {
        private string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        public CesarCipher(string message, string key) : base(message, key)
        {
            if (ValidateKey(Key))
            {
                Encrypt();
            }
        }

        protected override void Encrypt()
        {
            foreach (char item in Message)
            {
                int alphabetCharIndex;
                if (alphabet.Contains(item))
                {
                    alphabetCharIndex = alphabet.IndexOf(item);
                    encryptedMessage += alphabet[IndexOfEncryptedChar(alphabetCharIndex, Key)];
                }
            }
        }

        private int IndexOfEncryptedChar(int alphabetCharIndex, string key)
        {
            int lastAlphabetIndex = alphabet.Length - 1;
            int indexOfEncryptedChar = 0;
            if (key[0] == '-')
            {
                //обработка минусового значения
                //доделать
            }
            else
            {
                indexOfEncryptedChar = alphabetCharIndex + int.Parse(key);
                if (indexOfEncryptedChar > lastAlphabetIndex)
                {
                    indexOfEncryptedChar -= lastAlphabetIndex + 1;
                }

            }
            return indexOfEncryptedChar;
        }

        protected override bool ValidateKey(string key)
        {
            //метод проверки
            return true;
        }
    }
}
