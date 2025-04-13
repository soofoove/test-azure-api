public interface IPenisRepository
{
    Task<string> GetAsync(CancellationToken cancellationToken);
}