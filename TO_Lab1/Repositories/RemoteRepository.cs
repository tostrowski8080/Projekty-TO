using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TO_Lab1.Repositories
{
    public interface RemoteRepository
    {
        Task<byte[]> GetAsync(string url);
    }
}
