using Application.DTO;

namespace Application.Services.Interfaces;

public interface IDispatchService
{
    Task AssignDriver(RideAssignationDto dto, CancellationToken cancellationToken);
}