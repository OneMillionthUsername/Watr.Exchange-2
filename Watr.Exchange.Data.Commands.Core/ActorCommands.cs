using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watr.Exchange.Data.Core;

namespace Watr.Exchange.Data.Commands.Core
{
    public sealed class CreateAdmin : CreateVertex<Admin>
    {
        public CreateAdmin(Admin vertex) : base(vertex)
        {
        }
    }
    public sealed class CreateStaffIndividual : CreateVertex<StaffIndividual>
    {
        public CreateStaffIndividual(StaffIndividual vertex) : base(vertex)
        {
        }
    }
    public sealed class CreateStaffGroup : CreateVertex<StaffGroup>
    {
        public CreateStaffGroup(StaffGroup vertex) : base(vertex)
        {
        }
    }
    public sealed class CreateIndependentIndividual : CreateVertex<IndependentIndividual>
    {
        public CreateIndependentIndividual(IndependentIndividual vertex) : base(vertex)
        {
        }
    }
    public sealed class CreateIndependentGroup : CreateVertex<IndependentGroup>
    {
        public CreateIndependentGroup(IndependentGroup vertex) : base(vertex)
        {
        }
    }
    public sealed class CreateIndividualAccreditedInvestor : CreateVertex<IndividualAccreditedInvestor>
    {
        public CreateIndividualAccreditedInvestor(IndividualAccreditedInvestor vertex) : base(vertex)
        {
        }
    }
    public sealed class CreateGroupAccreditedInvestor : CreateVertex<GroupAccreditedInvestor>
    {
        public CreateGroupAccreditedInvestor(GroupAccreditedInvestor vertex) : base(vertex)
        {
        }
    }
    public sealed class CreateIndividualUnAccreditedInvestor : CreateVertex<IndividualUnAccreditedInvestor>
    {
        public CreateIndividualUnAccreditedInvestor(IndividualUnAccreditedInvestor vertex) : base(vertex)
        {
        }
    }
    public sealed class CreateGroupUnAccreditedInvestor : CreateVertex<GroupUnAccreditedInvestor>
    {
        public CreateGroupUnAccreditedInvestor(GroupUnAccreditedInvestor vertex) : base(vertex)
        {
        }
    }
    public sealed class CreateCorporationPatron : CreateVertex<CorporationPatron>
    {
        public CreateCorporationPatron(CorporationPatron vertex) : base(vertex)
        {
        }
    }
    public sealed class CreateGovernmentPatron : CreateVertex<GovernmentPatron>
    {
        public CreateGovernmentPatron(GovernmentPatron vertex) : base(vertex)
        {
        }
    }
}
