using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebApi.Core;
using WebApi.Core.DomainModel.Entities;
namespace WebApi.Data;

public class DataContext: IDataContext {
   // fake storage with JSON file
   private readonly string _filePath = string.Empty;
   private JsonSerializerOptions _jsonOptions = new JsonSerializerOptions {
      PropertyNameCaseInsensitive = true,
      //ReferenceHandler = ReferenceHandler.Preserve,
      ReferenceHandler = ReferenceHandler.IgnoreCycles,
      WriteIndented = true
   };
   private readonly ILogger<DataContext> _logger;

   public ICollection<Person> People { get; } = [];
   public ICollection<Car> Cars { get; } = [];

   private class CombinedCollections {
      public ICollection<Person> People { get; init; } = [];
      public ICollection<Car> Cars { get; init; } = [];
   }

   public DataContext(
      ILogger<DataContext> logger
   ) {
      _logger = logger;
      
      try {
         // Create the directory if it does not exist /Users/rogallab/Webtech/WebApps/WebApp02
         var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
         var directory = Path.Combine(homeDirectory, "Webtech/WebApis/WebApi02");
         if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
         
         // Create the file path
         _filePath = Path.Combine(directory, "people&cars.json");
         _logger.LogInformation("File path: {_filePath}", _filePath);
         
         // Create an Empty JSON file
         if (!File.Exists(_filePath)) {
            var emptyCollections = new {
               People = new Collection<Person>(),
               Cars = new Collection<Car>()
            };
            var emptyJson = JsonSerializer.Serialize(
               emptyCollections,
               _jsonOptions
            );
            File.WriteAllText(_filePath, emptyJson, Encoding.UTF8);
         }
         // Read the JSON file
         var json = File.ReadAllText(_filePath, Encoding.UTF8);
         // Prettify the JSON for logging
         var prettyJson = JsonSerializer.Serialize(
            value: JsonSerializer.Deserialize<JsonDocument>(json), 
            options: new JsonSerializerOptions { 
               PropertyNameCaseInsensitive = true,
               WriteIndented = true 
            }
         );
         _logger.LogInformation("Deserialize: {json}", prettyJson);
         // Deserialize the JSON file
         var combinedCollections = JsonSerializer.Deserialize<CombinedCollections>(
            json,
            _jsonOptions
         ) ?? throw new ApplicationException("Deserialization failed");

         People = combinedCollections.People;
         Cars = combinedCollections.Cars;
      }
      catch (Exception e) {
         Console.WriteLine(e.Message);
      }
   }

   public void SaveAllChanges() {
      try {
         var combinedCollections = new {
            People = People, Cars = Cars
         };
         var json = JsonSerializer.Serialize(
            combinedCollections,
            _jsonOptions
         );
         _logger.LogInformation("Serialize: {json}", json);
         File.WriteAllText(_filePath, json, Encoding.UTF8);
      }
      catch (Exception e) {
         Console.WriteLine(e.Message);
         throw; // Re-throw the exception
      }
   }
}