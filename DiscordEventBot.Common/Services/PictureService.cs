using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiscordEventBot.Common.Services
{
    public class PictureService
    {
        #region Private Fields

        private readonly HttpClient _http;

        #endregion Private Fields

        #region Public Constructors

        public PictureService(HttpClient http)
            => _http = http;

        #endregion Public Constructors

        #region Public Methods

        public async Task<Stream> GetCatPictureAsync()
        {
            var resp = await _http.GetAsync("https://cataas.com/cat");
            return await resp.Content.ReadAsStreamAsync();
        }

        #endregion Public Methods
    }
}