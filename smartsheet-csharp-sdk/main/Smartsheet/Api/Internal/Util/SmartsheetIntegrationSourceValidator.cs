using System;
using System.Linq;
using Smartsheet.Api.Models;

namespace Smartsheet.Api.Internal.Util
{
    public class SmartsheetIntegrationSourceValidator
    {
        private static readonly string documentationLink = "https://developers.smartsheet.com/api/smartsheet/guides/basics/http-and-rest#http-headers";

        /// <summary>
        /// Validates a smartsheet integration source string in the format:
        ///   type, organisation name, integrator name
        ///
        /// - type: must be one of the enum values
        /// - organisation name: optional (can be empty)
        /// - integrator name: non-empty
        /// </summary>
        /// <param name="input">The input string to validate.</param>
        /// <returns>True if the format is valid.</returns>
        /// <exception cref="SmartsheetException">Thrown if the format is invalid.</exception>
        public static bool IsValidFormat(string input)
        {
            if (input == null)
            {
                throw new SmartsheetException("Smartsheet integration source cannot be null");
            }

            string[] parts = input.Split(new char[] { ',' }, StringSplitOptions.None);
            if (parts.Length != 3)
            {
                throw new SmartsheetException("Invalid smartsheet integration source format. " +
                        "Expected format: 'TYPE,ORGANIZATION,INTEGRATOR. " + documentationLink);
            }

            string integrationType = parts[0].Trim();
            string integratorName = parts[2].Trim();

            // The First slot (integration type) must match enum
            if (!IsValidType(integrationType))
            {
                throw new SmartsheetException("Invalid smartsheet integration source format. " +
                        "The integration type has to be one of the following: "
                        + string.Join(", ", Enum.GetNames(typeof(SmartsheetIntegrationSourceType)))
                        + ". Invalid integration type: " + integrationType + " " + documentationLink);
            }

            // Integrator name must be non-empty
            if (string.IsNullOrEmpty(integratorName))
            {
                throw new SmartsheetException("Invalid smartsheet integration source format. " +
                        "The integrator name cannot be empty.");
            }

            return true;
        }

        /// <summary>
        /// Checks if the integration type matches one of the enum values
        /// </summary>
        /// <param name="integrationTypeValue">The integration type value to check.</param>
        /// <returns>True if the type is valid.</returns>
        private static bool IsValidType(string integrationTypeValue)
        {
            if (string.IsNullOrEmpty(integrationTypeValue))
            {
                return false;
            }
            return Enum.GetNames(typeof(Models.SmartsheetIntegrationSourceType))
                         .Any(name => name.Equals(integrationTypeValue, StringComparison.OrdinalIgnoreCase));
        }
    }
}
