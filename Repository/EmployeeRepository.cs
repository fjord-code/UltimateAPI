﻿using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext context)
        : base(context)
    { 
    }

    public void CreateEmployeeForCompany(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployee(Employee employee)
    {
        Delete(employee);
    }

    public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        return await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges: trackChanges)
            .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        return await FindByCondition(e => e.CompanyId == companyId, trackChanges)
            .OrderBy(e => e.Name)
            .ToListAsync();
    }
}
