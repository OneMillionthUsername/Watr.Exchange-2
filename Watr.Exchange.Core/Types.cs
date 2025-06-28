using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Watr.Exchange.Core
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActorTypes
    {
        Unknown,
        Admin,
        Expert,
        Patron,
        //Community
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ExpertTypes
    {
        Unknown,
        Staff,
        Independent
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PatronTypes
    {
        Unknown,
        Investor,
        Government,
        Corporation
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InvestorType
    {
        Unknown,
        Accredited,
        Unaccredited,
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActorStereotype
    {
        Unknown,
        Individual,
        Group
    }
}
