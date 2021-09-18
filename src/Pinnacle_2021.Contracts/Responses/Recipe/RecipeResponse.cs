namespace Pinnacle_2021.Contracts.Responses.Recipe
{
	public record RecipeResponse(string Label,
		string Image, string Url, int Calories, string[] MealType, string[] DishType);
}
