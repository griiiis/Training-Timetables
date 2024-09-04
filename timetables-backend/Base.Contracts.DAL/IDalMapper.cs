namespace Base.Contracts.DAL;

public interface IDALMapper<TLeftObject, TRightObject>
    where TLeftObject : class
    where TRightObject : class
{
    TLeftObject? Map(TRightObject? inObject);
    TRightObject? Map(TLeftObject? inObject);
}