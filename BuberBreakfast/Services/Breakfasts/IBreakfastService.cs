using BuberBreakfast.Contracts;
using BuberBreakfast.Models;

namespace BuberBreakfast.Services.Breakfasts;

public interface IBreakfastService
{
    void CreateBreakfast(Breakfast breakfast);
    Breakfast GetBreakfast(Guid id);
    // BreakfastResponse UpdateBreakfast(Guid id, UpsertBreakfastRequest request);
    // BreakfastResponse DeleteBreakfast(Guid id);
}