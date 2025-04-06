using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ciphers;

namespace AlghorithmsApp.Models.Cipher
{
    class CipherData
    {
        public string CipherName {  get; set; }

        public CipherData()
        {

        }

        public CipherData(string name)
        {
            CipherName = name;
        }
    }
}
