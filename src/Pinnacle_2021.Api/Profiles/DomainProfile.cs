
using AutoMapper;

using Pinnacle_2021.Api.Entities;
using Pinnacle_2021.Contracts.Requests;
using Pinnacle_2021.Contracts.Responses;

namespace Pinnacle_2021.Api.Profiles
{
	public class DomainProfile : Profile
	{
		public DomainProfile()
		{
			CreateMap<UserForCreation, User>();
			CreateMap<User, UserResponse>()
				.ForMember(
					dest => dest.CompleteName,
					opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}")
				);


			CreateMap<InventoryForCreation, Inventory>();
			CreateMap<Inventory, InventoryCreationResponse>();

			CreateMap<Item, ItemResponse>();

			CreateMap<AddItemRequest, InventoryItem>();
		}
	}
}
