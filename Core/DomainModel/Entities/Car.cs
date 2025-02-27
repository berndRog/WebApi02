namespace WebApi.Core.DomainModel.Entities;

public class Car: AEntity {
   
   // properties with getter only
   public override Guid Id { get; init; } = Guid.NewGuid();
   public string Maker {get; private set;} = string.Empty;
   public string Model {get; private set;} = string.Empty;
   public int Year {get; private set;} = 1900;
   public double Price {get; private set;} = 0.0;
   public string? ImageUrl { get; private set; } = null;
   // navigation property
   public Guid? PersonId { get; private set; } = null;
   public Person? Person { get; private set; } = null;

   // EF Core requires a constructor
   public Car(Guid id, string maker, string model, int year, double price, string? imageUrl = null) {
      Id = id;
      Maker = maker;
      Model = model;
      Year = year;
      Price = price;
      ImageUrl = imageUrl;
   }
   public Car(Guid id, string maker, string model, int year, double price, string? imageUrl, Guid? personId) {
      Id = id;
      Maker = maker;
      Model = model;
      Year = year;
      Price = price;
      ImageUrl = imageUrl;
      Person = null; 
      PersonId = personId;
   }
   
   public void SetImageUrl(string imageUrl) =>
      ImageUrl = imageUrl;

   public void Set(Person? person) {
      if (person != null) {
         PersonId = person.Id;
         Person = person;
      }
      else {
         PersonId = null;
         Person = null;
      }
   }
   
   public void Update(Car updCar) {
      Maker = updCar.Maker;  // can the car maker be updated?
      Model = updCar.Model;  // can the car model be updated?
      Year = updCar.Year;    // can the car year be updated?
      Price = updCar.Price;
   }
   
}