using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Watr.Exchange.Core;

namespace Watr.Exchange.Data.Core
{
    public abstract class Actor : Vertex, IActor, IActorSpecification
    {
        public string Name { get; set; } = null!;
        public abstract ActorTypes Type { get; }
        public abstract ActorStereotype Stereotype { get; }
        public string EmailAddress { get; set; } = null!;
    }
    public class Admin : Actor, IAdmin, IConcrete
    {
        public override ActorTypes Type => ActorTypes.Admin;

        public override ActorStereotype Stereotype => ActorStereotype.Individual;

        public  string FirstName { get; set; } = null!;
        public  string LastName { get; set; } = null!;
    }
    public abstract class Expert : Actor, IExpert, IExpertSepcification
    {
        public override ActorTypes Type => ActorTypes.Expert;
        public abstract ExpertTypes ExpertType { get; }
    }
    public abstract class IndividualExpert : Expert, IIndividualActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Individual;
        public  string FirstName { get; set; } = null!;
        public  string LastName { get; set; } = null!;
    }
    public abstract class ExpertGroup : Expert, IGroupActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Group;
    }
    public class StaffIndividual : IndividualExpert, IStaff, IConcrete
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class StaffGroup : ExpertGroup, IStaff, IConcrete
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class IndependentIndividual : IndividualExpert, IIndependentExpert, IConcrete
    {
        public override ExpertTypes ExpertType =>  ExpertTypes.Independent;
    }
    public class IndependentGroup : ExpertGroup, IIndependentExpert, IConcrete
    {
        public override ExpertTypes ExpertType => ExpertTypes.Independent;
    }

    public abstract class Patron : Actor, IPatron, IPatronSpecification
    {
        public override ActorTypes Type => ActorTypes.Patron;

        public abstract PatronTypes PatronType { get; }
    }
    public abstract class GroupPatron : Patron, IGroupActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Group;
    }
    public abstract class IndividualPatron : Patron, IIndividualActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Individual;
        public  string FirstName { get; set; } = null!;
        public  string LastName { get; set; } = null!;
    }
    public abstract class IndividualInvestor : IndividualPatron, IInvestor, IInvestorSpecification
    {
        public override PatronTypes PatronType => PatronTypes.Investor;
        public abstract InvestorType InvestorType { get; }
    }
    public abstract class GroupInvestor : GroupPatron, IInvestor, IInvestorSpecification
    {
        public override PatronTypes PatronType => PatronTypes.Investor;
        public abstract InvestorType InvestorType { get; }
    }
    public class IndividualAccreditedInvestor : IndividualInvestor, IAccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class GroupAccreditedInvestor : GroupInvestor, IAccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class IndividualUnAccreditedInvestor : IndividualInvestor, IUnaccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited;
    }
    public class GroupUnAccreditedInvestor : GroupInvestor, IUnaccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited; 
    }
    public class CorporationPatron : GroupPatron, ICorporationPatron, IConcrete
    {
        public override PatronTypes PatronType => PatronTypes.Corporation;
    }
    public class GovernmentPatron : GroupPatron, IGorvernmentPatron, IConcrete
    {
        public override PatronTypes PatronType => PatronTypes.Government;
    }
}
