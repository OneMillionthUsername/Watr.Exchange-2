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
        Admin,
        Expert,
        Patron,
        Community
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ExpertTypes
    {
        Staff,
        Independent
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PatronTypes
    {
        Investor,
        Government,
        Corporation
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum InvestorType
    {
        Accredited,
        Unaccredited,
    }
    public enum ActorStereotype
    {
        Individual,
        Group
    }
}
