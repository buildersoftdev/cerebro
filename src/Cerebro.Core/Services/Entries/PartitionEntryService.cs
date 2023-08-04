﻿using Cerebro.Core.Abstractions.Repositories;
using Cerebro.Core.Abstractions.Services;
using Cerebro.Core.Models.Common.Addresses;
using Cerebro.Core.Models.Configurations;
using Cerebro.Core.Models.Entities.Entries;
using Cerebro.Core.Utilities.Extensions;
using Microsoft.Extensions.Logging;

namespace Cerebro.Core.Services.Entries
{
    public class PartitionEntryService : IPartitionEntryService
    {
        private readonly ILogger<PartitionEntryService> _logger;
        private readonly IPartitionEntryRepository _partitionEntryRepository;
        private readonly NodeConfiguration _nodeConfiguration;

        public PartitionEntryService(ILogger<PartitionEntryService> logger, IPartitionEntryRepository partitionEntryRepository, NodeConfiguration nodeConfiguration)
        {
            _logger = logger;
            _partitionEntryRepository = partitionEntryRepository;
            _nodeConfiguration = nodeConfiguration;
        }

        public bool CreatePartitionEntry(int addressId, string addressAlias, int partitionId, MessageIndexTypes messageIndexType, string createdBy)
        {
            var partitionEntry = _partitionEntryRepository.GetPartitionEntry(addressId, partitionId);
            if (partitionEntry != null)
                return false;

            partitionEntry = new PartitionEntry()
            {
                AddressAlias = addressAlias,
                AddressId = addressId,
                PartitionId = partitionId,
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = createdBy,
                MessageIndexType = messageIndexType,
                NodeId = _nodeConfiguration.NodeId,

                // storing will start from 1, and it will increese as 'i++'
                //TODO: Check the starting entry when we implement RocksDB Services

                CurrentEntry = 1,
                MarkDeleteEntryPosition = 0,
                Positions = new Dictionary<string, IndexPosition>()
                {
                    { DateTime.Now.GenerateAddressIndex(messageIndexType), new IndexPosition() { StartEntryPosition = 1 } }
                }
            };

            return _partitionEntryRepository.AddPartitionEntry(partitionEntry);
        }

        public List<PartitionEntry> GetPartitionEntries(int addressId)
        {
            return _partitionEntryRepository.GetPartitionEntries(addressId);
        }

        public PartitionEntry? GetPartitionEntry(int addressId, int partitionId)
        {
            return _partitionEntryRepository.GetPartitionEntry(addressId, partitionId);
        }

        public bool UpdatePartitionEntry(PartitionEntry entry)
        {
            return _partitionEntryRepository.UpdatePartitionEntry(entry);
        }
    }
}
