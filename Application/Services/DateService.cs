using Application.Interfaces;

namespace Application.Services;

public class DateService : IDataService
{

    private readonly DateTime _currentDate;
    public DateService()
    {
        _currentDate = DateTime.UtcNow;
    }
    public DateTime GetCurrentDate()
    {
        return _currentDate;
    }
}