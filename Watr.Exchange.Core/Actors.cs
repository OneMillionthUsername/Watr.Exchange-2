using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watr.Exchange.Core
{
    public interface IActor
    {
        string Name { get; set; }
        string EmailAddress { get; set; }
        ActorTypes Type { get; }
        ActorStereotype Stereotype { get; }
    }
    public interface IIndividualActor : IActor
    {
        string FirstName { get; set; }
        string LastName { get; set; }
    }
    public interface IAdmin : IIndividualActor { }
    public interface IExpert : IActor { }
    public interface IStaff : IExpert { }
    public interface IIndependentExpert : IExpert { }
    public interface IPatron : IActor 
    {
        PatronTypes PatronType { get; }
    }
    public interface IInvestor : IPatron 
    {
        InvestorType InvestorType { get; }
    }
    public interface IGorvernmentPatron : IPatron, IGroupActor { }
    public interface ICorporationPatron : IPatron, IGroupActor { }
    public interface IAccreditedInvestor : IPatron { }
    public interface IUnaccreditedInvestor : IPatron { }
    public interface IGroupActor : IActor
    {

    }
}
