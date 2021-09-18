using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Pinnacle_2021.Contracts.Requests;
using Pinnacle_2021.Contracts.Responses;

namespace Pinnacle_2021.Api.Services.Domain
{
	public interface IInventoryService
	{
		Task<IEnumerable<InventoryResponse>> GetAll(Guid userId);
		Task<InventoryCreationResponse> Create(Guid userId, InventoryForCreation request);
	}
}