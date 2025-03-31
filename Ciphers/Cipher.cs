namespace Ciphers
{
    abstract class Cipher : ICipher
    {
        protected string Message { get; }
        protected string Key { get; }

        protected string encryptedMessage;
        public string EncryptedMessage
        {
            get
            {
                return encryptedMessage;
            }
        }
        public Cipher(string message, string key)
        {
            Message = message;
            Key = key;


        }
        /// <summary>
        /// метод шифровки
        /// </summary>

        protected virtual void Encrypt()
        {
            ValidateKey("");
        }

        protected virtual bool ValidateKey(string key)
        {

            return true;
        }

    }
}
