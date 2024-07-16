using System.Text.Json.Serialization;

namespace AttractionProcessing;

/// <summary>
/// Represents an attraction.
/// </summary>
[Serializable]
public class Attraction : IComparable<Attraction>
{
    [JsonPropertyName("Name")] public string Name { get; set; } = string.Empty;

    [JsonPropertyName("Photo")] public string Photo { get; set; } = string.Empty;
    [JsonPropertyName("AdmArea")] public string AdmArea { get; set; } = string.Empty;

    [JsonPropertyName("District")] public string District { get; set; } = string.Empty;

    [JsonPropertyName("Location")] public string Location { get; set; } = string.Empty;

    [JsonPropertyName("RegistrationNumber")]
    public string RegistrationNumber { get; set; } = string.Empty;

    [JsonPropertyName("State")] public string State { get; set; } = string.Empty;

    [JsonPropertyName("LocationType")] public string LocationType { get; set; } = string.Empty;

    [JsonPropertyName("global_id")] public string GlobalId { get; set; } = string.Empty;

    [JsonPropertyName("geodata_center")] public string GeoDataCenter { get; set; } = string.Empty;

    [JsonPropertyName("geoarea")] public string GeoArea { get; set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="Attraction"/> class.
    /// </summary>
    public Attraction()
    {
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="Attraction"/> class.
    /// </summary>
    /// <param name="name">The name of the attraction.</param>
    /// <param name="photo">The URL of the attraction's photo.</param>
    /// <param name="admArea">The administrative area where the attraction is located.</param>
    /// <param name="district">The district where the attraction is located.</param>
    /// <param name="location">The location of the attraction.</param>
    /// <param name="registrationNumber">The registration number of the attraction.</param>
    /// <param name="state">The state of the attraction.</param>
    /// <param name="locationType">The location type of the attraction.</param>
    /// <param name="globalId">The global ID of the attraction.</param>
    /// <param name="geoDataCenter">The geodata center of the attraction.</param>
    /// <param name="geoArea">The geoarea of the attraction.</param>
    public Attraction(string name, string photo, string admArea, string district, string location,
        string registrationNumber, string state, string locationType, string globalId, string geoDataCenter,
        string geoArea)
    {
        Name = name;
        Photo = photo;
        AdmArea = admArea;
        District = district;
        Location = location;
        RegistrationNumber = registrationNumber;
        State = state;
        LocationType = locationType;
        GlobalId = globalId;
        GeoDataCenter = geoDataCenter;
        GeoArea = geoArea;
    }

    /// <summary>
    /// Compares the current object with another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared.
    /// Less than zero, this object is less than the <paramref name="other"/> object.
    /// Zero, this object is equal to <paramref name="other"/>.
    /// Greater than zero, this object is greater than <paramref name="other"/>.
    /// </returns>
    public int CompareTo(Attraction? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return string.Compare(Name, other.Name, StringComparison.Ordinal);
    }

    /// <summary>
    /// Compares the current object with another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <param name="reverse">A flag indicating whether the sort order should be reversed.</param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared.
    /// Less than zero, this object is less than the <paramref name="other"/> object.
    /// Zero, this object is equal to <paramref name="other"/>.
    /// Greater than zero, this object is greater than <paramref name="other"/>.
    /// </returns>
    public int CompareTo(Attraction? other, bool reverse)
    {
        int reverseInt = reverse ? -1 : 1;
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return reverseInt * 1;
        return reverseInt * string.Compare(Name, other.Name, StringComparison.Ordinal);
    }


    /// <summary>
    /// Gets the value of a property by its name.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>The value of the property.</returns>
    /// <exception cref="ArgumentException">Thrown when an incorrect property name is passed.</exception>
    public string GetPropertyValue(string propertyName)
    {
        string result = propertyName switch
        {
            "Name" => Name,
            "Photo" => Photo,
            "AdmArea" => AdmArea,
            "District" => District,
            "Location" => Location,
            "RegistrationNumber" => RegistrationNumber,
            "State" => State,
            "LocationType" => LocationType,
            "global_id" => GlobalId,
            "geodata_center" => GeoDataCenter,
            "geoarea" => GeoArea,
            _ => throw new ArgumentException("A non-correct class prperty has been passed")
        };
        return result;
    }
}