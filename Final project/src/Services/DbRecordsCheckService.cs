using CareerChoiceBackend.Entities;
using CareerChoiceBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AndreiKorbut.CareerChoiceBackend.Services
{
    public class DbRecordsCheckService : IDbRecordsCheckService
    {
        private readonly CareerContext _dbContext;

        public DbRecordsCheckService(CareerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool RecordExistsInDatabase<T>(T value, string tableName, string columnName)
        {
            var dbSetProperty = _dbContext.GetType().GetProperty(tableName);
            if (dbSetProperty == null)
            {
                throw new ArgumentException($"DbSet for table {tableName} not found in DbContext.");
            }

            var dbSet = dbSetProperty.GetValue(_dbContext) as IQueryable<object>;
            if (dbSet == null)
            {
                throw new InvalidOperationException($"DbSet property {tableName} is not of type IQueryable<object>.");
            }

            var elementType = dbSet.ElementType;

            var property = elementType.GetProperty(columnName);
            if (property == null)
            {
                throw new ArgumentException($"Property {columnName} not found in table {tableName}.");
            }

            var parameter = Expression.Parameter(elementType, "e");
            var propertyAccess = Expression.Property(parameter, property);

            try
            {
                var convertedValue = Expression.Constant(Convert.ChangeType(value, property.PropertyType));

                var equality = Expression.Equal(propertyAccess, convertedValue);
                var lambda = Expression.Lambda(equality, parameter);

                var anyMethod = typeof(Queryable).GetMethods()
                                                  .First(method => method.Name == "Any" && method.GetParameters().Length == 2)
                                                  .MakeGenericMethod(elementType);
                var result = (bool)anyMethod.Invoke(null, new object[] { dbSet, lambda });
                
                return result;
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException($"Value '{value}' cannot be converted to the type of property '{columnName}'.");
            }
        }
    }
}
