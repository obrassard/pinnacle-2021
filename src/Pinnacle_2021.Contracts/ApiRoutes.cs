namespace Pinnacle_2021.Contracts
{
	public static class ApiRoutes
	{
		public const string ROOT = "api";

		public static class Users
		{
			public const string BASE_ROUTE = ROOT + "/users";

			public const string GET = BASE_ROUTE;
			public const string CREATE = BASE_ROUTE;
		}
	}
}
