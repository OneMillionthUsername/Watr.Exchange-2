using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Watr.Exchange.Core;

namespace Watr.Exchange.Data.Core
{
    public abstract class Actor : Vertex, IActor
    {
        public string Name { get; set; } = null!;
        public abstract ActorTypes Type { get; }
        public abstract ActorStereotype Stereotype { get; }
        public string EmailAddress { get; set; } = null!;
    }
    public class Admin : Actor, IAdmin
    {
        public override ActorTypes Type => ActorTypes.Admin;

        public override ActorStereotype Stereotype => ActorStereotype.Individual;

        public  string FirstName { get; set; } = null!;
        public  string LastName { get; set; } = null!;
    }
    public abstract class Expert : Actor
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
    public class StaffIndividual : IndividualExpert, IStaff
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class StaffGroup : ExpertGroup, IStaff
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class IndependentIndividual : IndividualExpert, IIndependentExpert
    {
        public override ExpertTypes ExpertType =>  ExpertTypes.Independent;
    }
    public class IndependentGroup : ExpertGroup, IIndependentExpert
    {
        public override ExpertTypes ExpertType => ExpertTypes.Independent;
    }

    public abstract class Patron : Actor, IPatron
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
    public abstract class IndividualInvestor : IndividualPatron, IInvestor
    {
        public override PatronTypes PatronType => PatronTypes.Investor;
        public abstract InvestorType InvestorType { get; }
    }
    public abstract class GroupInvestor : GroupPatron, IInvestor
    {
        public override PatronTypes PatronType => PatronTypes.Investor;
        public abstract InvestorType InvestorType { get; }
    }
    public class IndividualAccreditedInvestor : IndividualInvestor, IAccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class GroupAccreditedInvestor : GroupInvestor, IAccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class IndividualUnAccreditedInvestor : IndividualInvestor, IUnaccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited;
    }
    public class GroupUnAccreditedInvestor : GroupInvestor, IUnaccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited;
    }
    public class CorporationPatron : GroupPatron, ICorporationPatron
    {
        public override PatronTypes PatronType => PatronTypes.Corporation;
    }
    public class GovernmentPatron : GroupPatron, IGorvernmentPatron
    {
        public override PatronTypes PatronType => PatronTypes.Government;
    }
}
