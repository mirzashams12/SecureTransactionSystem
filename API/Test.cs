
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly IGuidService _guidService1;
    private readonly IGuidService _guidService2;

    public TestController(IGuidService guidService1, IGuidService guidService2)
    {
        _guidService1 = guidService1;
        _guidService2 = guidService2;
    }

    /// <summary>
    /// This method demonstrates the behavior of the IGuidService when injected with different lifetimes (Transient, Scoped, Singleton). Depending on how the service is registered in the dependency injection container, the GUID values returned by _guidService1 and _guidService2 will differ. For example, if registered as Transient, each call will return a new GUID; if registered as Scoped, both will return the same GUID within the same request but different across requests; if registered as Singleton, both will always return the same GUID regardless of the request. This illustrates how service lifetimes affect the state and behavior of services in an ASP.NET Core application.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(new { Guid1 = _guidService1.GetGuid(), Guid2 = _guidService2.GetGuid() });
    }

    /// <summary>
    /// This method simulates an asynchronous operation, which is a common pattern in web applications to improve responsiveness and scalability. It uses Task.Delay to mimic a time-consuming operation, allowing the thread to be freed up to handle other requests while waiting for the operation to complete.
    /// </summary>
    /// <returns></returns>
    [HttpGet("async")]
    public async Task<IActionResult> TestAsync()
    {
        // Simulate an asynchronous operation
        // the await keyword allows the method to asynchronously wait for the Task.Delay to complete without blocking the thread, enabling better performance and responsiveness in the application.
        await Task.Delay(2000); //await for 2 seconds
        return Ok("Data retrieved successfully!");
    }

    /// <summary>
    /// This method simulates a blocking operation, which can lead to performance issues in a web application. It will block the thread for 2 seconds, preventing it from handling other requests during that time.
    /// </summary>
    /// <returns></returns>
    [HttpGet("blocked")]
    public IActionResult BlockingTest()
    {
        // Simulate a blocking operation
        // Thread vs Task: Thread.Sleep will block the current thread for the specified duration, which can lead to performance issues in a web application as it prevents the thread from handling other requests. 
        // In contrast, Task.Delay allows the thread to be freed up while waiting, improving responsiveness and scalability.
        Thread.Sleep(2000);
        return Ok("Data retrieved successfully!");
    }
}