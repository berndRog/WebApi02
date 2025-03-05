using Microsoft.AspNetCore.Mvc;
using WebApi.Core;
using WebApi.Core.DomainModel.Entities;
namespace WebApi.Controllers; 

[Route("carshop")]

[ApiController]
[Consumes("application/json")] //default
[Produces("application/json")] //default

public class CarsController(
   IPeopleRepository peopleRepository,
   ICarsRepository carsRepository,
   IDataContext dataContext
) : ControllerBase {
   
   [HttpPost("people/{personId:guid}/cars")]
   public ActionResult<Car> Create(
      [FromRoute] Guid personId,
      [FromBody]  Car car
   ) {
      // find person
      var person = peopleRepository.FindById(personId);
      if (person == null)
         return BadRequest("Bad request: personId doesn't exists.");

      // check if car with given Id already exists   
      if(carsRepository.FindById(car.Id) != null) 
         return Conflict("Car with given Id already exists");
      
      // add car to person in the domain model
      person.AddCar(car);
      
      // add car to repository and save to datastore
      carsRepository.Add(car); 
      dataContext.SaveAllChanges();
      
      // return created car as Dto
      var requestPath = Request?.Path ?? $"http://localhost:5200/carshop/cars/{car.Id}";
      var uri = new Uri($"{requestPath}/{car.Id}", UriKind.Relative);
      return Created(uri, car); 
   }
}