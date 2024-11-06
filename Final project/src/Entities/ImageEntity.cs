using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AndreiKorbut.CareerChoiceBackend.Entities
{
    public class ImageEntity
    {
        public int Id { get; set; }

        public string Base64ImageData { get; set; }

        public virtual CareerEntity Career { get; set; }

        public virtual CategoryEntity Category { get; set; }
    }
}
