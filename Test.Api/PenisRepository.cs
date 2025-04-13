
public class PenisRepository : IPenisRepository
{
    public Task<string> GetAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult("zhopa 1");
    }
}