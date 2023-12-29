using Lesson_17_Entity_Framework.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lesson_17_Entity_Framework.Models
{
    public class CareerEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string ProfessionName { get; set; }
        public string Description { get; set; }
        public CategoryEntity Category { get; set; }
        public string ImgURL  { get; set; }
    }


    public class CategoryEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public string ImgURL { get; set; }

        public int CareerEntityId { get; set; }
        public virtual CareerEntity CareerEntity { get; set; }


    }


    public class CategoryQuery
    {
        public int CategoryName { get; set; }
    }


    public class CareerGroupList
    {
        public CareerListModel[] CareerList { get; set; }
    }




    public class SearchParameters
    {
        public string Query { get; set; }
    }



    public class CareersEducationTimeRange
    {
        public int EducationTimeMin { get; set; }
        public int EducationTimeMax { get; set; }
    }


    public class CareerDetailsModel
    {
        public string ProfessionName { get; set; }
        public string Description { get; set; }
        public SalaryRange SalaryRange { get; set; }
        public CategoryName Category { get; set; }
        public string ImgURL  { get; set; }
        public TypicalTaskList[] TypicalTasks { get; set; }
        public CareerCharacteristicEntity[] CareerCharacteristics { get; set; }

        public ReviewEntity[] Reviews { get; set; }
    
        public SalaryReportEntity[] SalaryStatistics { get; set; }

        public AverageReviewAndReviewCount AverageReviewAndReviewCount { get; set; }
    }

    public class CategoryName
    {
        public string Name { get; set; }
    }


    public class CareerList
    {
        public CareerListModel[] Careers { get; set; }
    }

    public class CareerListModel
    {
        public string CareerName { get; set; }
        public SalaryRange SalaryRange { get; set; }
        public CategoryEntity Category { get; set; }
        public string ImgURL  { get; set; }
        public AverageReviewAndReviewCount AverageReviewAndReviewCount  { get; set; }
    }

    public class AverageReviewAndReviewCount
    {
        public float AverageReview { get; set; }
        public int ReviewCount { get; set; }
    }

    public class SalaryRange
    {
        public int SalaryMin { get; set; }
        public int SalaryMax { get; set; }
    }

    public class TypicalTaskList
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class TypicalTaskEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int CareerId { get; set; }
        public virtual CareerEntity CareerEntity { get; set; }
    }

    public class CareerCharacteristicEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Group { get; set; }

        public int AverageRating { get; set; }

        public int CareerEntityId { get; set; }
        public virtual CareerEntity CareerEntity { get; set; }
    }



    public class CareerCharacteristicReviewEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Text { get; set; }

        public string Rating { get; set; }

        public int CareerCharacteristicId { get; set; }
        public virtual CareerCharacteristicEntity CareerCharacteristicEntity { get; set; }

        public int UserId { get; set; }
        public virtual UserEntity UserEntity { get; set; }
    }


    public class CareerCharacteristicViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Group { get; set; }

        public int AverageRating { get; set; }

        public int CareerEntityId { get; set; }
        public virtual CareerEntity CareerEntity { get; set; }
    }



    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //...
    }



     public class ReviewEntity
     {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Text { get; set; }

        public string Rating { get; set; }

        public int UserId { get; set; }
        public virtual UserEntity UserEntity { get; set; }

        public virtual ICollection<string> PositiveBulletPoints { get; set; }

        public virtual ICollection<string> NegativeBulletPoints { get; set; }

        // Foreign key to CareerEntity
        public int CareerEntityId { get; set; }
        public virtual CareerEntity CareerEntity { get; set; }
     }


    public class SalaryReportList
    {
        public SalaryReportEntity[] Reports { get; set; }
    }

    public class SalaryRangeResult
    {
        public string ExperienceYears { get; set; }
        public int MedianSalary { get; set; }
        public int MinSalary { get; set; }
        public int MaxSalary { get; set; }
    }

    public class SalaryReportEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int Salary { get; set; }
        public string ExperienceYears { get; set; }

        public int CareerEntityId { get; set; }
        public virtual CareerEntity CareerEntity { get; set; }
    }



    public class CategoryWithCareers
    {
        public string Name { get; set; }

        public string ImgURL { get; set; }
        public List<CareerEntity> Careers { get; set; }
    }


    public class SearchResult
    {
        public List<CareerEntity> MatchedCareers { get; set; }
        public List<CategoryEntity> MatchedCategories { get; set; }
    }

    public class CareersEducationTimeRange
    {

        public int EducationTimeMin { get; set; }
        public int EducationTimeMax { get; set; }
    }

    public class SearchCategory
    {
        public int CategoryName { get; set; }
    }

    public class CareerGroupList
    {
        public CareerListModel[] CareerList { get; set; }
    }
}





