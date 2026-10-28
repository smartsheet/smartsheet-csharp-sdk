namespace Smartsheet.Api.Models
{
    /// <summary>
    /// Represents a single data classification label.
    /// </summary>
    public class ClassificationLabel
    {
        /// <summary>
        /// Gets or sets the label ID (UUID string).
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the display name of the label.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the label.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the hex color code for the label (e.g. "#FFE0E3").
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// Gets or sets the sensitivity order (lower = more sensitive).
        /// </summary>
        public int? SensitivityOrder { get; set; }

        /// <summary>
        /// Gets or sets whether this label is the default classification.
        /// </summary>
        public bool? IsDefault { get; set; }
    }
}
