using System.Threading.Tasks;

namespace Ulis.Net.Library
{
    interface ITranslatorClient
    {
        Task<string> Translate(string text);
    }
}