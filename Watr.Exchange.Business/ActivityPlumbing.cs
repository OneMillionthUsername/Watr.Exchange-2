using AutoMapper;
using ExRam.Gremlinq.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Business.Core;
using Watr.Exchange.Core;
using Watr.Exchange.Data.Commands.Core;
using Watr.Exchange.Data.Core;
using Watr.Exchange.Data.Queries.Core;
using Watr.Exchange.DTO;

namespace Watr.Exchange.Business
{
    public abstract class Activity : IActivity
    {
        protected ILogger Logger { get; }
        protected IMediator Mediator { get; }
        protected IMapper Mapper { get; }
        public Activity(ILogger logger, IMediator mediator, IMapper mapper)
        {
            Logger = logger;
            Mediator = mediator;
            Mapper = mapper;
        }
    }
    public abstract class Activity<TKey> : Activity, IActivity<TKey>
        where TKey : IEquatable<TKey>
    {
        protected abstract TKey FromString(string key);
        protected virtual string FromKey(TKey key)
        {
            return key.ToString() ?? throw new InvalidDataException();
        }
        protected Activity(ILogger logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }
    }
    public abstract class Activity<TKey, TObject> : Activity<TKey>, IActivity<TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
    {
        protected Activity(ILogger<TObject> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }
    }
    public abstract class CommandActivity<TKey, TObject> : Activity<TKey, TObject>, ICommandActivity<TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
    {
        protected CommandActivity(ILogger<TObject> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }
    }
    public abstract class CreateActivity<TCreateDTO, TReadDTO, TKey, TObject, TVertex, TCreateQuery, TReadActivity>
        : CommandActivity<TKey, TObject>, ICreateActivity<TCreateDTO, TReadDTO, TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TVertex : TObject, IVertex, new()
        where TCreateDTO : ICreateDTO, TObject
        where TReadDTO : IReadDTO<TKey>, TObject
        where TReadActivity : IReadActivity<TReadDTO, TKey, TObject>
        where TCreateQuery: CreateVertex<TVertex>
    {
        protected abstract TCreateQuery CreateQuery(TVertex vertext);
        protected TReadActivity Read { get; }
        protected CreateActivity(ILogger<TObject> logger, IMediator mediator, IMapper mapper, TReadActivity activity) : base(logger, mediator, mapper)
        {
            Read = activity;
        }

        public virtual async Task<TReadDTO> Create(TCreateDTO dto, CancellationToken token = default)
        {
            try
            {
                var query = CreateQuery(Mapper.Map<TVertex>(dto));
                var id = FromString(await Mediator.Send(query, token));
                return (await Read.GetByKey(id, token)) ?? throw new InvalidDataException();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
    public abstract class CreateActivity<TCreateDTO, TReadDTO, TKey, TObject, TVertex> :
        CreateActivity<TCreateDTO, TReadDTO, TKey, TObject, TVertex, CreateVertex<TVertex>, IReadActivity<TReadDTO, TKey, TObject>>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TVertex : TObject, IVertex, new()
        where TCreateDTO : ICreateDTO, TObject
        where TReadDTO : IReadDTO<TKey>, TObject
    {
        protected override string FromKey(TKey key)
        {
            return key.ToString() ?? throw new NotImplementedException();
        }
        protected CreateActivity(ILogger<TObject> logger, IMediator mediator, IMapper mapper, IReadActivity<TReadDTO, TKey, TObject> activity) : base(logger, mediator, mapper, activity)
        {
        }
    }
    public abstract class GuidCreateActivity<TCreateDTO, TReadDTO, TObject, TVertex> :
        CreateActivity<TCreateDTO, TReadDTO, Guid, TObject, TVertex>
        where TObject : IObject
        where TVertex : TObject, IVertex, new()
        where TCreateDTO : ICreateDTO, TObject
        where TReadDTO : IReadDTO<Guid>, TObject
    {
        protected GuidCreateActivity(ILogger<TObject> logger, IMediator mediator, IMapper mapper, IReadActivity<TReadDTO, Guid, TObject> activity) : base(logger, mediator, mapper, activity)
        {
        }
        protected override Guid FromString(string key)
        {
            return Guid.Parse(key);
        }
    }
    public abstract class UpdateActivity<TUpdateDTO, TReadDTO, TKey, TObject, TVertex, TUpdateCommand, TReadActivity> :
        CommandActivity<TKey, TObject>, IUpdateActivity<TUpdateDTO, TReadDTO, TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TVertex : TObject, IVertex
        where TUpdateDTO : IUpdateDTO<TKey>, TObject
        where TReadDTO : IReadDTO<TKey>, TObject
        where TReadActivity : IReadActivity<TReadDTO, TKey, TObject>
        where TUpdateCommand : UpdateVertex<TVertex>
    {
        protected TReadActivity Read { get; }
        protected abstract TUpdateCommand CreateCommand(TVertex vertex);
        protected UpdateActivity(ILogger<TObject> logger, IMediator mediator, IMapper mapper, TReadActivity read) : base(logger, mediator, mapper)
        {
            Read = read;
        }

        public async Task<TReadDTO> Update(TUpdateDTO dto, CancellationToken token = default)
        {
            try
            {
                var vertex = Mapper.Map<TVertex>(dto);
                var updateCommand = CreateCommand(vertex);
                await Mediator.Send(updateCommand, token);
                return (await Read.GetByKey(dto.Id, token)) ?? throw new InvalidDataException();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
    public abstract class UpdateActivity<TUpdateDTO, TReadDTO, TKey, TObject, TVertex> :
        UpdateActivity<TUpdateDTO, TReadDTO, TKey, TObject, TVertex, UpdateVertex<TVertex>, IReadActivity<TReadDTO, TKey, TObject>>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TVertex : TObject, IVertex
        where TUpdateDTO : IUpdateDTO<TKey>, TObject
        where TReadDTO : IReadDTO<TKey>, TObject
    {
        protected UpdateActivity(ILogger<TObject> logger, IMediator mediator, IMapper mapper, IReadActivity<TReadDTO, TKey, TObject> read) : base(logger, mediator, mapper, read)
        {
        }
        protected override UpdateVertex<TVertex> CreateCommand(TVertex vertex)
        {
            return new UpdateVertex<TVertex>(vertex);
        }
    }
    public class GuidUpdateActivity<TUpdateDTO, TReadDTO, TObject, TVertex> :
        UpdateActivity<TUpdateDTO, TReadDTO, Guid, TObject, TVertex>, IConcrete
        where TObject : IObject
        where TVertex : TObject, IVertex
        where TUpdateDTO : IUpdateDTO<Guid>, TObject
        where TReadDTO : IReadDTO<Guid>, TObject
    {
        public GuidUpdateActivity(ILogger<TObject> logger, IMediator mediator, IMapper mapper, IReadActivity<TReadDTO, Guid, TObject> read) : base(logger, mediator, mapper, read)
        {
        }

        protected override Guid FromString(string key)
        {
            return Guid.Parse(key);
        }
    }
    public abstract class QueryActivity<TKey, TObject, TVertex, TCountQuery> : Activity<TKey, TObject>, IQueryActivity<TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TVertex: TObject, IVertex
        where TCountQuery: GetQueryPageCount<TVertex>
    {
        protected abstract TCountQuery CreateCountQuery(Expression<Func<TVertex, bool>>? filter = null);
        protected QueryActivity(ILogger<TObject> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }

        public virtual Task<long> Count(Expression<Func<TObject, bool>>? filter = null, CancellationToken token = default)
        {
            try
            {
                var countQuery = CreateCountQuery(filter?.ConvertBaseToDerived<TObject, TVertex>());
                return Mediator.Send(countQuery, token);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
    public abstract class DeleteActivity<TDeleteDTO, TKey, TObject, TVertex, TDeleteCommand, THardDeleteCommand> : CommandActivity<TKey, TObject>,
        IDeleteActivity<TDeleteDTO, TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TDeleteDTO : IDeleteDTO<TKey>
        where TVertex :TObject, IVertex
        where TDeleteCommand : DeleteVertex
        where THardDeleteCommand: HardDeleteVertex
        
    {
        protected abstract TDeleteCommand CreateDeleteCommand(TKey id);
        protected abstract THardDeleteCommand CreateHardDeleteCommand(TKey id);
        protected DeleteActivity(ILogger<TObject> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }

        public async Task Delete(TDeleteDTO dto, bool hardDrop = false, CancellationToken token = default)
        {
            try
            {
                if (hardDrop)
                {
                    var cmd = CreateHardDeleteCommand(dto.Id);
                    await Mediator.Send(cmd, token);
                }
                else
                {
                    var cmd = CreateDeleteCommand(dto.Id);
                    await Mediator.Send(cmd, token);
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
    public abstract class DeleteActivity<TDeleteDTO, TKey, TObject, TVertex> :
        DeleteActivity<TDeleteDTO, TKey, TObject, TVertex, DeleteVertex, HardDeleteVertex>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TDeleteDTO : IDeleteDTO<TKey>
        where TVertex : TObject, IVertex
    {
        protected override DeleteVertex CreateDeleteCommand(TKey id)
        {
            return new DeleteVertex(FromKey(id));
        }
        protected override HardDeleteVertex CreateHardDeleteCommand(TKey id)
        {
            return new HardDeleteVertex(FromKey(id));
        }
        protected DeleteActivity(ILogger<TObject> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }
    }
    public class GuidDeleteActivity<TDeleteDTO, TObject, TVertex> : DeleteActivity<TDeleteDTO, Guid, TObject, TVertex>, IConcrete
        where TObject : IObject
        where TDeleteDTO : IDeleteDTO<Guid>
        where TVertex : TObject, IVertex
    {
        public GuidDeleteActivity(ILogger<TObject> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }

        protected override Guid FromString(string key)
        {
            return Guid.Parse(key);
        }
    }
    public abstract class ReadActivity<TReadDTO, TKey, TObject, TVertex, TCountQuery, TReadQuery, TReadAllQuery> : 
        QueryActivity<TKey, TObject, TVertex, TCountQuery>,
        IReadActivity<TReadDTO, TKey, TObject>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TVertex : TObject, IVertex
        where TCountQuery : GetQueryPageCount<TVertex>
        where TReadDTO : IReadDTO<TKey>, TObject
        where TReadQuery : GetVertexById<TVertex>
        where TReadAllQuery : GetVertexes<TVertex>
    {
        protected abstract TReadAllQuery CreateReadAllQuery(Pager? pager = null, Expression<Func<TVertex, bool>>? filter = null,
            Expression<Func<IQueryable<TVertex>, IOrderedQueryable<TVertex>>>? orderBy = null);
        protected abstract TReadQuery CreateReadQuery(TKey key);
        protected ReadActivity(ILogger<TObject> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }

        public async virtual IAsyncEnumerable<TReadDTO> Get(Pager? pager = null, Expression<Func<TObject, bool>>? filter = null, 
            Expression<Func<IQueryable<TObject>, IOrderedQueryable<TObject>>>? orderBy = null, [EnumeratorCancellation] CancellationToken token = default)
        {
            TReadAllQuery? query = null;
            try
            {
                query = CreateReadAllQuery(pager, filter?.ConvertBaseToDerived<TObject, TVertex>(), orderBy?.Rebind<TObject, TVertex>());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
            await foreach (var item in Mediator.CreateStream(query, token))
            {
                yield return Mapper.Map<TReadDTO>(item);
            }
        }

        public virtual async Task<TReadDTO?> GetByKey(TKey key, CancellationToken token = default)
        {
            try
            {
                var query = CreateReadQuery(key);
                var vertex = await Mediator.Send(query, token);
                if(vertex != null)
                    return Mapper.Map<TReadDTO>(vertex);
                
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                throw;
            }
            return default;
        }
    }
    public abstract class ReadActivity<TReadDTO, TKey, TObject, TVertex> :
        ReadActivity<TReadDTO, TKey, TObject, TVertex, GetQueryPageCount<TVertex>, GetVertexById<TVertex>, GetVertexes<TVertex>>
        where TKey : IEquatable<TKey>
        where TObject : IObject
        where TVertex : TObject, IVertex
        where TReadDTO : IReadDTO<TKey>, TObject
    {
        public ReadActivity(ILogger<TObject> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }

        protected override GetQueryPageCount<TVertex> CreateCountQuery(Expression<Func<TVertex, bool>>? filter = null)
        {
            return new GetQueryPageCount<TVertex>(CreateReadAllQuery(filter: filter));
        }

        protected override GetVertexes<TVertex> CreateReadAllQuery(Pager? pager = null, Expression<Func<TVertex, bool>>? filter = null, Expression<Func<IQueryable<TVertex>, IOrderedQueryable<TVertex>>>? orderBy = null)
        {
            return new GetVertexes<TVertex>(pager, filter, orderBy);
        }

        protected override GetVertexById<TVertex> CreateReadQuery(TKey key)
        {
            return new GetVertexById<TVertex>(FromKey(key));
        }

       
    }
    public class GuidReadActivity<TReadDTO, TObject, TVertex> : ReadActivity<TReadDTO, Guid, TObject, TVertex>, IConcrete
            where TObject : IObject
            where TVertex : TObject, IVertex
            where TReadDTO : IReadDTO<Guid>, TObject
    {
        public GuidReadActivity(ILogger<TObject> logger, IMediator mediator, IMapper mapper) : base(logger, mediator, mapper)
        {
        }

        protected override Guid FromString(string key)
        {
            return Guid.Parse(key);
        }
    }
    /// <summary>
    /// Helpers to transform Expression&lt;Func&lt;TSource,bool&gt;&gt;
    /// into Expression&lt;Func&lt;TTarget,bool&gt;&gt;.
    /// </summary>
    public static class ExpressionConverter
    {
        /// <summary>
        /// If TTarget inherits from TSource, you can just swap the parameter type.
        /// </summary>
        public static Expression<Func<TTarget, bool>> ConvertBaseToDerived<TSource, TTarget>(
            this Expression<Func<TSource, bool>> source)
            where TTarget : TSource
        {
            // new parameter of the derived type:
            var newParam = Expression.Parameter(typeof(TTarget), source.Parameters[0].Name);

            // visitor that replaces the old param with the new one:
            var body = new ParamReplacer(source.Parameters[0], newParam)
                       .Visit(source.Body)!;

            return Expression.Lambda<Func<TTarget, bool>>(body, newParam);
        }

        /// <summary>
        /// If you have some mapping from TTarget → TSource, e.g. a selector
        /// Func&lt;TTarget,TSource&gt;, this will inline that mapping into the predicate.
        /// </summary>
        public static Expression<Func<TTarget, bool>> ConvertWithMapping<TSource, TTarget>(
            this Expression<Func<TSource, bool>> source,
            Expression<Func<TTarget, TSource>> mapping)
        {
            // e.g. mapping = targ => targ.AsTVertex()
            // newParam = mapping.Parameters[0]
            var newParam = mapping.Parameters[0];

            // inline mapping into the source predicate:
            // replace source.Parameters[0] with mapping.Body
            var inlinedBody = new ParamReplacer(source.Parameters[0], mapping.Body)
                              .Visit(source.Body)!;

            return Expression.Lambda<Func<TTarget, bool>>(inlinedBody, newParam);
        }

        private class ParamReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _from;
            private readonly Expression _to;

            public ParamReplacer(ParameterExpression from, Expression to)
            {
                _from = from;
                _to = to;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                // whenever you see the old parameter, swap in the new expression
                return node == _from ? _to : base.VisitParameter(node);
            }
        }
    }
    public static class OrderExpressionRebinder
    {
        public static Expression<Func<IQueryable<TVertex>, IOrderedQueryable<TVertex>>>
          Rebind<TObject, TVertex>(
            this Expression<Func<IQueryable<TObject>, IOrderedQueryable<TObject>>> expr)
        {
            // 1) Create a new root parameter of the target type
            var oldRoot = expr.Parameters[0];
            var newRoot = Expression.Parameter(
              typeof(IQueryable<TVertex>), oldRoot.Name);

            // 2) Visit & rewrite the body
            var body = new RebindVisitor<TObject, TVertex>(oldRoot, newRoot)
                         .Visit(expr.Body)!;

            // 3) Build the new lambda
            return Expression.Lambda<Func<IQueryable<TVertex>, IOrderedQueryable<TVertex>>>(
              body, newRoot);
        }

        private class RebindVisitor<TSource, TTarget> : ExpressionVisitor
        {
            private readonly ParameterExpression _oldRoot;
            private readonly ParameterExpression _newRoot;

            public RebindVisitor(ParameterExpression oldRoot, ParameterExpression newRoot)
            {
                _oldRoot = oldRoot;
                _newRoot = newRoot;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                // swap out the root IQueryable<TSource>
                if (node == _oldRoot)
                    return _newRoot;
                return base.VisitParameter(node);
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                // only rewrite the static Queryable.OrderBy* / ThenBy* methods
                if (node.Method.DeclaringType == typeof(Queryable) &&
                    (node.Method.Name == nameof(Queryable.OrderBy) ||
                     node.Method.Name == nameof(Queryable.OrderByDescending) ||
                     node.Method.Name == nameof(Queryable.ThenBy) ||
                     node.Method.Name == nameof(Queryable.ThenByDescending)))
                {
                    // get the unbound generic definition, e.g. OrderBy<TSource, TKey>
                    var genericDef = node.Method.GetGenericMethodDefinition();
                    var args = node.Method.GetGenericArguments(); // [TSource, TKey]
                    var keyType = args[1];

                    // make a new method OrderBy<TTarget, TKey>
                    var newMethod = genericDef.MakeGenericMethod(
                      typeof(TTarget), keyType);

                    // rewrite the source sequence (first arg)
                    var newSource = Visit(node.Arguments[0]);

                    // extract & rewrite the projection lambda (second arg)
                    var oldLambda = (LambdaExpression)StripQuotes(
                      node.Arguments[1]);

                    // create a new lambda param of type TTarget
                    var newParam = Expression.Parameter(
                      typeof(TTarget), oldLambda.Parameters[0].Name);

                    // replace oldParam -> newParam in the body
                    var newBody = new ParamReplacer(oldLambda.Parameters[0], newParam)
                                  .Visit(oldLambda.Body)!;

                    // re-quote into an Expression<Func<TTarget, TKey>>
                    var newLambda = Expression.Quote(
                      Expression.Lambda(newBody, newParam));

                    // build the new method call
                    return Expression.Call(
                      null,
                      newMethod,
                      newSource,
                      newLambda);
                }

                return base.VisitMethodCall(node);
            }

            private static Expression StripQuotes(Expression e)
            {
                while (e.NodeType == ExpressionType.Quote)
                    e = ((UnaryExpression)e).Operand;
                return e;
            }
        }

        private class ParamReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _from;
            private readonly Expression _to;

            public ParamReplacer(ParameterExpression from, Expression to)
            {
                _from = from;
                _to = to;
            }

            protected override Expression VisitParameter(ParameterExpression node)
                => node == _from ? _to : base.VisitParameter(node);
        }
    }

}
