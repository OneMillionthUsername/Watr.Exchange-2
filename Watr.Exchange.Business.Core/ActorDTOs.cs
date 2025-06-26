using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Business.Core;
using Watr.Exchange.Core;
using Watr.Exchange.Data.Core;

namespace Watr.Exchange.Business
{
    public abstract class CreateActorDTO : CreateDTO, IActor
    {
        public string Name { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;

        public abstract ActorTypes Type { get; }

        public abstract ActorStereotype Stereotype { get; }
    }
    public class CreateAdminDTO : CreateActorDTO, IAdmin
    {
        public override ActorTypes Type => ActorTypes.Admin;

        public override ActorStereotype Stereotype => ActorStereotype.Individual;

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    public abstract class CreateExpertDTO : CreateActorDTO
    {
        public override ActorTypes Type => ActorTypes.Expert;
        public abstract ExpertTypes ExpertType { get; }
    }
    public abstract class CreateIndividualExpertDTO : CreateExpertDTO, IIndividualActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Individual;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    public abstract class CreateExpertGroupDTO : CreateExpertDTO, IGroupActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Group;
    }
    public class CreateStaffIndividualDTO : CreateIndividualExpertDTO, IStaff
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class CreateStaffGroupDTO : CreateExpertGroupDTO, IStaff
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class CreateIndependentIndividualDTO : CreateIndividualExpertDTO, IIndependentExpert
    {
        public override ExpertTypes ExpertType => ExpertTypes.Independent;
    }
    public class CreateIndependentGroupDTO : CreateExpertGroupDTO, IIndependentExpert
    {
        public override ExpertTypes ExpertType => ExpertTypes.Independent;
    }

    public abstract class CreatePatronDTO : CreateActorDTO, IPatron
    {
        public override ActorTypes Type => ActorTypes.Patron;

        public abstract PatronTypes PatronType { get; }
    }
    public abstract class CreateGroupPatronDTO : CreatePatronDTO, IGroupActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Group;
    }
    public abstract class CreateIndividualPatronDTO : CreatePatronDTO, IIndividualActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Individual;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    public abstract class CreateIndividualInvestorDTO : CreateIndividualPatronDTO, IInvestor
    {
        public override PatronTypes PatronType => PatronTypes.Investor;
        public abstract InvestorType InvestorType { get; }
    }
    public abstract class CreateGroupInvestorDTO : CreateGroupPatronDTO, IInvestor
    {
        public override PatronTypes PatronType => PatronTypes.Investor;
        public abstract InvestorType InvestorType { get; }
    }
    public class CreateIndividualAccreditedInvestorDTO : CreateIndividualInvestorDTO, IAccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class CreateGroupAccreditedInvestorDTO : CreateGroupInvestorDTO, IAccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class CreateIndividualUnAccreditedInvestorDTO : CreateIndividualInvestorDTO, IUnaccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited;
    }
    public class CreateGroupUnAccreditedInvestorDTO : CreateGroupInvestorDTO, IUnaccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited;
    }
    public class CreateCorporationPatronDTO : CreateGroupPatronDTO, ICorporationPatron
    {
        public override PatronTypes PatronType => PatronTypes.Corporation;
    }
    public class CreateGovernmentPatronDTO : CreateGroupPatronDTO, IGorvernmentPatron
    {
        public override PatronTypes PatronType => PatronTypes.Government;
    }


    public class UpdateActorDTO : UpdateDTO<Guid>, IActor
    {
        public string Name { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;

        public virtual ActorTypes Type { get; set; }

        public virtual ActorStereotype Stereotype { get; set; }
    }
    public class UpdateAdminDTO : UpdateActorDTO, IAdmin
    {
        public override ActorTypes Type => ActorTypes.Admin;

        public override ActorStereotype Stereotype => ActorStereotype.Individual;

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    public class UpdateExpertDTO : UpdateActorDTO
    {
        public override ActorTypes Type => ActorTypes.Expert;
        public virtual ExpertTypes ExpertType { get; set; }
    }
    public class UpdateIndividualExpertDTO : UpdateExpertDTO, IIndividualActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Individual;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    public class UpdateExpertGroupDTO : UpdateExpertDTO, IGroupActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Group;
    }
    public class UpdateStaffIndividualDTO : UpdateIndividualExpertDTO, IStaff
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class UpdateStaffGroupDTO : UpdateExpertGroupDTO, IStaff
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class UpdateIndependentIndividualDTO : UpdateIndividualExpertDTO, IIndependentExpert
    {
        public override ExpertTypes ExpertType => ExpertTypes.Independent;
    }
    public class UpdateIndependentGroupDTO : UpdateExpertGroupDTO, IIndependentExpert
    {
        public override ExpertTypes ExpertType => ExpertTypes.Independent;
    }

    public class UpdatePatronDTO : UpdateActorDTO, IPatron
    {
        public override ActorTypes Type => ActorTypes.Patron;

        public virtual PatronTypes PatronType { get; set; }
    }
    public class UpdateGroupPatronDTO : UpdatePatronDTO, IGroupActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Group;
    }
    public class UpdateIndividualPatronDTO : UpdatePatronDTO, IIndividualActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Individual;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    public class UpdateIndividualInvestorDTO : UpdateIndividualPatronDTO, IInvestor
    {
        public override PatronTypes PatronType => PatronTypes.Investor;
        public virtual InvestorType InvestorType { get; set; }
    }
    public class UpdateGroupInvestorDTO : UpdateGroupPatronDTO, IInvestor
    {
        public override PatronTypes PatronType => PatronTypes.Investor;
        public virtual InvestorType InvestorType { get; set; }
    }
    public class UpdateIndividualAccreditedInvestorDTO : UpdateIndividualInvestorDTO, IAccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class UpdateGroupAccreditedInvestorDTO : UpdateGroupInvestorDTO, IAccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class UpdateIndividualUnAccreditedInvestorDTO : UpdateIndividualInvestorDTO, IUnaccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited;
    }
    public class UpdateGroupUnAccreditedInvestorDTO : UpdateGroupInvestorDTO, IUnaccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited;
    }
    public class UpdateCorporationPatronDTO : UpdateGroupPatronDTO, ICorporationPatron
    {
        public override PatronTypes PatronType => PatronTypes.Corporation;
    }
    public class UpdateGovernmentPatronDTO : UpdateGroupPatronDTO, IGorvernmentPatron
    {
        public override PatronTypes PatronType => PatronTypes.Government;
    }

    public class DeleteActorDTO: DeleteDTO<Guid>
    {

    }

    public abstract class ReadActorDTO : ReadDTO<Guid>, IActor
    {
        public string Name { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;

        public abstract ActorTypes Type { get; }

        public abstract ActorStereotype Stereotype { get; }
    }
    public class ReadAdminDTO : ReadActorDTO, IAdmin
    {
        public override ActorTypes Type => ActorTypes.Admin;

        public override ActorStereotype Stereotype => ActorStereotype.Individual;

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    public abstract class ReadExpertDTO : ReadActorDTO
    {
        public override ActorTypes Type => ActorTypes.Expert;
        public abstract ExpertTypes ExpertType { get; }
    }
    public abstract class ReadIndividualExpertDTO : ReadExpertDTO, IIndividualActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Individual;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    public abstract class ReadExpertGroupDTO : ReadExpertDTO, IGroupActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Group;
    }
    public class ReadStaffIndividualDTO : ReadIndividualExpertDTO, IStaff
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class ReadStaffGroupDTO : ReadExpertGroupDTO, IStaff
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class ReadIndependentIndividualDTO : ReadIndividualExpertDTO, IIndependentExpert
    {
        public override ExpertTypes ExpertType => ExpertTypes.Independent;
    }
    public class ReadIndependentGroupDTO : ReadExpertGroupDTO, IIndependentExpert
    {
        public override ExpertTypes ExpertType => ExpertTypes.Independent;
    }

    public abstract class ReadPatronDTO : ReadActorDTO, IPatron
    {
        public override ActorTypes Type => ActorTypes.Patron;

        public abstract PatronTypes PatronType { get; }
    }
    public abstract class ReadGroupPatronDTO : ReadPatronDTO, IGroupActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Group;
    }
    public abstract class ReadIndividualPatronDTO : ReadPatronDTO, IIndividualActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Individual;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    public abstract class ReadIndividualInvestorDTO : ReadIndividualPatronDTO, IInvestor
    {
        public override PatronTypes PatronType => PatronTypes.Investor;
        public abstract InvestorType InvestorType { get; }
    }
    public abstract class ReadGroupInvestorDTO : ReadGroupPatronDTO, IInvestor
    {
        public override PatronTypes PatronType => PatronTypes.Investor;
        public abstract InvestorType InvestorType { get; }
    }
    public class ReadIndividualAccreditedInvestorDTO : ReadIndividualInvestorDTO, IAccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class ReadGroupAccreditedInvestorDTO : ReadGroupInvestorDTO, IAccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class ReadIndividualUnAccreditedInvestorDTO : ReadIndividualInvestorDTO, IUnaccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited;
    }
    public class ReadGroupUnAccreditedInvestorDTO : ReadGroupInvestorDTO, IUnaccreditedInvestor
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited;
    }
    public class ReadCorporationPatronDTO : ReadGroupPatronDTO, ICorporationPatron
    {
        public override PatronTypes PatronType => PatronTypes.Corporation;
    }
    public class ReadGovernmentPatronDTO : ReadGroupPatronDTO, IGorvernmentPatron
    {
        public override PatronTypes PatronType => PatronTypes.Government;
    }
}
