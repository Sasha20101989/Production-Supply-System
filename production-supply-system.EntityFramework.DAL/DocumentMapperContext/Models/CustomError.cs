namespace production_supply_system.EntityFramework.DAL.DocumentMapperContext.Models;

public class CustomError
{
    public string? ErrorMessage { get; set; }

    public int Row { get; set; }

    public int Column { get; set; }
}
