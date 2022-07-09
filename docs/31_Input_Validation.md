# Input Validation

Creating and updating objects means that the user input has to be validated.  
This can happen mostly  automated by using `data annotations`.  

An example for the `Forecast` class:

```c#
public class ForecastResponse : IAPIObject {
    public string ID { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public int TemperatureC { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    [SummaryValidation]
    [Required]
    public string Summary { get; set; }
}
```

In the above example the `Data`, `TemperatureC` and `Summary` are set as required.  
The DotNet `System.ComponentModel.DataAnnotations` contain all the basic validation attributes.  

If some more validation is required, used here for `Summary` a custom attribute can be made:

```c#
internal class SummaryValidation : ValidationAttribute {
    private Summary Value;

    public override bool IsValid(object value) {
        try {
            Value = Enum.Parse<Summary>((string)value);
            return (Summary)Value != Summary.Freezing;
        } catch (Exception) { }
        return false;
    }

    public override string FormatErrorMessage(string name) {
        return $"I will not allow {Value} for {name}!";
    }
}
```

When a user tries to Update or Create an object with `Freezing` as `Summary` it will not be allowed, because who likes freezing weather?!
