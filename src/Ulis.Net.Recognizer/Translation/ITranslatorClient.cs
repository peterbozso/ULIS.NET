using System.Threading.Tasks;

namespace Ulis.Net.Recognizer.Translation
{
    public interface ITranslatorClient
    {
        Task<string> TranslateAsync(string text);
    }
}