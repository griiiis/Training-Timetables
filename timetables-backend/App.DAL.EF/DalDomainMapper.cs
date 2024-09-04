using App.DAL.EF.Repositories;
using App.Domain;
using AutoMapper;
using Base.Contracts.DAL;
using Base.Contracts.Domain;

namespace App.DAL.EF;

public class DalDomainMapper<TLeftObject, TRightObject> : IDALMapper<TLeftObject, TRightObject> 
    where TLeftObject : class 
    where TRightObject : class
{
    public readonly IMapper _mapper;

    public DalDomainMapper(IMapper mapper)
    {
        _mapper = mapper;
    }
    public TLeftObject? Map(TRightObject? inObject)
    {
        return _mapper.Map<TLeftObject>(inObject);
    }

    public TRightObject? Map(TLeftObject? inObject)
    {
        return _mapper.Map<TRightObject>(inObject);
    }
}