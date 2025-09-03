using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

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

    private async Task CheckIfCompanyExistsAsync(Guid companyId, bool trackChanges)
    {
        var company = await _repositoryManager.Company.GetCompanyAsync(companyId, trackChanges);

        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }
    }

    private async Task<Employee> GetEmployeeForCompanyAndCheckIfItExistsAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var employeeDb = await _repositoryManager.Employee.GetEmployeeAsync(companyId, id, trackChanges);

        if (employeeDb is null)
        {
            throw new EmployeeNotFoundException(id);
        }

        return employeeDb;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
    {
        await CheckIfCompanyExistsAsync(companyId, trackChanges);

        var employeeEntity = _mapper.Map<Employee>(employeeForCreation);

        _repositoryManager.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await _repositoryManager.SaveAsync();

        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);
        return employeeToReturn;
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
    {
        await CheckIfCompanyExistsAsync(companyId, trackChanges);
        var employeeForCompany = await GetEmployeeForCompanyAndCheckIfItExistsAsync(companyId, id, trackChanges);

        _repositoryManager.Employee.DeleteEmployee(employeeForCompany);
        await _repositoryManager.SaveAsync();
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        await CheckIfCompanyExistsAsync(companyId, trackChanges);
        var employee = await GetEmployeeForCompanyAndCheckIfItExistsAsync(companyId, id, trackChanges);

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(
        Guid companyId, Guid id, bool compTrackChanges, bool empTrackChanges)
    {
        await CheckIfCompanyExistsAsync(companyId, compTrackChanges);
        var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExistsAsync(companyId, id, empTrackChanges);

        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

        return (employeeToPatch, employeeEntity);
    }

    public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        await CheckIfCompanyExistsAsync(companyId, trackChanges);

        var employeesWithMetaData = await _repositoryManager.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);

        return (employees: employeesDto, metaData: employeesWithMetaData.MetaData);
    }

    public async Task SaveChangesForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)
    {
        _mapper.Map(employeeToPatch, employeeEntity);
        await _repositoryManager.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid id, 
        EmployeeForUpdateDto employeeForUpdate, bool compTrackChanges, bool empTrackChanges)
    {
        await CheckIfCompanyExistsAsync(companyId, compTrackChanges);
        var employeeEntity = await GetEmployeeForCompanyAndCheckIfItExistsAsync(companyId, id, empTrackChanges);

        if (employeeEntity is null)
        {
            throw new EmployeeNotFoundException(id);
        }

        _mapper.Map(employeeForUpdate, employeeEntity);
        await _repositoryManager.SaveAsync();
    }
}
