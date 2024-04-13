namespace Vortex.Core.Abstractions.Services.Data
{
    public interface IPartitionDataService<T>
    {
        void Put(ReadOnlySpan<byte> entryId, ReadOnlySpan<T> entity);
        void PutTemporaryForDistribution(ReadOnlySpan<byte> entryId, ReadOnlySpan<T> entity);

        byte[]? Get(byte[] entryId);
        bool TryGet(byte[] entryId, out byte[] entity);

        //T GetNext(long entryId);
        //bool TryGetNext(long entryId, out T entity);
        //void Delete(long entryId);
    }
}
