using AutoMapper;
using Watr.Exchange.Core;
using Watr.Exchange.Data.Core;
using Watr.Exchange.Mapping.Core;

namespace Watr.Exchange.Business.Mappings.Test;

[TestClass]
public class MappingTests
{
    private IMapper Mapper { get; set; }
    public MappingTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ActorMappingProfile>();
        });
        config.AssertConfigurationIsValid();
        Mapper = config.CreateMapper();
    }

    [TestMethod]
    public void Map_UpdateAdminDTO_To_Admin()
    {
        var dto = new UpdateAdminDTO()
        {
            Id = Guid.NewGuid(),
            Name = "Alice Admin",
            EmailAddress = "alice@example.com",
            FirstName = "Alice",
            LastName = "Admin"
        };
        Actor result = Mapper.Map<Actor>(dto);
        Assert.IsInstanceOfType<Admin>(result);
        var admin = (Admin)result;
        Assert.AreEqual(dto.Name, admin.Name);
        Assert.AreEqual(dto.EmailAddress, admin.EmailAddress);
        Assert.AreEqual(dto.FirstName, admin.FirstName);
        Assert.AreEqual(dto.LastName, admin.LastName);
    }
    [TestMethod]
    public void Map_UpdateActorDTO_To_Admin()
    {
        var dto = new UpdateGenericActorDTO()
        {
            Id = Guid.NewGuid(),
            Name = "Alice Admin",
            EmailAddress = "alice@example.com",
            Stereotype = ActorStereotype.Individual,
            Type = ActorTypes.Admin
        };
        Actor result = Mapper.Map<Actor>(dto);
        Assert.IsInstanceOfType<Admin>(result);
        var admin = (Admin)result;
        Assert.AreEqual(dto.Name, admin.Name);
        Assert.AreEqual(dto.EmailAddress, admin.EmailAddress);
    }
}
