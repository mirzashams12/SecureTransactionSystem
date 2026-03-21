namespace Application.Interfaces;

/// <summary>
/// The IGuidService interface defines a contract for a service that provides a method to retrieve a GUID (Globally Unique Identifier). The GetGuid method is expected to return a GUID value, and the behavior of this service will depend on how it is implemented and registered in the dependency injection container. For example, if implemented by the GuidService class and registered with different lifetimes (Transient, Scoped, Singleton), the GUID values returned by GetGuid will differ based on the lifetime of the service, demonstrating the impact of service lifetimes on the state and behavior of services in an ASP.NET Core application.
/// </summary>
public interface IGuidService
{
    Guid GetGuid();
}