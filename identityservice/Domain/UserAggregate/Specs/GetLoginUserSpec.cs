using identityservice.Domain.UserAggregate;
using sharedkernel;

namespace identityservice.Domain.UserAggregate.Specs
{
    public class GetLoginUserSpec : BaseSpec<User>
    {
        public GetLoginUserSpec(string email, string password, Status status = Status.Active) : base(x => x.Email.EmailAddress == email
        && x.Password.Password == password
        && x.Status == status)
        {
            AddInclude(x => x.Role);
        }
    }
}

