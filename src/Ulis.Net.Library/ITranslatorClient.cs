using System.Threading.Tasks;

namespace Ulis.Net.Library
{
    public interface ITranslatorClient
    {
        Task<string> TranslateAsync(string text);
    }
}