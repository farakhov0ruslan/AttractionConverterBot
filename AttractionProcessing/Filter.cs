namespace AttractionProcessing;

/// <summary>
/// A static class for filtering and extracting values from a list of attractions.
/// </summary>
public static class Filter
{
    /// <summary>
    /// Gets a list of unique values for a specified field from a list of attractions.
    /// </summary>
    /// <param name="attractions">The list of attractions.</param>
    /// <param name="field">The field to extract values from.</param>
    /// <returns>A list of unique values for the specified field.</returns>
    public static List<string> ValuesField(List<Attraction> attractions, string field)
    {
        SortedSet<string> valuesField = new();
        foreach (Attraction a in attractions)
        {
            valuesField.Add(a.GetPropertyValue(field));
        }

        return valuesField.ToList();
    }
}