
using Microsoft.EntityFrameworkCore;

public class PenisRepository(PenisDbContext dbContext) : IPenisRepository
{
    public async Task<string> GetAsync(CancellationToken cancellationToken)
    {
        var penis = await dbContext.Penises
            .OrderBy(x => x.Id)
            .LastOrDefaultAsync(cancellationToken);
        return penis?.Value ?? "";
    }
}