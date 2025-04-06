using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciphers
{
    public class BlockCipher : Cipher
    {
        public BlockCipher(string message, string key) : base(message, key)
        {
            if (ValidateKey(Key))
            {
                Encrypt();
            }
        }

        protected override void Encrypt()
        {
            string[] blocks = DivideIntoBlocks(Message, Key.Length, '_');

            foreach (string block in blocks)
            {
                char[] encryptBlock = new char[Key.Length];

                for (int i = 0; i < Key.Length; i++)
                {
                    int keyIndex = int.Parse(Key[i].ToString()) - 1;
                    encryptBlock[keyIndex] = block[i];
                }

                foreach (char item in encryptBlock)
                {
                    encryptedMessage += item;
                }
            }
        }

        private string[] DivideIntoBlocks(string message, int length, char additionalSymbol)
        {
            bool endOfLastBlock = false;
            while (!endOfLastBlock)
            {
                if (message.Length % length != 0)
                {
                    message += additionalSymbol;
                }
                else
                {
                    endOfLastBlock = true;
                }
            }

            int blockIndex = 0;
            string block = string.Empty;
            List<string> blocks = new List<string>();
            for (int i = 0; i < message.Length; i++)
            {
                block += message[i];

                blockIndex++;
                if (blockIndex == length)
                {
                    blockIndex = 0;
                    blocks.Add(block);
                    block = string.Empty;
                }
            }

            return blocks.ToArray();
        }

        protected override bool ValidateKey(string key)
        {
            // добавить проверки
            return true;
        }
    }
}
