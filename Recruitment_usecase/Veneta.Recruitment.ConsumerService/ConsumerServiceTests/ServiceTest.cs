using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Veneta.Recruitment.ConsumerService.Models;
using Veneta.Recruitment.ConsumerService.Requests;
using Veneta.Recruitment.ConsumerService.ValueObjects;

namespace ConsumerServiceTests
{
    public class ServiceTest
    {
        private readonly WebApplicationFactory<Program> _program;

        public ServiceTest()
        {
            _program = new WebApplicationFactory<Program>();
        }

        [Fact]
        public async Task TestService()
        {
            var id = Guid.NewGuid();
            using var client = _program.CreateClient();
            var consumer = new CreateConsumerRequest(id, new Address("street", "postalCode", "houseNumber"), "firstName", "lastName");
            await client.PostAsJsonAsync("/consumers", consumer);
           
            //give projection time to update the view
            await Task.Delay(1000);

            var view = await client.GetFromJsonAsync<ConsumerView>($"/consumers/{id}");
            Assert.NotNull(view);
            Assert.Equal(consumer.FirstName, view.FirstName);
            Assert.Equal(consumer.LastName, view.LastName);
            Assert.Equal(consumer.Address.HouseNumber, view.Address.HouseNumber);
            Assert.Equal(consumer.Address.PostalCode, view.Address.PostalCode);
            Assert.Equal(consumer.Address.Street, view.Address.Street);
        }
    }
}