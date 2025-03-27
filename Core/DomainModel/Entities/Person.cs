using System.Text.Json.Serialization;
namespace WebApi.Core.DomainModel.Entities; 

public class Person: AEntity {

   // properties with getter only
   public override Guid Id { get; init; } = Guid.NewGuid();
   [JsonInclude]
   public string FirstName { get; set; } = string.Empty;
   [JsonInclude]
   public string LastName { get; private set; } = string.Empty;
   [JsonInclude]
   public string? Email { get; private set; } = null;
   [JsonInclude]
   public string? Phone { get; private set; } = null;
   // navigation property Person -> Car [0..*]
   [JsonInclude]
   public ICollection<Car> Cars { get; private set; } = [];
   
   // ctor
   public Person() { }
   public Person(Guid id, string firstName, string lastName, string? email = null,
      string? phone = null) {
      Id = id;
      FirstName= firstName;
      LastName = lastName;
      Email = email;
      Phone = phone;
   }   
   
   // methods
   public void Set(string? email = null, string? phone = null) {
      if(email != null) Email = email;
      if(phone != null) Phone = phone;
   } 
   
   public void Update (Person updPerson) {
      FirstName = updPerson.FirstName;
      LastName = updPerson.LastName;
      if(updPerson.Email != null) Email = updPerson.Email;
      if(updPerson.Phone != null) Phone = updPerson.Phone;
   }
   
   // methods car
   public void AddCar(Car car) {
      car.Set(this);
      Cars.Add(car);
   }
   public void RemoveCar(Car car) {
      car.Set(null);
      Cars.Remove(car);
   }
}