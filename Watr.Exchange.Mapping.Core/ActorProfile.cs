using AutoMapper;
using System;
using Watr.Exchange.Business.Core;   // for CreateDTO, UpdateDTO, ReadDTO
using Watr.Exchange.Business;        // for Create*/Update* DTO classes
using Watr.Exchange.Data.Core;       // for Vertex & Actor classes
using Watr.Exchange.Core;
using System.Reflection.Metadata;            // for IActor, etc.


namespace Watr.Exchange.Mapping.Core
{
    public class UpdateGenericActorTypeConverter<TGenericUpdate> : ITypeConverter<TGenericUpdate, Actor>
        where TGenericUpdate: UpdateGenericActorDTO
    {
        public Actor Convert(TGenericUpdate source, Actor destination, ResolutionContext context)
        {
            bool isGroup = source.Stereotype == ActorStereotype.Group;
            if (source.Stereotype == ActorStereotype.Unknown)
                throw new InvalidDataException("Stereotype Unknown");
            switch (source.Type)
            {
                case ActorTypes.Unknown:
                    throw new InvalidDataException("Actor Type Unknown");
                case ActorTypes.Admin:
                    return source.MapGenericDto<TGenericUpdate, UpdateAdminDTO, Admin>(context);
                case ActorTypes.Expert:
                    switch (source.ExpertType)
                    {
                        case ExpertTypes.Unknown:
                            throw new InvalidDataException("Expert Type Unknown");
                        case ExpertTypes.Independent:
                            return isGroup
                                ? source.MapGenericDto<TGenericUpdate, UpdateIndependentGroupDTO, IndependentGroup>(context)
                                : source.MapGenericDto<TGenericUpdate, UpdateIndependentIndividualDTO, IndependentIndividual>(context);
                        case ExpertTypes.Staff:
                            return isGroup
                                ? source.MapGenericDto<TGenericUpdate, UpdateStaffGroupDTO, StaffGroup>(context)
                                : source.MapGenericDto<TGenericUpdate, UpdateStaffIndividualDTO, StaffIndividual>(context);
                        default:
                            throw new NotImplementedException();
                    }
                case ActorTypes.Patron:
                    switch (source.PatronType)
                    {
                        case PatronTypes.Unknown:
                            throw new InvalidDataException("Patron Type Unknown");
                        case PatronTypes.Investor:
                            switch (source.InvestorType)
                            {
                                case InvestorType.Unknown:
                                    throw new InvalidDataException("Investor Type Unknown");
                                case InvestorType.Unaccredited:
                                    return isGroup
                                        ? source.MapGenericDto<TGenericUpdate, UpdateGroupUnAccreditedInvestorDTO, GroupUnAccreditedInvestor>(context)
                                        : source.MapGenericDto<TGenericUpdate, UpdateIndividualAccreditedInvestorDTO, IndividualAccreditedInvestor>(context);
                                case InvestorType.Accredited:
                                    return isGroup
                                        ? source.MapGenericDto<TGenericUpdate, UpdateGroupAccreditedInvestorDTO, GroupUnAccreditedInvestor>(context)
                                        : source.MapGenericDto<TGenericUpdate, UpdateIndividualAccreditedInvestorDTO, IndividualAccreditedInvestor>(context);
                                default:
                                    throw new NotImplementedException();
                            }
                        case PatronTypes.Government:
                            if (isGroup)
                                return source.MapGenericDto<TGenericUpdate, UpdateGovernmentPatronDTO, GovernmentPatron>(context);
                            throw new InvalidDataException("Government cannot be an individual");
                        case PatronTypes.Corporation:
                            if (isGroup)
                                return source.MapGenericDto<TGenericUpdate, UpdateCorporationPatronDTO, CorporationPatron>(context);
                            throw new InvalidDataException("Corporation cannot be an individual");
                        default:
                            throw new NotImplementedException();
                    }
                default:
                    throw new NotImplementedException();
            }
        }
    }
    
    public class ActorMappingProfile : Profile
    {
        public ActorMappingProfile()
        {
            //
            //––– CREATE mappings: CreateDTO → Vertex
            //
            CreateMap<CreateActorDTO, Actor>()
                .Include<CreateAdminDTO, Admin>()
                .Include<CreateStaffIndividualDTO, StaffIndividual>()
                .Include<CreateStaffGroupDTO, StaffGroup>()
                .Include<CreateIndependentIndividualDTO, IndependentIndividual>()
                .Include<CreateIndependentGroupDTO, IndependentGroup>()
                .Include<CreateIndividualAccreditedInvestorDTO, IndividualAccreditedInvestor>()
                .Include<CreateGroupAccreditedInvestorDTO, GroupAccreditedInvestor>()
                .Include<CreateIndividualUnAccreditedInvestorDTO, IndividualUnAccreditedInvestor>()
                .Include<CreateGroupUnAccreditedInvestorDTO, GroupUnAccreditedInvestor>()
                .Include<CreateCorporationPatronDTO, CorporationPatron>()
                .Include<CreateGovernmentPatronDTO, GovernmentPatron>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.ETag, opt => opt.Ignore())
                .ForMember(dest => dest.TimeToLive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<CreateAdminDTO, Admin>();
            CreateMap<CreateStaffIndividualDTO, StaffIndividual>();
            CreateMap<CreateStaffGroupDTO, StaffGroup>();
            CreateMap<CreateIndependentIndividualDTO, IndependentIndividual>();
            CreateMap<CreateIndependentGroupDTO, IndependentGroup>();
            CreateMap<CreateIndividualAccreditedInvestorDTO, IndividualAccreditedInvestor>();
            CreateMap<CreateGroupAccreditedInvestorDTO, GroupAccreditedInvestor>();
            CreateMap<CreateIndividualUnAccreditedInvestorDTO, IndividualUnAccreditedInvestor>();
            CreateMap<CreateGroupUnAccreditedInvestorDTO, GroupUnAccreditedInvestor>();
            CreateMap<CreateCorporationPatronDTO, CorporationPatron>();
            CreateMap<CreateGovernmentPatronDTO, GovernmentPatron>();

            //
            //––– UPDATE mappings: UpdateDTO → Vertex
            //
            CreateMap<UpdateGenericActorDTO, UpdateAdminDTO>()
                .ForMember(dest => dest.FirstName, opt =>
                    opt.MapFrom(_ => StringIgnore.Ignore))
                  .ForMember(dest => dest.LastName, opt =>
                    opt.MapFrom(_ => StringIgnore.Ignore));

            CreateMap<UpdateGenericActorDTO, UpdateIndependentGroupDTO>();
            CreateMap<UpdateGenericActorDTO, Actor>()
                .ConvertUsing<UpdateGenericActorTypeConverter<UpdateGenericActorDTO>>();
            CreateMap<UpdateGenericIndividualDTO, Actor>()
                .ConvertUsing<UpdateGenericActorTypeConverter<UpdateGenericIndividualDTO>>();
            CreateMap<UpdateGenericGroupDTO, Actor>()
                .ConvertUsing<UpdateGenericActorTypeConverter<UpdateGenericGroupDTO>>();

            CreateMap<UpdateAdminDTO, Admin>().MapId<UpdateAdminDTO, Admin, Guid>().IgnoreGraphMetadata();
            CreateMap<UpdateAdminDTO, Actor>()
                .ConvertUsing((src, dest, ctx) => ctx.Mapper.Map<Admin>(src));
            CreateMap<UpdateIndependentIndividualDTO, Actor>()
                .ConvertUsing((src, dest, ctx) => ctx.Mapper.Map<IndependentIndividual>(src));
            CreateMap<UpdateStaffIndividualDTO, StaffIndividual>().IgnoreGraphMetadata();
            CreateMap<UpdateStaffGroupDTO, StaffGroup>().IgnoreGraphMetadata();
            CreateMap<UpdateIndependentIndividualDTO, IndependentIndividual>().IgnoreGraphMetadata();
            CreateMap<UpdateIndependentGroupDTO, IndependentGroup>().IgnoreGraphMetadata();
            CreateMap<UpdateIndividualAccreditedInvestorDTO, IndividualAccreditedInvestor>().IgnoreGraphMetadata();
            CreateMap<UpdateGroupAccreditedInvestorDTO, GroupAccreditedInvestor>().IgnoreGraphMetadata();
            CreateMap<UpdateIndividualUnAccreditedInvestorDTO, IndividualUnAccreditedInvestor>().IgnoreGraphMetadata();
            CreateMap<UpdateGroupUnAccreditedInvestorDTO, GroupUnAccreditedInvestor>().IgnoreGraphMetadata();
            CreateMap<UpdateCorporationPatronDTO, CorporationPatron>().IgnoreGraphMetadata();
            CreateMap<UpdateGovernmentPatronDTO, GovernmentPatron>().IgnoreGraphMetadata();

            //
            //––– READ mappings: Vertex → ReadDTO
            //
            CreateMap<Actor, ReadActorDTO>()
                .Include<Admin, ReadAdminDTO>()
                .Include<StaffIndividual, ReadStaffIndividualDTO>()
                .Include<StaffGroup, ReadStaffGroupDTO>()
                .Include<IndependentIndividual, ReadIndependentIndividualDTO>()
                .Include<IndependentGroup, ReadIndependentGroupDTO>()
                .Include<IndividualAccreditedInvestor, ReadIndividualAccreditedInvestorDTO>()
                .Include<GroupAccreditedInvestor, ReadGroupAccreditedInvestorDTO>()
                .Include<IndividualUnAccreditedInvestor, ReadIndividualUnAccreditedInvestorDTO>()
                .Include<GroupUnAccreditedInvestor, ReadGroupUnAccreditedInvestorDTO>()
                .Include<CorporationPatron, ReadCorporationPatronDTO>()
                .Include<GovernmentPatron, ReadGovernmentPatronDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.CreatedByUserId, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.CreatedByUserId) ? (Guid?)null : Guid.Parse(src.CreatedByUserId)))
                .ForMember(dest => dest.UpdatedByUserId, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.UpdatedByUserId) ? (Guid?)null : Guid.Parse(src.UpdatedByUserId)));

            CreateMap<Admin, ReadAdminDTO>();
            CreateMap<StaffIndividual, ReadStaffIndividualDTO>();
            CreateMap<StaffGroup, ReadStaffGroupDTO>();
            CreateMap<IndependentIndividual, ReadIndependentIndividualDTO>();
            CreateMap<IndependentGroup, ReadIndependentGroupDTO>();
            CreateMap<IndividualAccreditedInvestor, ReadIndividualAccreditedInvestorDTO>();
            CreateMap<GroupAccreditedInvestor, ReadGroupAccreditedInvestorDTO>();
            CreateMap<IndividualUnAccreditedInvestor, ReadIndividualUnAccreditedInvestorDTO>();
            CreateMap<GroupUnAccreditedInvestor, ReadGroupUnAccreditedInvestorDTO>();
            CreateMap<CorporationPatron, ReadCorporationPatronDTO>();
            CreateMap<GovernmentPatron, ReadGovernmentPatronDTO>();
            
        }
    }
}
