using Final_project.Models.General;
using Final_project.Models.GET_models;
using Final_project.Models.POST;
using Final_project.Models.QueryModels;
using System.ComponentModel.DataAnnotations;

namespace Final_project.Services
{
    public interface IDbRecordsCheckService
    {
        bool RecordExistsInDatabase<T>(T value, string tableName, string columnName);

    }
}
