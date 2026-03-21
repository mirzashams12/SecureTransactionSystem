using Application.Interfaces;

namespace Application.Services;

/// <summary>
/// The GuidService class implements the IGuidService interface and provides a way to generate and retrieve a GUID (Globally Unique Identifier). The constructor initializes a private readonly field _guid with a new GUID value. The GetGuid method returns this GUID value. Depending on how this service is registered in the dependency injection container (Transient, Scoped, Singleton), the behavior of the GUID generation and retrieval will differ, demonstrating the impact of service lifetimes on the state and behavior of services in an ASP.NET Core application.
/// </summary>
public class GuidService : IGuidService
{
    private readonly Guid _guid;

    /// <summary>
    /// The constructor of the GuidService class initializes the _guid field with a new GUID value using Guid.NewGuid(). This means that each instance of GuidService will have its own unique GUID value. The behavior of this GUID value will depend on how the service is registered in the dependency injection container. For example, if registered as Transient, each request for IGuidService will create a new instance of GuidService, resulting in a new GUID value each time. If registered as Scoped, the same instance (and thus the same GUID) will be used within a single HTTP request, but different requests will have different GUID values. If registered as Singleton, the same instance (and thus the same GUID) will be shared across the entire application, resulting in the same GUID value for all requests and throughout the application's lifetime.
    /// If registered as Transient, each request for IGuidService will create a new instance of GuidService, resulting in a new GUID value each time. If registered as Scoped, the same instance (and thus the same GUID) will be used within a single HTTP request, but different requests will have different GUID values. If registered as Singleton, the same instance (and thus the same GUID) will be shared across the entire application, resulting in the same GUID value for all requests and throughout the application's lifetime.
    /// If registered as Scoped, the same instance (and thus the same GUID) will be used within a single HTTP request, but different requests will have different GUID values. If registered as Singleton, the same instance (and thus the same GUID) will be shared across the entire application, resulting in the same GUID value for all requests and throughout the application's lifetime.
    /// </summary>
    public GuidService()
    {
        _guid = Guid.NewGuid();
    }

    public Guid GetGuid()
    {
        return _guid;
    }
}