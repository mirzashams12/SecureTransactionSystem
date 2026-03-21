namespace Application.Interfaces;

public interface ICurrentUserService
{
    string? IpAddress { get; }
    Guid? UserId { get; }
    string? Email { get; }
}