
using AutoMapper;

using Pinnacle_2021.Api.DAL;

namespace Pinnacle_2021.Api.Services.System
{
	public class BaseDomainService
	{
		protected IMapper Mapper { get; }
		protected PinnacleContext Context { get; }

		public BaseDomainService(IMapper mapper, PinnacleContext context)
		{
			Mapper = mapper;
			Context = context;
		}
	}
}
