using System;
using identityservice.Domain.UserAggregate;
using sharedkernel;

namespace identityservice.Domain.UserAggregate.Specs
{
    public class GetRegisteredUserSpec : BaseSpec<User>
    {
        public GetRegisteredUserSpec(Status status, int page, int pageSize) : base(x => x.Status == status)
        {
            AddPaging(page, pageSize);
            AddSortByDescendingExpression(x => x.CreatedDate);
        }
    }
}

