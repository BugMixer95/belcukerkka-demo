using System.Threading.Tasks;

namespace Belcukerkka.Services
{
    /// <summary>
    /// Specifies the contract for a renderer.
    /// </summary>
    public interface IPartialToStringRenderer
    {
        Task<string> RenderPartialToStringAsync<TModel>(string partialName, TModel model);
    }
}
