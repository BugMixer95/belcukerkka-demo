using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Belcukerkka.Services
{
    /// <summary>
    /// Specifies the renderer of partial views to strings.
    /// </summary>
    public class PartialToStringRenderer : IPartialToStringRenderer
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IHttpContextAccessor _contextAccessor;

        public PartialToStringRenderer(IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IHttpContextAccessor contextAccessor)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Renders partial view with specified model to a string representation.
        /// </summary>
        /// <param name="partialName">Name of a partial view to be found.</param>
        /// <param name="model">Model that should be sent to a specified partial view.</param>
        /// <returns></returns>
        public async Task<string> RenderPartialToStringAsync<TModel>(string partialName, TModel model)
        {
            var actionContext = new ActionContext(_contextAccessor.HttpContext, 
                _contextAccessor.HttpContext.GetRouteData(), new ActionDescriptor());
            
            await using (StringWriter sw = new StringWriter())
            {
                var partialView = FindView(actionContext, partialName);

                var viewContext = new ViewContext(
                    actionContext,
                    partialView,
                    new ViewDataDictionary<TModel>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        actionContext.HttpContext,
                        _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await partialView.RenderAsync(viewContext);

                return sw.ToString();
            }
        }

        /// <summary>
        /// Searches for specified partial view.
        /// </summary>
        /// <param name="actionContext">Action context.</param>
        /// <param name="partialName">Name of a partial view to be searched for.</param>
        /// <returns>ViewEngineResult if specified partial view has been found; otherwise, throws an exception.</returns>
        private IView FindView(ActionContext actionContext, string partialName)
        {
            var getPartialResult = _viewEngine.GetView(null, partialName, false);
            
            if (getPartialResult.Success)
            {
                return getPartialResult.View;
            }
            
            var findPartialResult = _viewEngine.FindView(actionContext, partialName, false);
            
            if (findPartialResult.Success)
            {
                return findPartialResult.View;
            }
            
            var searchedLocations = getPartialResult.SearchedLocations.Concat(findPartialResult.SearchedLocations);
            
            var errorMessage = string.Join(
                Environment.NewLine,
                new[] { $"Unable to find partial '{partialName}'. The following locations were searched:" }.Concat(searchedLocations));
            
            throw new InvalidOperationException(errorMessage);
        }
    }
}
