using CoreModels = ExpressFoodDelivery.Orders.Core.Model;
using ExpressFoodDelivery.Orders.Contracts;

namespace ExpressFoodDelivery.Orders.WebApi.ContractMappers
{
    public static class AddressMapper
    {
        public static CoreModels.Address Map(Address contractModel)
            => new CoreModels.Address(contractModel.AddressLine1, contractModel.AddressLine2, 
                contractModel.City, contractModel.Postcode, contractModel.Country);
    }
}
