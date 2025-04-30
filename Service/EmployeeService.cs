using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
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

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

        _repositoryManager.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await _repositoryManager.SaveAsync();

        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
        return employeeToReturn;
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employeeForCompany = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        if (employeeForCompany is null)
        {
            throw new EmployeeNotFoundException(id);
        }

        _repositoryManager.Employee.DeleteEmployee(employeeForCompany);
        await _repositoryManager.SaveAsync();
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges: trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employee = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges: trackChanges);
        if (employee is null)
        {
            throw new EmployeeNotFoundException(id);
        }

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(
        Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, compTrackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employeeEntity = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, empTrackChanges);
        if (employeeEntity is null)
        {
            throw new EmployeeNotFoundException(id);
        }

        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

        return (employeeToPatch, employeeEntity);
    }

    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employees = await _repositoryManager.Employee.GetEmployeesAsync(companyId, trackChanges);
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        _mapper.Map(employeeToPatch, employeeEntity);
        await _repositoryManager.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, 
        EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, compTrackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var employeeEntity = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, 
            empTrackChanges);
        if (employeeEntity is null)
        {
            throw new EmployeeNotFoundException(id);
        }

        _mapper.Map(employeeForUpdate, employeeEntity);
        await _repositoryManager.SaveAsync();
    }
}
