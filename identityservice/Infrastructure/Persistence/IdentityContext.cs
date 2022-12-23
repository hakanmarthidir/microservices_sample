using identityservice.Domain.UserAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using sharedkernel;

namespace identityservice.Infrastructure.Persistence
{
    public class IdentityContext : DbContext
    {
        private readonly IMediator? _mediator;
        public DbSet<Domain.UserAggregate.User> Users { get; set; }
        public DbSet<Domain.UserAggregate.Role> Roles { get; set; }
        public DbSet<RefreshToken> Tokens { get; set; }


        public IdentityContext(DbContextOptions<IdentityContext> options, IMediator? mediator)
       : base(options)
        {
            this._mediator = mediator;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RoleConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TokenConfiguration).Assembly);


            #region RoleSeeding
            modelBuilder.Entity<Domain.UserAggregate.Role>().HasData(
                Domain.UserAggregate.Role.CreateRole("Client", true),
                Domain.UserAggregate.Role.CreateRole("Administrator", false)
                );
            #endregion
        }

        public override int SaveChanges()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            if (_mediator == null) return result;

            await this.DispatchEvents();

            return result;
        }

        private async Task DispatchEvents()
        {
            var entityWithRaisedEvents = ChangeTracker.Entries<BaseEntity<Guid>>().Select(e => e.Entity);
            if (entityWithRaisedEvents.Any())
            {
                var eventList = entityWithRaisedEvents.Where(e => e.DomainEvents != null && e.DomainEvents.Count > 0).ToList();
                if (eventList != null)
                {
                    foreach (var entity in eventList)
                    {
                        var raisedEvents = entity.DomainEvents.ToList();
                        entity.ClearEvents();
                        foreach (var domainEvent in raisedEvents)
                        {
                            await _mediator.Publish(domainEvent).ConfigureAwait(false);
                        }
                    }
                }

            }



        }
    }
}

