using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

public class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public EmployeeService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
        _mapper = mapper;
    }

    public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        var company = _repositoryManager.Company.GetCompany(companyId, trackChanges: trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employee = _repositoryManager.Employee.GetEmployee(companyId, id, trackChanges: trackChanges);
        if (employee is null)
        {
            throw new EmployeeNotFoundException(id);
        }

        return _mapper.Map<EmployeeDto>(employee);
    }

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        var company = _repositoryManager.Company.GetCompany(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employees = _repositoryManager.Employee.GetEmployees(companyId, trackChanges);
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
}
