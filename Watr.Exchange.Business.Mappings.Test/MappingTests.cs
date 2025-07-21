using AutoMapper;
using Watr.Exchange.Core;
using Watr.Exchange.Data.Core.Accounts;
using Watr.Exchange.Data.Core.Actors;
using Watr.Exchange.DTO;
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
            cfg.AddProfile<AccountMappingProfile>();
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
            FirstName = "Alice",
            LastName = "Admin"
        };
        Actor result = Mapper.Map<Actor>(dto);
        Assert.IsInstanceOfType<Admin>(result);
        var admin = (Admin)result;
        Assert.AreEqual(dto.Name, admin.Name);
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
            Stereotype = ActorStereotype.Individual,
            Type = ActorTypes.Admin
        };
        Actor result = Mapper.Map<Actor>(dto);
        Assert.IsInstanceOfType<Admin>(result);
        var admin = (Admin)result;
        Assert.AreEqual(dto.Name, admin.Name);
    }
    [TestMethod]
    public void Map_UpdateGroupIndependentExpert()
    {
        var dto = new UpdateIndependentIndividualDTO()
        {
            Id = Guid.NewGuid(),
            Name = "Alice Independent Expert",
            FirstName = "Alice",
            LastName = "Independent"
        };
        Actor result = Mapper.Map<Actor>(dto);
    }

	//
	//---Acount Mappings---
    //
	[TestMethod]
	public void Map_CreateAccountDTO_To_Account()
	{
		var dto = new CreateAccountDTO
		{
			FirstName = "Max",
			LastName = "Mustermann",
			ObjectId = "obj-789"
		};

		var result = Mapper.Map<Account>(dto);
        Assert.IsInstanceOfType<Account>(result);
		Assert.IsNotNull(result);
		Assert.AreEqual(dto.FirstName, result.FirstName);
		Assert.AreEqual(dto.LastName, result.LastName);
		Assert.AreEqual(dto.ObjectId, result.ObjectId);
	}

	[TestMethod]
	public void Map_Account_To_CreateAccountDTO()
	{
		var account = new Account
		{
			FirstName = "Erika",
			LastName = "Musterfrau",
			ObjectId = "obj-123"
		};

		var result = Mapper.Map<CreateAccountDTO>(account);

		Assert.IsNotNull(result);
        Assert.IsInstanceOfType<CreateAccountDTO>(result);
		Assert.AreEqual(account.FirstName, result.FirstName);
		Assert.AreEqual(account.LastName, result.LastName);
		Assert.AreEqual(account.ObjectId, result.ObjectId);
	}

	[TestMethod]
	public void Map_CreateRoleDTO_To_Role()
	{
		var dto = new CreateRoleDTO { Name = "Admin" };
		var result = Mapper.Map<Role>(dto);

		Assert.IsNotNull(result);
        Assert.IsInstanceOfType<Role>(result);
		Assert.AreEqual(dto.Name, result.Name);
	}

	[TestMethod]
	public void Map_Role_To_ReadRoleDTO()
	{
		var role = new Role { Name = "User" };
		var result = Mapper.Map<ReadRoleDTO>(role);

		Assert.IsNotNull(result);
        Assert.IsInstanceOfType<ReadRoleDTO>(result);
		Assert.AreEqual(role.Name, result.Name);
	}

    [TestMethod]
    public void Map_ReadAccountDTO_To_Account()
    {
        var dto = new ReadAccountDTO
        {
            FirstName = "John",
            LastName = "Doe",
            ObjectId = "obj-456",
            Roles = new List<ReadRoleDTO>
            {
                new ReadRoleDTO { Id = Guid.NewGuid(), Name = "User" }
            }
        };
        var result = Mapper.Map<Account>(dto);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<Account>(result);
		Assert.AreEqual(dto.FirstName, result.FirstName);
        Assert.AreEqual(dto.LastName, result.LastName);
        Assert.AreEqual(dto.ObjectId, result.ObjectId);
	}

    [TestMethod]
    public void Map_Account_To_ReadAccountDTO()
    {
        var account = new Account
        {
            FirstName = "Jane",
            LastName = "Doe",
            ObjectId = "obj-123",
        };
        var result = Mapper.Map<ReadAccountDTO>(account);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType<ReadAccountDTO>(result);
		Assert.AreEqual(account.FirstName, result.FirstName);
        Assert.AreEqual(account.LastName, result.LastName);
        Assert.AreEqual(account.ObjectId, result.ObjectId);
	}

}
