
using Microsoft.EntityFrameworkCore;

public class PenisRepository(PenisDbContext dbContext) : IPenisRepository
{
    public async Task<string> GetAsync(CancellationToken cancellationToken)
    {
        var penis = await dbContext.Penises.LastAsync(cancellationToken);
        return penis.Value;
    }
}