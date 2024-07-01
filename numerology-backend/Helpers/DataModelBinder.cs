using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OmsSolution.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace OmsSolution.Helpers
{
    public class DataModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            string dataQueryString = bindingContext.ValueProvider.GetValue("data").FirstValue;
            string decodedData = Uri.UnescapeDataString(dataQueryString);

            // Deserialize the JSON string to the DataModel
            DataModel model = JsonConvert.DeserializeObject<DataModel>(decodedData);

            bindingContext.Result = ModelBindingResult.Success(model);
            return Task.CompletedTask;
        }
    }
}
