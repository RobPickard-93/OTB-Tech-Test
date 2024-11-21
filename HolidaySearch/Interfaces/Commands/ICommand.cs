using HolidaySearch.Models;

namespace HolidaySearch.Interfaces.Commands
{
    public interface ICommand<TRequest, TResponse>
    {
        Task<Result<TResponse>> Execute(TRequest request);
    }
}
