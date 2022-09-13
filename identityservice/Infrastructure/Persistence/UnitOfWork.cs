using System;
using identityservice.Domain.UserAggregate;
using identityservice.Domain.UserAggregate.Interfaces;
using Microsoft.EntityFrameworkCore;
using sharedkernel.Interfaces;

namespace identityservice.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IdentityContext _context;

        public UnitOfWork(IdentityContext context)
        {
            _context = context;
        }

        private IUserRepository _userRepository { get; }
        public IUserRepository UserRepository
        {
            get { return _userRepository ?? new UserRepository(this._context); }
        }

        private IRoleRepository _roleRepository { get; }
        public IRoleRepository RoleRepository
        {
            get { return _roleRepository ?? new RoleRepository(this._context); }
        }

        private IRefreshTokenRepository _refreshTokenRepository { get; }
        public IRefreshTokenRepository RefreshTokenRepository
        {
            get { return _refreshTokenRepository ?? new RefreshTokenRepository(this._context); }
        }

        public int Save()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> SaveAsync(CancellationToken token = default(CancellationToken))
        {
            return await _context.SaveChangesAsync(token).ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }
    }
}

