using Microsoft.AspNetCore.Mvc;
using WebApi.Core;
using WebApi.Core.DomainModel.Entities;
namespace WebApi.Controllers;

[Route("carshop")]

[ApiController]
[Consumes("application/json")] //default
[Produces("application/json")] //default
public class PersonController(
   IPersonRepository personRepository,
   IDataContext dataContext
   //ILogger<PersonController> logger
) : ControllerBase {
   
   // Get all people 
   [HttpGet("people")]  
   public ActionResult<IEnumerable<Person>> GetAll() {
      var people = personRepository.SelectAll();
      return Ok(people);
   }
   
   // Get person by Id
   [HttpGet("people/{id:guid}")]
   public ActionResult<Person> GetById(
      [FromRoute] Guid id
   ) {
      // switch(personRepository.FindById(id)) {
      //    case Person person:
      //       return Ok(person);
      //    case null:
      //       return NotFound("Owner with given Id not found");
      // };
      return personRepository.FindById(id) switch {
         Person person => Ok(person),
         null => NotFound("Person with given id not found")
      };
   }
   
   // Get person by name
   [HttpGet("people/name")]
   public ActionResult<Person> GetByName(
      [FromQuery] string name
   ) {
      return personRepository.FindByName(name) switch {
         Person person => Ok(person),
         null => NotFound("Person with given name not found")
      };
   }
   
   // Get person by email
   [HttpGet("people/email")]
   public ActionResult<Person> GetByEmail(
      [FromQuery] string email
   ) {
      return personRepository.FindByEmail(email) switch {
         Person person => Ok(person),
         null => NotFound("Person with given EMail not found")
      };
   }
   
   // Create a new person
   [HttpPost("people")]  
   public ActionResult<Person> Create(
      [FromBody] Person person
   ) {
      // apply changes and save
      personRepository.Add(person);
      dataContext.SaveChanges();
      
      return Created($"/people/{person.Id}", person);
   }
   
   // Update a person 
   [HttpPut("people/{id}")]
   public ActionResult<Person> Update(
      Guid id,
      [FromBody] Person updPerson
   ) {
      var person = personRepository.FindById(id);
      if (person == null) return NotFound();
      
      // update domain model
      person.Update(updPerson);
      
      // apply changes and save
      personRepository.Update(person);
      dataContext.SaveChanges();
      
      return Ok(person);
   }

   // Delete a person   http://localhost:5100/people/{id}
   [HttpDelete("people/{id}")]
   public IActionResult Delete(Guid id) {
      // find person by id
      var person = personRepository.FindById(id);
      if (person == null) return NotFound();
      
      // apply changes and save
      personRepository.Remove(person);
      dataContext.SaveChanges();
      
      return NoContent();
   }
}