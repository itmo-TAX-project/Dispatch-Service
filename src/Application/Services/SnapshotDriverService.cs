using Application.DTO;
using Application.DTO.Enums;
using Application.Repositories;
using Application.Services.Interfaces;
using System.Transactions;

namespace Application.Services;

public class SnapshotDriverService : ISnapshotDriverService
{
    private readonly IDriverSnapshotRepository _driverSnapshotRepository;

    public SnapshotDriverService(IDriverSnapshotRepository driverSnapshotRepository)
    {
        _driverSnapshotRepository = driverSnapshotRepository;
    }

    public async Task AddDriverSnapshotAsync(DriverSnapshotDto dto, CancellationToken cancellationToken)
    {
        TransactionScope transaction = CreateTransactionScope();

        DriverSnapshotDto? isDriverSnapshotExists = await _driverSnapshotRepository.GetDriverSnapshotAsync(
            dto.DriverId,
            cancellationToken);

        if (isDriverSnapshotExists != null)
        {
            await UpdateDriverSnapshotAsync(dto, cancellationToken);
        }

        await _driverSnapshotRepository.CreateDriverSnapshotAsync(dto, cancellationToken);

        transaction.Complete();
        transaction.Dispose();
    }

    public async Task UpdateDriverSnapshotAsync(DriverSnapshotDto dto, CancellationToken cancellationToken)
    {
        TransactionScope transaction = CreateTransactionScope();

        DriverSnapshotDto? isDriverSnapshotExists = await _driverSnapshotRepository.GetDriverSnapshotAsync(
            dto.DriverId,
            cancellationToken);

        if (isDriverSnapshotExists == null)
        {
            throw new Exception("DriverSnapshot not found");
        }

        await _driverSnapshotRepository.UpdateDriverSnapshotAsync(dto, cancellationToken);

        transaction.Complete();
        transaction.Dispose();
    }

    public async Task SetCurrentVehicleSegmentAsync(long driverId, VehicleSegment segment, CancellationToken cancellationToken)
    {
        TransactionScope transaction = CreateTransactionScope();

        DriverSnapshotDto? isDriverSnapshotExists = await _driverSnapshotRepository.GetDriverSnapshotAsync(
            driverId,
            cancellationToken);

        if (isDriverSnapshotExists == null)
        {
            throw new Exception("DriverSnapshot not found");
        }

        await _driverSnapshotRepository.SetCurrentVehicleSegmentAsync(driverId, segment, cancellationToken);

        transaction.Complete();
        transaction.Dispose();
    }

    public async Task DeleteDriverSnapshotAsync(long driverId, CancellationToken cancellationToken)
    {
        TransactionScope transaction = CreateTransactionScope();

        DriverSnapshotDto? isDriverSnapshotExists = await _driverSnapshotRepository.GetDriverSnapshotAsync(
            driverId,
            cancellationToken);

        if (isDriverSnapshotExists == null)
        {
            throw new Exception("DriverSnapshot not found");
        }

        await _driverSnapshotRepository.DeleteDriverSnapshotAsync(driverId, cancellationToken);

        transaction.Complete();
        transaction.Dispose();
    }

    private static TransactionScope CreateTransactionScope()
    {
        return new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled);
    }
}