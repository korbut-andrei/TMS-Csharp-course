using AndreiKorbut.CareerChoiceBackend.Models.General;
using AndreiKorbut.CareerChoiceBackend.Models.GET_models;
using AndreiKorbut.CareerChoiceBackend.Models.POST;
using AndreiKorbut.CareerChoiceBackend.Models.QueryModels;
using System.ComponentModel.DataAnnotations;

namespace AndreiKorbut.CareerChoiceBackend.Services
{
    public interface IDbRecordsCheckService
    {
        bool RecordExistsInDatabase<T>(T value, string tableName, string columnName);

    }
}
