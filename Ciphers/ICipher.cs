using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciphers
{
    internal interface ICipher
    {
        string EncryptedMessage { get; }
    }
}
