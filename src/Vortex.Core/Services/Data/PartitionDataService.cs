using Microsoft.Extensions.Logging.Abstractions;
using Vortex.Core.Abstractions.Factories;
using Vortex.Core.Abstractions.Repositories.Data;
using Vortex.Core.Abstractions.Services;
using Vortex.Core.Abstractions.Services.Data;
using Vortex.Core.Models.Entities.Addresses;
using Vortex.Core.Models.Entities.Entries;

namespace Vortex.Core.Services.Data
{
    public class PartitionDataService : IPartitionDataService<byte>, IDisposable
    {
        private bool disposed = false;

        private readonly IPartitionEntryService _partitionEntryService;
        private readonly IPartitionDataFactory _partitionDataFactory;
        private readonly Address _address;
        private readonly PartitionEntry _partitionEntry;

        private readonly IPartitionDataRepository _partitionDataRepository;

        public PartitionDataService(
            IPartitionEntryService partitionEntryService,
            IPartitionDataFactory partitionDataFactory,
            Address address,
            PartitionEntry partitionEntry)
        {
            _partitionEntryService = partitionEntryService;
            _partitionDataFactory = partitionDataFactory;
            _address = address;
            _partitionEntry = partitionEntry;

            _partitionDataRepository = _partitionDataFactory
                .CreatePartitionDataRepository(address, partitionEntry);
        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _partitionDataRepository.CloseConnection();
                }

                disposed = true;
            }
        }

        public void Put(ReadOnlySpan<byte> entryId, ReadOnlySpan<byte> entity)
        {
            _partitionDataRepository
                .Put(entryId, entity);
        }

        public void PutTemporaryForDistribution(ReadOnlySpan<byte> entryId, ReadOnlySpan<byte> entity)
        {
            _partitionDataRepository
                .PutTemporary(entryId, entity);
        }

        public byte[]? Get(byte[] entryId)
        {
            var data = _partitionDataRepository.Get(entryId);
            if (data is null)
                return null;

            return _partitionDataRepository.Get(entryId);
        }

        public bool TryGet(byte[] entryId, out byte[] entity)
        {
            entity = Get(entryId)!;

            if (entity is null)
                return false;

            return true;

        }
    }
}
