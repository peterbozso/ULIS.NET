using System.Threading.Tasks;
using Refit;

namespace Ulis.Net.Library.MicrosoftTranslator
{
    internal interface IMicrosoftTranslatorApi
    {
        [Post(@"/translate?api-version=3.0&to={targetLanguage}")]
        Task<string> Translate(string targetLanguage, [Body] MicrosoftTranslatorText[] text);
    }
}