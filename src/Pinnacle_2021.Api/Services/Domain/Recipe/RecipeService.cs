using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using Flurl;
using Flurl.Http;

using Pinnacle_2021.Contracts.Responses.Recipe;


using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OneOf;

namespace Pinnacle_2021.Api.Services.Domain.Recipe
{
	public class RecipeService : IRecipeService
	{

		IConfiguration _config;
		public RecipeService(IConfiguration configuration)
		{
			_config = configuration;
		}
		public async Task<OneOf<IEnumerable<RecipeResponse>, string>> GetRecipeFromIngredients(string ingredients)
		{
			try
			{
				var temp = _config.GetSection("RecipeApi")["app_key"];

				var response = await "https://api.edamam.com/api/recipes/v2".SetQueryParams(
					new
					{
						app_key = _config.GetSection("RecipeApi")["app_key"],
						app_id = _config.GetSection("RecipeApi")["app_id"],
						q = ingredients,
						type = "public"
					}
				).GetJsonAsync<AllRecipeInfo>();

				//Trop noob pour auto-mapper
				var recipeList = new List<RecipeResponse>();
				
				foreach (var hit in response.Hits)
				{
					recipeList.Add(new RecipeResponse(hit.Recipe.Label, hit.Recipe.Image, hit.Recipe.Url, (int)hit.Recipe.Calories, hit.Recipe.MealType));
				}

				return recipeList;

			} catch (FlurlHttpException ex)
			{
				var error = await ex.GetResponseStringAsync();

				return error;
			}



			
		}
	}



    public partial class AllRecipeInfo
	{
		//[JsonProperty("from")]
		//public long From { get; set; }

		//[JsonProperty("to")]
		//public long To { get; set; }

		//[JsonProperty("count")]
		//public long Count { get; set; }

		//[JsonProperty("_links")]
		//public Links Links { get; set; }

		[JsonProperty("hits")]
		public Hit[] Hits { get; set; }
	}

	public partial class Hit
	{
		[JsonProperty("recipe")]
		public Recipe Recipe { get; set; }


	}

	public partial class Next
	{
		[JsonProperty("href")]
		public string Href { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }
	}

	public partial class Recipe
	{
		//[JsonProperty("uri")]
		//public string Uri { get; set; }

		[JsonProperty("label")]
		public string Label { get; set; }

		[JsonProperty("image")]
		public string Image { get; set; }

		//[JsonProperty("source")]
		//public string Source { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		//[JsonProperty("shareAs")]
		//public string ShareAs { get; set; }

		//[JsonProperty("yield")]
		//public long Yield { get; set; }

		//[JsonProperty("dietLabels")]
		//public string[] DietLabels { get; set; }

		//[JsonProperty("healthLabels")]
		//public string[] HealthLabels { get; set; }

		//[JsonProperty("cautions")]
		//public string[] Cautions { get; set; }

		//[JsonProperty("ingredientLines")]
		//public string[] IngredientLines { get; set; }

		//[JsonProperty("ingredients")]
		//public Ingredient[] Ingredients { get; set; }

		[JsonProperty("calories")]
		public long Calories { get; set; }

		//[JsonProperty("totalWeight")]
		//public long TotalWeight { get; set; }

		//[JsonProperty("cuisineType")]
		//public string[] CuisineType { get; set; }

		[JsonProperty("mealType")]
		public string[] MealType { get; set; }

		//[JsonProperty("dishType")]
		//public string[] DishType { get; set; }

		//[JsonProperty("totalNutrients")]
		//public TotalDaily TotalNutrients { get; set; }

		//[JsonProperty("digest")]
		//public Digest[] Digest { get; set; }
	}

	public partial class Digest
	{
		[JsonProperty("label")]
		public string Label { get; set; }

		[JsonProperty("tag")]
		public string Tag { get; set; }

		[JsonProperty("schemaOrgTag")]
		public string SchemaOrgTag { get; set; }

		[JsonProperty("total")]
		public long Total { get; set; }

		[JsonProperty("hasRDI")]
		public bool HasRdi { get; set; }

		[JsonProperty("daily")]
		public long Daily { get; set; }

		[JsonProperty("unit")]
		public string Unit { get; set; }

		[JsonProperty("sub")]
		public TotalDaily Sub { get; set; }
	}

	public partial class TotalDaily
	{
	}

	public partial class Ingredient
	{
		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("quantity")]
		public long Quantity { get; set; }

		[JsonProperty("measure")]
		public string Measure { get; set; }

		[JsonProperty("food")]
		public string Food { get; set; }

		[JsonProperty("weight")]
		public long Weight { get; set; }

		[JsonProperty("foodId")]
		public string FoodId { get; set; }
	}

	public class RecipeError
	{
		[JsonProperty("errorCode")]
		public string ErrorCode { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("params")]
		public string[] Params { get; set; }
	}
}
