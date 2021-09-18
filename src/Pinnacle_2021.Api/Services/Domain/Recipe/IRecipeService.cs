using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Pinnacle_2021.Contracts.Responses.Recipe;

namespace Pinnacle_2021.Api.Services.Domain.Recipe
{
	public interface IRecipeService
	{
		public Task<IEnumerable<RecipeResponse>> GetRecipeFromIngredients(string ingredients);
	}
}
