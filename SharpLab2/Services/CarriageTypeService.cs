using SharpLab2.Data;
using SharpLab2.Models;

namespace SharpLab2.Services;

public class CarriageTypeService(AppDbContext context) : BaseService<CarriageType>(context), ICarriageTypeService
{
}
