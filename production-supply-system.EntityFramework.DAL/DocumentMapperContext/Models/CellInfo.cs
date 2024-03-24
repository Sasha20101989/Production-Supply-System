namespace production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;

public class CellInfo
{
    public object? Value { get; set; }

    public bool HasError => Errors is not null && Errors.Count > 0;

    public List<CustomError> Errors { get; set; } = [];
}
