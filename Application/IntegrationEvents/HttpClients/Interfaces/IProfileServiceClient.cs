using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.IntegrationEvents.HttpClients.Dtos;
namespace Application.IntegrationEvents.HttpClients.Interfaces
{
    public interface IProfileServiceClient
    {
        //Task<UserProfileDto> GetUserProfileByIdAsync(string userProfileId);
        Task<AddressDto> GetAddressByIdAsync(string addressId);
    }
}
