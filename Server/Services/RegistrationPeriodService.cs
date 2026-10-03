using Microsoft.EntityFrameworkCore;
using Server.Data;
using Shared;

namespace Server.Services;

public class RegistrationPeriodService
{
    private readonly AppDbContext _context;

    public RegistrationPeriodService(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// The window currently in effect: the most recently added record.
    /// Returns null when no window has been configured.
    /// </summary>
    public Task<RegistrationPeriod?> GetCurrentAsync() =>
        _context.RegistrationPeriods
            .AsNoTracking()
            .OrderByDescending(rp => rp.Id)
            .FirstOrDefaultAsync();

    /// <summary>
    /// Whether registration is open right now, per the server clock.
    /// Fails closed: no configured window means registration is shut.
    /// </summary>
    public async Task<bool> IsOpenNowAsync()
    {
        var period = await GetCurrentAsync();
        return period?.IsOpen ?? false;
    }
}
