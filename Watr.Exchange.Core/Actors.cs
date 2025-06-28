using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watr.Exchange.Core
{
    
    public interface IActor : IObject
    {
        string Name { get; set; }
    }
    
    public interface IActorSpecification : ISpecification<ActorTypes, ActorStereotype>
    {
    }
    public interface IIndividualActor : IActor
    {
        string FirstName { get; set; }
        string LastName { get; set; }
    }
    public interface IAdmin : IIndividualActor, IConcrete { }
    public interface IExpertSepcification
    {
        ExpertTypes ExpertType { get; }
    }
    public interface IExpert : IActor { }
    public interface IStaff : IExpert { }
    public interface IIndependentExpert : IExpert { }
    public interface IPatron : IActor 
    {
        
    }
    public interface IPatronSpecification :IActorSpecification
    {
        PatronTypes PatronType { get; }
    }
    public interface IInvestor : IPatron 
    {
        
    }
    public interface IInvestorSpecification : IActorSpecification
    {
        InvestorType InvestorType { get; }
    }
    public interface IGorvernmentPatron : IPatron, IGroupActor, IConcrete { }
    public interface ICorporationPatron : IPatron, IGroupActor, IConcrete { }
    public interface IAccreditedInvestor : IPatron { }
    public interface IUnaccreditedInvestor : IPatron { }
    public interface IGroupActor : IActor
    {

    }
}
