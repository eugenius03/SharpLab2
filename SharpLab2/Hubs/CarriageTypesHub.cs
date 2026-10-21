using SharpLab2.Models;
using SharpLab2.Services;

namespace SharpLab2.Hubs;

public class CarriageTypesHub(ICarriageTypeService service) : CrudHub<CarriageType>(service)
{
}
