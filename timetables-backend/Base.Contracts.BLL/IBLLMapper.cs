using Base.Contracts.DAL;

namespace Base.Contracts.BLL;

public interface IBLLMapper<TLeftObject, TRightObject> : IDALMapper<TLeftObject, TRightObject>
    where TLeftObject : class
    where TRightObject : class
{
    
}