using Contracts;

namespace Repository;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _context;
    private readonly Lazy<ICompanyRepository> _companyRepository;
    private readonly Lazy<IEmployeeRepository> _employeeRepository;

    public ICompanyRepository Company => _companyRepository.Value;
    public IEmployeeRepository Employee => _employeeRepository.Value;

    public RepositoryManager(RepositoryContext context)
    {
        _context = context;

        _companyRepository = new Lazy<ICompanyRepository>(() => new CompanyRepository(_context));
        _employeeRepository = new Lazy<IEmployeeRepository>(() => new EmployeeRepository(_context));
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
