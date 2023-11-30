using Lesson_17_Entity_Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lesson_17_Entity_Framework.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionsController : ControllerBase
    {
        private readonly ProfessionsContext professionsContext;

        public ProfessionsController(ProfessionsContext professionsContext)
        {
            this.professionsContext = professionsContext;
        }

        [HttpGet]
        [Route("GetProfessions")]
        public List<Professions> GetProfessions()
        {
            return professionsContext.Professions.ToList();
        }


        [HttpGet]
        [Route("GetProfession")]
        public Professions GetProfession(int id)
        {
            return professionsContext.Professions.Where(x => x.Id == id).FirstOrDefault();
        }

        [HttpPost]
        [Route("AddProfession")]
        public string AddProfession(Professions profession)
        {
            string response = string.Empty;

            professionsContext.Professions.Add(profession);

            professionsContext.SaveChanges();

            return "Profession successfully added";
        }

        [HttpPut]
        [Route("UpdateProfession")]
        public string UpdateProfession(Professions profession)
        {
            professionsContext.Entry(profession).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            professionsContext.SaveChanges();

            return "User has been successfully updated.";
        }

        [HttpDelete]
        [Route("DeleteProfession/{id}")]
        public string DeleteProfession(int id)
        {
            // Ensure that only the ID is passed for deletion
            if (ModelState.IsValid)
            {
                // Retrieve the profession by ID
                Professions professionToDelete = professionsContext.Professions.Find(id);

                // Check if the profession exists
                if (professionToDelete != null)
                {
                    // Remove the profession
                    professionsContext.Professions.Remove(professionToDelete);
                    professionsContext.SaveChanges();

                    return "Profession has been successfully deleted.";
                }
                else
                {
                    return "Profession not found.";
                }
            }
            else
            {
                return "Invalid model state. Please provide a valid ID.";
            }
        }


    }
}
