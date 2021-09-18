
using System.Threading.Tasks;

using AutoMapper;

using Pinnacle_2021.Api.DAL;
using Pinnacle_2021.Api.Entities;
using Pinnacle_2021.Api.Services.System;
using Pinnacle_2021.Contracts.Requests;
using Pinnacle_2021.Contracts.Responses;

namespace Pinnacle_2021.Api.Services.Domain
{
	public class UserService : BaseDomainService, IUserService
	{
		public UserService(IMapper mapper, PinnacleContext pinnacleContext) : base(mapper, pinnacleContext)
		{
		}

		#region Post

		public async Task<UserCreationResponse> Create(UserForCreation userRequest)
		{
			var user = Mapper.Map<User>(userRequest);
			await Context.Users.AddAsync(user);
			await Context.SaveChangesAsync();

			return Mapper.Map<UserCreationResponse>(user);
		}

		#endregion
	}
}
