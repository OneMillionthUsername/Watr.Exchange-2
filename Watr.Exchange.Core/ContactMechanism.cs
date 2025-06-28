using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Watr.Exchange.Core
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ContactMechanismTypes
    {
        Unknown,
        Phone,
        Email,
        MailingAddress
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ContactMechanismStereotypes
    {
        Unknown,
        Home,
        Work
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PhoneTypes
    {
        Unknown,
        Mobile,
        Landline,
        Fax
    }
    public interface IContactMechanismSpecification : ISpecification<ContactMechanismTypes>
    {
    }
    public interface IContactMechanism : IObject
    {
        string? Name { get; set; }
        bool CanContact { get; set; }
        ContactMechanismStereotypes Stereotype { get; set; }
    }
    public interface IPhoneSpecification : IContactMechanismSpecification
    {
        PhoneTypes PhoneType { get; }
    }
    public interface IPhone : IContactMechanism
    {
        string CountryCode { get; set; }
        string Number { get; set; }
    }
    public interface IMobilePhone : IPhone, IConcrete
    {
        bool? IMSGEnabled { get; set; }
        bool? RCSEnabled { get; set; }
    }
    public interface ILandline : IPhone, IConcrete
    {

    }
    public interface IFax : IPhone, IConcrete
    {

    }
    public interface IEmailAddress : IContactMechanism, IConcrete
    {
        string EmailAddress { get; set; }
    }
    public interface IMailingAddress : IContactMechanism, IConcrete
    {
        string AddressLine1 { get; set; }
        string? AddressLine2 { get; set; }
        string? AddressLine3 { get; set; }
        string City { get; set; }
        string Region { get; set; }
        string? PostalCode { get; set; }
        string Country { get; set; }
    }
}
