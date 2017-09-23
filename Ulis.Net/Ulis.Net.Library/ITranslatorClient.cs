using System.Threading.Tasks;

namespace Ulis.Net.Library
{
    interface ITranslatorClient
    {
        Task<string> TranslateAsync(string text);
    }
}