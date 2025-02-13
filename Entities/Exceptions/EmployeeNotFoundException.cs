namespace Entities.Exceptions;

public class EmployeeNotFoundException : NotFoundException
{
    public EmployeeNotFoundException(Guid id)
        : base($"Employee with the id: {id} doesn't exist in the database.")
    {

    }

}
