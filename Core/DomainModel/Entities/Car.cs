using System.Text.Json.Serialization;
using WebApi.Core.DomainModel.NullEntities;
namespace WebApi.Core.DomainModel.Entities;

public class Car: AEntity {
   
   // properties with getter only
   public override Guid Id { get; init; } = Guid.NewGuid();
   [JsonInclude]
   public string Maker {get; private set;} = string.Empty;
   [JsonInclude]
   public string Model {get; private set;} = string.Empty;
   [JsonInclude]
   public int Year {get; private set;} = 1900;
   [JsonInclude]
   public double Price {get; private set;} = 0.0;
   [JsonInclude]
   public string? ImageUrl { get; private set; } = null;
   // navigation property
   [JsonInclude]
   public Guid PersonId { get; private set; } = NullPerson.Instance.Id;
   [JsonInclude]
   public Person Person { get; private set; } = NullPerson.Instance;

   // EF Core requires a constructor
   internal Car() { }  // for subclasses only
   public Car(Guid id, string maker, string model, int year, double price, string? imageUrl = null) {
      Id = id;
      Maker = maker;
      Model = model;
      Year = year;
      Price = price;
      ImageUrl = imageUrl;
   }
  
   // methods
   public void SetImageUrl(string imageUrl) =>
      ImageUrl = imageUrl;

   public void Set(Person? person) {
      if (person != null) {
         PersonId = person.Id;
         Person = person;
      }
      else {
         PersonId = NullPerson.Instance.Id;
         Person = NullPerson.Instance;
      }
   }
   
   public void Update(Car updCar) {
      Maker = updCar.Maker;  // can the car maker be updated?
      Model = updCar.Model;  // can the car model be updated?
      Year = updCar.Year;    // can the car year be updated?
      Price = updCar.Price;
   }
   
}