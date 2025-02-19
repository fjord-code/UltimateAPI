using Contracts;
using Entities.Models;

namespace Repository;

internal sealed class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext context)
        : base(context)
    { 
    }

    public void CreateCompany(Company company)
    {
        Create(company);
    }

    public IEnumerable<Company> GetAllCompanies(bool trackChanges)
    {
        return FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToList();
    }

    public Company? GetCompany(Guid companyId, bool trackChanges)
    {
        return FindByCondition(c => c.Id.Equals(companyId), trackChanges)
            .SingleOrDefault();
    }
}
