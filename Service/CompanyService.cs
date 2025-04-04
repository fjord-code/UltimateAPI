﻿using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
    {
        _repository = repository; 
        _logger = logger;
        _mapper = mapper;
    }

    public CompanyDto CreateCompany(CompanyForCreationDto company)
    {
        var companyEntity = _mapper.Map<Company>(company);

        _repository.Company.CreateCompany(companyEntity);
        _repository.Save();

        return _mapper.Map<CompanyDto>(companyEntity);
    }

    public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
    {
        if (companyCollection is null)
        {
            throw new CompanyCollectionBadRequest();
        }

        var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
        foreach (var companyEntity in companyEntities)
        {
            _repository.Company.CreateCompany(companyEntity);
        }

        _repository.Save();

        var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        var ids = string.Join(",", companyCollectionToReturn.Select(c => c.Id));

        return (companies: companyCollectionToReturn, ids: ids);
    }

    public void DeleteCompany(Guid companyId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        _repository.Company.DeleteCompany(company);
        _repository.Save();
    }

    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
    {
        var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges);
        var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

        return companiesDto;
    }

    public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
        {
            throw new IdParametersBadRequestException();
        }
        
        var companyEntities = _repository.Company.GetByIds(ids, trackChanges);
        if (companyEntities.Count() != ids.Count())
        {
            throw new CollectionByIdsBadRequestException();
        }

        var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
        return companiesToReturn;
    }

    public async Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        var companyDto = _mapper.Map<CompanyDto>(company);
        return companyDto;
    }

    public void UpdateCompany(Guid companyId, CompanyForUpdateDto companyForUpdate, 
        bool trackChanges)
    {
        var companyEntity = _repository.Company.GetCompany(companyId, trackChanges);

        if (companyEntity is null)
        {
            throw new CompanyNotFoundException(companyId);
        }

        _mapper.Map(companyForUpdate, companyEntity);
        _repository.Save();
    }
}
