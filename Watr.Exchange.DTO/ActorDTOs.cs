using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Core;

namespace Watr.Exchange.DTO
{
    
    public abstract class CreateActorDTO : CreateDTO, IActor, IActorSpecification
    {
        public string Name { get; set; } = null!;

        public abstract ActorTypes Type { get; }

        public abstract ActorStereotype Stereotype { get; }
    }
    public class CreateAdminDTO : CreateActorDTO, IAdmin, IConcrete
    {
        public override ActorTypes Type => ActorTypes.Admin;

        public override ActorStereotype Stereotype => ActorStereotype.Individual;

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    public abstract class CreateExpertDTO : CreateActorDTO, IExpert, IExpertSepcification
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
    public class CreateStaffIndividualDTO : CreateIndividualExpertDTO, IStaff, IConcrete
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class CreateStaffGroupDTO : CreateExpertGroupDTO, IStaff, IConcrete
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class CreateIndependentIndividualDTO : CreateIndividualExpertDTO, IIndependentExpert, IConcrete
    {
        public override ExpertTypes ExpertType => ExpertTypes.Independent;
    }
    public class CreateIndependentGroupDTO : CreateExpertGroupDTO, IIndependentExpert, IConcrete
    {
        public override ExpertTypes ExpertType => ExpertTypes.Independent;
    }

    public abstract class CreatePatronDTO : CreateActorDTO, IPatron, IPatronSpecification
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
    public abstract class CreateIndividualInvestorDTO : CreateIndividualPatronDTO, IInvestor, IInvestorSpecification
    {
        public override PatronTypes PatronType => PatronTypes.Investor;
        public abstract InvestorType InvestorType { get; }
    }
    public abstract class CreateGroupInvestorDTO : CreateGroupPatronDTO, IInvestor, IInvestorSpecification
    {
        public override PatronTypes PatronType => PatronTypes.Investor;
        public abstract InvestorType InvestorType { get; }
    }
    public class CreateIndividualAccreditedInvestorDTO : CreateIndividualInvestorDTO, IAccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class CreateGroupAccreditedInvestorDTO : CreateGroupInvestorDTO, IAccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class CreateIndividualUnAccreditedInvestorDTO : CreateIndividualInvestorDTO, IUnaccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited;
    }
    public class CreateGroupUnAccreditedInvestorDTO : CreateGroupInvestorDTO, IUnaccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited;
    }
    public class CreateCorporationPatronDTO : CreateGroupPatronDTO, ICorporationPatron, IConcrete
    {
        public override PatronTypes PatronType => PatronTypes.Corporation;
    }
    public class CreateGovernmentPatronDTO : CreateGroupPatronDTO, IGorvernmentPatron, IConcrete
    {
        public override PatronTypes PatronType => PatronTypes.Government;
    }


    public class UpdateActorDTO : UpdateDTO<Guid>, IActor, IActorSpecification
    {
        public string Name { get; set; } = null!;

        public virtual ActorTypes Type { get; set; } = ActorTypes.Unknown;

        public virtual ActorStereotype Stereotype { get; set; } = ActorStereotype.Unknown;

       
    }
    public class UpdateGenericActorDTO : UpdateActorDTO,
        IExpertSepcification, IPatronSpecification, IInvestorSpecification, IGeneric
    {
        public virtual ExpertTypes ExpertType { get; set; } = ExpertTypes.Unknown;

        public virtual PatronTypes PatronType { get; set; } = PatronTypes.Unknown;

        public virtual InvestorType InvestorType { get; set; } = InvestorType.Unknown;
    }
    public class UpdateGenericIndividualDTO : UpdateGenericActorDTO, IIndividualActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Individual;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    public class UpdateGenericGroupDTO : UpdateGenericActorDTO, IGroupActor
    {
        public override ActorStereotype Stereotype => ActorStereotype.Group;
    }
    public class UpdateAdminDTO : UpdateActorDTO, IAdmin, IConcrete
    {
        public override ActorTypes Type => ActorTypes.Admin;

        public override ActorStereotype Stereotype => ActorStereotype.Individual;

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    public class UpdateExpertDTO : UpdateActorDTO, IExpert, IExpertSepcification
    {
        public override ActorTypes Type => ActorTypes.Expert;
        public virtual ExpertTypes ExpertType { get; set; } = ExpertTypes.Unknown;
    }
    public class UpdateGenericExpertDTO : UpdateActorDTO, IExpert, IExpertSepcification, IGeneric
    {
        public override ActorTypes Type => ActorTypes.Expert;
        public virtual ExpertTypes ExpertType { get; set; } = ExpertTypes.Unknown;
    }
    public class UpdateIndividualExpertDTO : UpdateExpertDTO, IIndividualActor, IConcrete
    {
        public override ActorStereotype Stereotype => ActorStereotype.Individual;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    public class UpdateExpertGroupDTO : UpdateExpertDTO, IGroupActor, IConcrete
    {
        public override ActorStereotype Stereotype => ActorStereotype.Group;
    }
    public class UpdateStaffIndividualDTO : UpdateIndividualExpertDTO, IStaff, IConcrete
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class UpdateStaffGroupDTO : UpdateExpertGroupDTO, IStaff, IConcrete
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class UpdateIndependentIndividualDTO : UpdateIndividualExpertDTO, IIndependentExpert, IConcrete
    {
        public override ExpertTypes ExpertType => ExpertTypes.Independent;
    }
    public class UpdateIndependentGroupDTO : UpdateExpertGroupDTO, IIndependentExpert, IConcrete
    {
        public override ExpertTypes ExpertType => ExpertTypes.Independent;
    }

    public class UpdatePatronDTO : UpdateActorDTO, IPatron, IPatronSpecification
    {
        public override ActorTypes Type => ActorTypes.Patron;

        public virtual PatronTypes PatronType { get; set; } = PatronTypes.Unknown;
    }
    public class UpdateGenericPatronDTO : UpdatePatronDTO, IInvestorSpecification, IConcrete
    {
        public virtual InvestorType InvestorType { get; set; } = InvestorType.Unknown;
    }
    public abstract class UpdateGroupPatronDTO : UpdatePatronDTO, IGroupActor, IConcrete
    {
        public override ActorStereotype Stereotype => ActorStereotype.Group;
    }
    public abstract class UpdateIndividualPatronDTO : UpdatePatronDTO, IIndividualActor, IConcrete
    {
        public override ActorStereotype Stereotype => ActorStereotype.Individual;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
    public class UpdateGenericInvestorDTO : UpdatePatronDTO, IInvestor, IInvestorSpecification, IConcrete
    {
        public override PatronTypes PatronType => PatronTypes.Investor;
        public virtual InvestorType InvestorType { get; set; } = InvestorType.Unaccredited;
    }
    public abstract class UpdateIndividualInvestorDTO : UpdateIndividualPatronDTO, IInvestor, IConcrete
    {
        public override PatronTypes PatronType => PatronTypes.Investor;
        public virtual InvestorType InvestorType { get; set; } = InvestorType.Unknown;
    }
    public abstract class UpdateGroupInvestorDTO : UpdateGroupPatronDTO, IInvestor, IInvestorSpecification, IConcrete
    {
        public override PatronTypes PatronType => PatronTypes.Investor;
        public virtual InvestorType InvestorType { get; set; } = InvestorType.Unknown;
    }
    public class UpdateIndividualAccreditedInvestorDTO : UpdateIndividualInvestorDTO, IAccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class UpdateGroupAccreditedInvestorDTO : UpdateGroupInvestorDTO, IAccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class UpdateIndividualUnAccreditedInvestorDTO : UpdateIndividualInvestorDTO, IUnaccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited;
    }
    public class UpdateGroupUnAccreditedInvestorDTO : UpdateGroupInvestorDTO, IUnaccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited;
    }
    public class UpdateCorporationPatronDTO : UpdateGroupPatronDTO, ICorporationPatron, IConcrete
    {
        public override PatronTypes PatronType => PatronTypes.Corporation;
    }
    public class UpdateGovernmentPatronDTO : UpdateGroupPatronDTO, IGorvernmentPatron, IConcrete
    {
        public override PatronTypes PatronType => PatronTypes.Government;
    }

    public class DeleteActorDTO: DeleteDTO<Guid>
    {

    }
    public class DeleteAdminDTO : DeleteActorDTO { }
    public abstract class ReadActorDTO : ReadDTO<Guid>, IActor, IActorSpecification
    {
        public string Name { get; set; } = null!;

        public abstract ActorTypes Type { get; }

        public abstract ActorStereotype Stereotype { get; }
    }
    public class ReadAdminDTO : ReadActorDTO, IAdmin, IConcrete
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
    public class ReadStaffIndividualDTO : ReadIndividualExpertDTO, IStaff, IConcrete
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class ReadStaffGroupDTO : ReadExpertGroupDTO, IStaff, IConcrete
    {
        public override ExpertTypes ExpertType => ExpertTypes.Staff;
    }
    public class ReadIndependentIndividualDTO : ReadIndividualExpertDTO, IIndependentExpert, IConcrete
    {
        public override ExpertTypes ExpertType => ExpertTypes.Independent;
    }
    public class ReadIndependentGroupDTO : ReadExpertGroupDTO, IIndependentExpert, IConcrete
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
    public class ReadIndividualAccreditedInvestorDTO : ReadIndividualInvestorDTO, IAccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class ReadGroupAccreditedInvestorDTO : ReadGroupInvestorDTO, IAccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Accredited;
    }
    public class ReadIndividualUnAccreditedInvestorDTO : ReadIndividualInvestorDTO, IUnaccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited;
    }
    public class ReadGroupUnAccreditedInvestorDTO : ReadGroupInvestorDTO, IUnaccreditedInvestor, IConcrete
    {
        public override InvestorType InvestorType => InvestorType.Unaccredited;
    }
    public class ReadCorporationPatronDTO : ReadGroupPatronDTO, ICorporationPatron, IConcrete
    {
        public override PatronTypes PatronType => PatronTypes.Corporation;
    }
    public class ReadGovernmentPatronDTO : ReadGroupPatronDTO, IGorvernmentPatron, IConcrete
    {
        public override PatronTypes PatronType => PatronTypes.Government;
    }
}
