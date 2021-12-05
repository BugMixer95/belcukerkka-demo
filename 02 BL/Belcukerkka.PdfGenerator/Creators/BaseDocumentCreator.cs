using System.Threading.Tasks;

namespace Belcukerkka.PdfGenerator.Creators
{
    public abstract class BaseDocumentCreator
    {
        public abstract Task<byte[]> CreateAsync(DocumentRequestModel model);
    }
}
