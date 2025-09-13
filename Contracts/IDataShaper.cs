using Entities.Models;

namespace Contracts
{
    public interface IDataShaper<T> where T : class
    {
        IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString);
        ShapedEntity ShapeData(T entity, string fieldsString);
    }
}
