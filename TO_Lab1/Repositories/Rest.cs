using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TO_Lab1.Repositories
{
    public class Rest : RemoteRepository
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<byte[]> GetAsync(string url)
        {
            return await _client.GetByteArrayAsync(url);
        }
    }
}
