using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TO_Lab1.Encodings
{
    public interface Encoding
    {
        string getString(byte[] bytes);
    }
}
