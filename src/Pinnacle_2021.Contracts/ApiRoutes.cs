namespace Pinnacle_2021.Contracts
{
	public static class ApiRoutes
	{
		public const string ROOT = "api";

		public static class Inventories
		{
			public const string SEGMENT = "/inventories";
			public const string BASE_ROUTE = Users.GET_BY_ID + SEGMENT;

			public const string GET = BASE_ROUTE;
			public const string CREATE = BASE_ROUTE;
		}

		public static class Items
		{
			public const string GET = ROOT + "/items";
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
