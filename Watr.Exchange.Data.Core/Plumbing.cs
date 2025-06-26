using System;
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
        string EdgeName { get; }
    }
    public interface IEdge<TFrom, TTo> : IEdge
        where TFrom : IVertex
        where TTo : IVertex
    {
        [JsonIgnore]
        TFrom From { get; set; }
    }
    public interface ISingleEdge : IEdge { }
    public interface IMultiEdge : IEdge { }
    public interface ISingleEdge<TFrom, TTo> : ISingleEdge, IEdge<TFrom, TTo>
        where TFrom : IVertex
        where TTo : IVertex
    {
        [JsonIgnore]
        TTo? To { get; set; }
    }
    public interface IMultiEdge<TFrom, TTo> : IMultiEdge, IEdge<TFrom, TTo>
        where TFrom : IVertex
        where TTo : IVertex
    {
        [JsonIgnore]
        ICollection<TTo> To { get; set; }
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
        public virtual string PartitionKey => Id;
    }
    public abstract class Edge : GraphObject, IEdge
    {
        private static string? edgeName;
        public virtual string EdgeName
        {
            get
            {
                if (edgeName == null)
                    edgeName = GetType().Name;
                return edgeName;
            }
        }
    }
    public abstract class Edge<TFrom, TTo> : Edge, IEdge<TFrom, TTo>
        where TFrom : IVertex
        where TTo : IVertex
    {
        public TFrom From { get; set; } = default!;
    }
    public abstract class SingleEdge<TFrom, TTo> : Edge<TFrom, TTo>, ISingleEdge<TFrom, TTo>
        where TFrom : IVertex
        where TTo : IVertex
    {
        public TTo? To { get; set; }
    }
    public abstract class MultiEdge<TFrom, TTo> : Edge<TFrom, TTo>, IMultiEdge<TFrom, TTo>
        where TFrom : IVertex
        where TTo : IVertex
    {
        public virtual ICollection<TTo> To { get; set; } = [];
    }
}
