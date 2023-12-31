﻿using System.Linq.Expressions;
using App.DAL.Contracts;
using App.DAL.Contracts.Repositories.EntityRepositories.Identity;
using App.DAL.DTO.Entities.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.EntityRepositories.Identity;

public class RefreshTokenRepository : BaseAppEntityRepository<App.Domain.Identity.RefreshToken, RefreshToken>,
    IRefreshTokenRepository
{
    public RefreshTokenRepository(AbstractAppDbContext dbContext, IMapper mapper, IAppUnitOfWork uow) : base(dbContext, mapper, uow)
    {
    }

    protected override Domain.Identity.RefreshToken AfterMap(RefreshToken entity, Domain.Identity.RefreshToken mapped)
    {
        if (entity.User != null)
        {
            mapped.User = Uow.Users.GetTrackedEntity(entity.User.Id);
        }
        return mapped;
    }

    public async Task<ICollection<RefreshToken>> GetAllByUserIdAsync(Guid userId,
        params Expression<Func<App.Domain.Identity.RefreshToken, bool>>[] filters)
    {
        var newFilters = new List<Expression<Func<App.Domain.Identity.RefreshToken, bool>>>();
        newFilters.Add(rt => rt.UserId == userId);
        newFilters.AddRange(filters);
        return await GetAllAsync(newFilters.ToArray());
    }

    public async Task<ICollection<RefreshToken>> GetAllFullyExpiredByUserIdAsync(Guid userId)
    {
        return (await GetAllByUserIdAsync(userId)).Where(r => r.IsFullyExpired).ToList();
    }

    public async Task<ICollection<RefreshToken>> GetAllValidByUserIdAndRefreshTokenAsync(Guid userId,
        string refreshToken)
    {
        return await GetAllByUserIdAsync(userId, r =>
            (r.RefreshToken == refreshToken && r.ExpiresAt > DateTime.UtcNow) ||
            (r.PreviousRefreshToken == refreshToken && r.PreviousExpiresAt > DateTime.UtcNow));
    }

    public async Task ExecuteDeleteUserRefreshTokensAsync(Guid userId, string refreshToken)
    {
        await Entities
            .Where(r => r.UserId == userId &&
                        r.RefreshToken == refreshToken || r.PreviousRefreshToken == refreshToken)
            .ExecuteDeleteAsync();
    }
}