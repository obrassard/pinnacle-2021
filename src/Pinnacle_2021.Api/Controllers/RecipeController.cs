using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Pinnacle_2021.Api.Services.Domain.Recipe;
using Pinnacle_2021.Contracts;
using Pinnacle_2021.Contracts.Responses;
using Pinnacle_2021.Contracts.Responses.Recipe;

namespace Pinnacle_2021.Api.Controllers
{
	[ApiController]
	public class RecipeController: ControllerBase
	{

		private readonly IRecipeService _recipeService;

		public RecipeController(IRecipeService recipeService)
		{
			_recipeService = recipeService;
		}

		[HttpGet(ApiRoutes.Recipe.GET)]
		public async Task<ActionResult<IEnumerable<RecipeResponse>>> Get(string ingredients)
		{
			var response = await _recipeService.GetRecipeFromIngredients(ingredients);

			return Ok(response);
		}
	}
}
