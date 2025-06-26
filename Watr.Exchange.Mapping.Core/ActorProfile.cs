using AutoMapper;
using System;
using Watr.Exchange.Business.Core;   // for CreateDTO, UpdateDTO, ReadDTO
using Watr.Exchange.Business;        // for Create*/Update* DTO classes
using Watr.Exchange.Data.Core;       // for Vertex & Actor classes
using Watr.Exchange.Core;            // for IActor, etc.


namespace Watr.Exchange.Mapping.Core
{
    public class ActorMappingProfile : Profile
    {
        public ActorMappingProfile()
        {
            //
            //––– CREATE mappings: CreateDTO → Vertex
            //
            CreateMap<CreateActorDTO, Actor>()
                // include all concrete subtypes
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

                // generate a new GUID string for the Id, set audit dates
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(_ => DateTime.UtcNow))

                // ignore graph-specific or system fields on creation
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.ETag, opt => opt.Ignore())
                .ForMember(dest => dest.TimeToLive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            // concrete Create mappings
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
            //––– UPDATE mappings: UpdateDTO → existing Vertex
            //
            CreateMap<UpdateActorDTO, Actor>()
                .Include<UpdateAdminDTO, Admin>()
                
                .Include<UpdateStaffIndividualDTO, StaffIndividual>()
                .Include<UpdateStaffGroupDTO, StaffGroup>()
                .Include<UpdateIndependentIndividualDTO, IndependentIndividual>()
                .Include<UpdateIndependentGroupDTO, IndependentGroup>()
                .Include<UpdateIndividualAccreditedInvestorDTO, IndividualAccreditedInvestor>()
                .Include<UpdateGroupAccreditedInvestorDTO, GroupAccreditedInvestor>()
                .Include<UpdateIndividualUnAccreditedInvestorDTO, IndividualUnAccreditedInvestor>()
                .Include<UpdateGroupUnAccreditedInvestorDTO, GroupUnAccreditedInvestor>()
                .Include<UpdateCorporationPatronDTO, CorporationPatron>()
                .Include<UpdateGovernmentPatronDTO, GovernmentPatron>()

                // bump the UpdateDate
                .ForMember(dest => dest.UpdateDate, opt => opt.MapFrom(_ => DateTime.UtcNow))

                // do NOT overwrite system fields or the Id
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedByUserId, opt => opt.Ignore())
                .ForMember(dest => dest.ETag, opt => opt.Ignore())
                .ForMember(dest => dest.TimeToLive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            // concrete Update mappings
            CreateMap<UpdateAdminDTO, Admin>();
            CreateMap<UpdateStaffIndividualDTO, StaffIndividual>();
            CreateMap<UpdateStaffGroupDTO, StaffGroup>();
            CreateMap<UpdateIndependentIndividualDTO, IndependentIndividual>();
            CreateMap<UpdateIndependentGroupDTO, IndependentGroup>();
            CreateMap<UpdateIndividualAccreditedInvestorDTO, IndividualAccreditedInvestor>();
            CreateMap<UpdateGroupAccreditedInvestorDTO, GroupAccreditedInvestor>();
            CreateMap<UpdateIndividualUnAccreditedInvestorDTO, IndividualUnAccreditedInvestor>();
            CreateMap<UpdateGroupUnAccreditedInvestorDTO, GroupUnAccreditedInvestor>();
            CreateMap<UpdateCorporationPatronDTO, CorporationPatron>();
            CreateMap<UpdateGovernmentPatronDTO, GovernmentPatron>();

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

                // convert string IDs back into GUIDs, and parse audit fields
                .ForMember(dest => dest.Id,
                           opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.CreatedByUserId,
                           opt => opt.MapFrom(src => string.IsNullOrEmpty(src.CreatedByUserId)
                                                       ? (Guid?)null
                                                       : Guid.Parse(src.CreatedByUserId)))
                .ForMember(dest => dest.UpdatedByUserId,
                           opt => opt.MapFrom(src => string.IsNullOrEmpty(src.UpdatedByUserId)
                                                       ? (Guid?)null
                                                       : Guid.Parse(src.UpdatedByUserId)));

            // concrete Read mappings
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
