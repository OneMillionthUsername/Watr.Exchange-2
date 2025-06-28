using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Core;

namespace Watr.Exchange.Data.Core.ContactMechanisms
{
    public abstract class ContactMechanism : Vertex, IContactMechanism, IContactMechanismSpecification
    {
        public string? Name { get; set; }
        public bool CanContact { get; set; } = true;
        public ContactMechanismStereotypes Stereotype { get; set; }

        public abstract ContactMechanismTypes Type { get; }
    }
    public abstract class Phone : ContactMechanism, IPhone, IPhoneSpecification
    {
        public override ContactMechanismTypes Type => ContactMechanismTypes.Phone;

        public string CountryCode { get; set; } = null!;
        public string Number { get; set; } = null!;

        public abstract PhoneTypes PhoneType { get; }
    }

    public class MobilePhone : Phone, IMobilePhone
    {
        public override PhoneTypes PhoneType => PhoneTypes.Mobile;

        public bool? IMSGEnabled { get; set; }
        public bool? RCSEnabled { get; set; }
    }

    public class Landline : Phone, ILandline
    {
        public override PhoneTypes PhoneType => PhoneTypes.Landline;
    }
    public class Fax : Phone, IFax
    {
        public override PhoneTypes PhoneType => PhoneTypes.Fax;
    }
    public class EmailAddress : ContactMechanism, IEmailAddress
    {
        public override ContactMechanismTypes Type => ContactMechanismTypes.Email;

        string IEmailAddress.EmailAddress { get; set; } = null!;
    }
    public class MailingAddress : ContactMechanism, IMailingAddress
    {
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string City { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string? PostalCode { get; set; }
        public string Country { get; set; } = null!;

        public override ContactMechanismTypes Type => ContactMechanismTypes.MailingAddress;
    }
}
