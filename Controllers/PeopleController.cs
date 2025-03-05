using Microsoft.AspNetCore.Mvc;
using WebApi.Core;
using WebApi.Core.DomainModel.Entities;
namespace WebApi.Controllers;

[Route("carshop")]

[ApiController]
[Consumes("application/json")] //default
[Produces("application/json")] //default
public class PeopleController(
   IPeopleRepository peopleRepository,
   IDataContext dataContext
   //ILogger<PersonController> logger
) : ControllerBase {
   
   // Get all people 
   [HttpGet("people")]  
   public ActionResult<IEnumerable<Person>> GetAll() {
      var people = peopleRepository.SelectAll();
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
      return peopleRepository.FindById(id) switch {
         Person person => Ok(person),
         null => NotFound("Person with given id not found")
      };
   }
   
   // Get person by name
   [HttpGet("people/name")]
   public ActionResult<Person> GetByName(
      [FromQuery] string name
   ) {
      return peopleRepository.FindByName(name) switch {
         Person person => Ok(person),
         null => NotFound("Person with given name not found")
      };
   }
   
   // Get person by email
   [HttpGet("people/email")]
   public ActionResult<Person> GetByEmail(
      [FromQuery] string email
   ) {
      return peopleRepository.FindByEmail(email) switch {
         Person person => Ok(person),
         null => NotFound("Person with given EMail not found")
      };
   }
   
   // Create a new person
   [HttpPost("people")]  
   public ActionResult<Person> Create(
      [FromBody] Person person
   ) {
      // add to repository and save changes
      peopleRepository.Add(person);
      dataContext.SaveAllChanges();
      
      return Created($"/people/{person.Id}", person);
   }
   
   // Update a person 
   [HttpPut("people/{id:guid}")]
   public ActionResult<Person> Update(
      [FromRoute] Guid id,
      [FromBody]  Person updPerson
   ) {
      // find person by id
      var person = peopleRepository.FindById(id);
      if (person == null) return NotFound();
      // update domain model
      person.Update(updPerson);
      
      // update  repository and save changes
      peopleRepository.Update(person);
      dataContext.SaveAllChanges();
      
      return Ok(person);
   }

   // Delete a person   
   
   [HttpDelete("people/{id:guid}")]
   public IActionResult Delete(
      [FromRoute] Guid id
   ) {
      // find person by id
      var person = peopleRepository.FindById(id);
      if (person == null) return NotFound();
      
      // remove in repository and save changes
      peopleRepository.Remove(person);
      dataContext.SaveAllChanges();
      
      return NoContent();
   }
}