using Planner.Domain.Exceptions;

public class Address
{
    public string? Street { get; }
    public string? Number { get; }
    public string? Complement { get; }
    public string? City { get; }
    public string? State { get; }
    public string? ZipCode { get; }
    public string? Country { get; }

    public Address(
        string street = null,
        string number = null,
        string city = null,
        string state = null,
        string zipCode = null,
        string country = null,
        string complement = null
       )
    {
        if (!string.IsNullOrWhiteSpace(zipCode) && !IsValidZipCode(zipCode))
            throw new DomainException("Invalid zip code format");

        Street = street;
        Number = number;
        Complement = complement;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country ?? "Brasil"; 
    }

    private bool IsValidZipCode(string zipCode)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(zipCode, @"^\d{8}$");
    }

}