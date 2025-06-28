using AutoMapper;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Business;
using Watr.Exchange.Business.Core;
using Watr.Exchange.Core;
using Watr.Exchange.Data.Core;
using Watr.Exchange.DTO;

namespace Watr.Exchange.Mapping.Core
{
    internal static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSrc, TDest> IgnoreGraphMetadata<TSrc, TDest>(
            this IMappingExpression<TSrc, TDest> expr)
            where TDest : IGraphObject
        {
            return expr
                .ForMember(d => d.CreateDate, o => o.Ignore())
                .ForMember(d => d.UpdateDate, o => o.Ignore())
                .ForMember(d => d.CreatedByUserId, o => o.Ignore())
                .ForMember(d => d.UpdatedByUserId, o => o.Ignore())
                .ForMember(d => d.ETag, o => o.Ignore())
                .ForMember(d => d.TimeToLive, o => o.Ignore())
                .ForMember(d => d.IsDeleted, o => o.Ignore());
        }
        public static IMappingExpression<TSrc, TDest> MapId<TSrc, TDest, TKey>(
            this IMappingExpression<TSrc, TDest> expr)
            where TDest : IGraphObject
            where TKey: IEquatable<TKey>
            where TSrc : IUpdateDTO<TKey>
        {
            return expr.ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id.ToString()));
        }
        public static TGraphObject MapGenericDto<TGenericDTO, TConcreteDTO, TGraphObject>(this TGenericDTO source, ResolutionContext context)
            where TGenericDTO : IDTO, IGeneric
            where TConcreteDTO : IDTO, IConcrete
            where TGraphObject : IGraphObject, IConcrete
        {
            var dto = context.Mapper.Map<TConcreteDTO>(source);
            return context.Mapper.Map<TGraphObject>(dto);
        }
    }
}
