using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Watr.Exchange.Core;

namespace Watr.Exchange.Data.Core
{
    public interface IGraphObject : IObject
    {
        string Id { get; set; }
        DateTime CreateDate { get; set; }
        DateTime UpdateDate { get; set; }
        string? CreatedByUserId { get; set; }
        string? UpdatedByUserId { get; set; }
        string? ETag { get; set; }
        int? TimeToLive { get; set; }
        bool IsDeleted { get; set; }
    }
    public interface IVertex : IGraphObject
    {
        string PartitionKey { get; }

    }
    public interface IEdge : IGraphObject
    {
    }
    public interface IEdge<TFrom, TTo> : IEdge
        where TFrom : IVertex
        where TTo : IVertex
    {
        [JsonIgnore]
        TFrom From { get; set; }
    }
    public interface ISingleEdge : IEdge { }
    public interface IMultiEdge : IEnumerable { }
    public interface ISingleEdge<TFrom, TTo> : ISingleEdge, IEdgeValue<TFrom, TTo>
        where TFrom : IVertex
        where TTo : IVertex
    {
    }
    public interface IEdgeValue<TFrom, TTo> : IEdge<TFrom, TTo>
        where TFrom: IVertex
        where TTo: IVertex
    {
        [JsonIgnore]
        TTo? To { get; set; }
    }
    public interface IMultiEdge<TEdgeValue, TFrom, TTo> : IMultiEdge, IEnumerable<TEdgeValue>
        where TFrom : IVertex
        where TTo : IVertex
        where TEdgeValue : IEdgeValue<TFrom, TTo>
    {
        [JsonIgnore]
        ICollection<TEdgeValue> Values { get; set; }
    }

    public abstract class GraphObject : IGraphObject
    {

        public string Id { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string? CreatedByUserId { get; set; }
        public string? UpdatedByUserId { get; set; }
        public string? ETag { get; set; }
        public int? TimeToLive { get; set; }
        public bool IsDeleted { get; set; }
        
    }

    public abstract class Vertex : GraphObject, IVertex
    {
        private string? vertexName;

        public virtual string PartitionKey
        {
            get
            {
                vertexName ??= GetType().Name;
                return vertexName;
            }
        }
    }
    public abstract class Edge : GraphObject, IEdge
    {
    }
    public abstract class Edge<TFrom, TTo> : Edge, IEdge<TFrom, TTo>
        where TFrom : IVertex
        where TTo : IVertex
    {
        [JsonIgnore]
        public TFrom From { get; set; } = default!;
    }
    public abstract class SingleEdge<TFrom, TTo> : EdgeValue<TFrom, TTo>, ISingleEdge<TFrom, TTo>
        where TFrom : IVertex
        where TTo : IVertex
    {
    }
    public abstract class EdgeValue<TFrom, TTo> : Edge<TFrom, TTo>, IEdgeValue<TFrom, TTo>
        where TFrom : IVertex
        where TTo : IVertex
    {
        [JsonIgnore]
        public TTo? To { get; set; }
    }
    public abstract class MultiEdge<TEdgeValue, TFrom, TTo> : IMultiEdge<TEdgeValue, TFrom, TTo>
        where TFrom : IVertex
        where TTo : IVertex
        where TEdgeValue : IEdgeValue<TFrom, TTo>
    {
        [JsonIgnore]
        public virtual ICollection<TEdgeValue> Values { get; set; } = [];

        public IEnumerator<TEdgeValue> GetEnumerator()
        {
            foreach(var to in Values)
                yield return to;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
