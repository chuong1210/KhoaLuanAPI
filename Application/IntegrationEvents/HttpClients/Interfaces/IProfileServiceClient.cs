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
        // UserProfile operations
        Task<UserProfileDto> GetUserProfileByIdAsync(string userProfileId);
        Task<bool> UserProfileExistsAsync(string userProfileId);

        // Address operations
        Task<AddressDto> GetAddressByIdAsync(string addressId);
        Task<List<AddressDto>> GetAddressesByUserProfileIdAsync(string userProfileId);
        Task<AddressDto> GetDefaultAddressAsync(string userProfileId);
        Task<bool> ValidateAddressAsync(string addressId, string userProfileId);
    }
}
