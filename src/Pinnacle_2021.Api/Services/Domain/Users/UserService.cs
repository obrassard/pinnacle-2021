
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.EntityFrameworkCore;

using OneOf;

using Pinnacle_2021.Api.DAL;
using Pinnacle_2021.Api.Entities;
using Pinnacle_2021.Api.Models;
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

		#region Get

		public async Task<OneOf<UserResponse, EntityNotFound>> Get(string email)
		{
			var emailSearch = email.ToLower().Trim();
			var user = await Context.Users.Where(u => u.Email.ToLower().Trim() == emailSearch).FirstOrDefaultAsync();
			if (user == null)
				return new EntityNotFound();

			return Mapper.Map<UserResponse>(user);
		}

		#endregion

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
