using System;
using identityservice.Domain.UserAggregate;

namespace identityservice.Domain.UserAggregate.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        int Save();
        Task<int> SaveAsync(CancellationToken token = default(CancellationToken));
    }
}

