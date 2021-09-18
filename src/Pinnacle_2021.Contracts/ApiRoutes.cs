namespace Pinnacle_2021.Contracts
{
	public static class ApiRoutes
	{
		public const string ROOT = "api";

		public static class Recipe
		{
			public const string SEGMENT = "/recipes";
			public const string BASE_ROUTE = ROOT + SEGMENT;

			public const string GET = BASE_ROUTE;
		}

		public static class Inventories
		{
			public const string SEGMENT = "/inventories";
			public const string BASE_ROUTE = ROOT + SEGMENT;
			public const string BASE_USER_ROUTE = Users.GET_BY_ID + SEGMENT;
			public const string KEY = "inventoryId";

			public const string GET = BASE_USER_ROUTE;
			public const string GET_BY_ID = BASE_ROUTE + "/{" + KEY + "}";

			public const string CREATE = BASE_USER_ROUTE;
		}

		public static class Items
		{
			public const string BASE_ROUTE = ROOT + "/items";
			public const string KEY = "itemId";
			public const string INVENTORY_ITEM_ID = "inventoryItemId";

			public const string GET_BY_ID = BASE_ROUTE + "/{" + KEY + "}";

			public const string ADD_TO_INVENTORY = Inventories.GET_BY_ID + "/items";
			public const string CONSUME = BASE_ROUTE + "/{" + INVENTORY_ITEM_ID + "}";
			public const string CHANGE_QTY = BASE_ROUTE + "/{" + INVENTORY_ITEM_ID + "}";
		}

		public static class Users
		{
			public const string BASE_ROUTE = ROOT + SEGMENT;
			public const string SEGMENT = "/users";
			public const string KEY = "userId";

			public const string GET = BASE_ROUTE;
			public const string GET_BY_ID = BASE_ROUTE + "/{" + KEY + "}";

			public const string CREATE = BASE_ROUTE;
		}
	}
}
