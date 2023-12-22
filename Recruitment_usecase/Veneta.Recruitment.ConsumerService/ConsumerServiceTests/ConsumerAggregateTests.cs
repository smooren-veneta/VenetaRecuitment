using Veneta.Recruitment.ConsumerService;
using Veneta.Recruitment.ConsumerService.Commands.CreateConsumer;
using Veneta.Recruitment.ConsumerService.Commands.UpdateConsumer;
using Veneta.Recruitment.ConsumerService.Dependencies;
using Veneta.Recruitment.ConsumerService.ValueObjects;

namespace ConsumerServiceTests;

public class ConsumerAggregateTests
{
    private readonly ConsumerAggregate _sut;
    private readonly AggregateId _aggregateId = new(Guid.NewGuid());
    private readonly ConsumerId _consumerId = new(Guid.NewGuid());
    private readonly Address _address = new("Test Street", "12356", "c1 A");
    private readonly FirstName _firstName = new("first_name");
    private readonly LastName _lastName = new("last_name");

    public ConsumerAggregateTests()
    {
        _sut = new ConsumerAggregate();
    }

    [Fact]
    public void Given_ConsumerCreatedCommand_When_Apply_Then_ConsumerStateIsCreated()
    {
        // Arrange
        _sut.Initialize(_aggregateId, new List<IAggregateEvent<ConsumerAggregateEvent>>());
        var command = new CreateConsumerCommand(_consumerId, _address, _firstName, _lastName);

        // Act
        _sut.Handle(command);

        // Assert
        Assert.Equal(ConsumerStatus.Created, _sut.State.Status);
        Assert.Collection(_sut.Events,
            e => Assert.IsType<ConsumerAggregateEvent.ConsumerCreatedEvent>(e));
    }
    
    [Fact]
    public void Given_ConsumerUpdateCommand_FullName_When_Apply_Then_ConsumerStateIsUpdated()
    {
        // Arrange
        _sut.Initialize(_aggregateId, new List<IAggregateEvent<ConsumerAggregateEvent>>());
        var commandCreate = new CreateConsumerCommand(_consumerId, _address, _firstName, _lastName);
        _sut.Handle(commandCreate);
        
        var command = new UpdateConsumerCommand(_consumerId, _firstName, _lastName, null);

        // Act
        _sut.Handle(command);

        // Assert
        Assert.Equal(ConsumerStatus.Updated, _sut.State.Status);
        Assert.Collection(_sut.Events,
            e => Assert.IsType<ConsumerAggregateEvent.ConsumerCreatedEvent>(e),
            e => Assert.IsType<ConsumerAggregateEvent.ConsumerNameUpdatedEvent>(e));
    }
    
    [Fact]
    public void Given_ConsumerUpdateCommand_Address_When_Apply_Then_ConsumerStateIsUpdated()
    {
        // Arrange
        _sut.Initialize(_aggregateId, new List<IAggregateEvent<ConsumerAggregateEvent>>());
        var commandCreate = new CreateConsumerCommand(_consumerId, _address, _firstName, _lastName);
        _sut.Handle(commandCreate);
        
        var commandFullName = new UpdateConsumerCommand(_consumerId, _firstName, _lastName, null);
        _sut.Handle(commandFullName);
        
        var command = new UpdateConsumerCommand(_consumerId, null, null, _address);
        
        // Act
        _sut.Handle(command);

        // Assert
        Assert.Equal(ConsumerStatus.Updated, _sut.State.Status);
        Assert.Collection(_sut.Events,
            e => Assert.IsType<ConsumerAggregateEvent.ConsumerCreatedEvent>(e),
            e => Assert.IsType<ConsumerAggregateEvent.ConsumerNameUpdatedEvent>(e),
            e => Assert.IsType<ConsumerAggregateEvent.ConsumerAddressUpdatedEvent>(e)
            );
    }
}