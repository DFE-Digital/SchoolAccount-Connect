using Bogus;
using PlaywrightTests.GlobalConstants.Global;

namespace PlaywrightTests.DfE.UIMapping.Forms;

/// <summary>
/// Represents the data filled into the form on the resource journey pages.
/// </summary>
/// <remarks>We can use this class to generate an entire set of Resource Data.
/// ServiceId and ServiceName must be passed into the constructor.
/// Note there is a silent dependency here that the names of the properties exactly match those of the database table.
/// This is because, in order to keep things simple, we are currently not using any form of mapper.
/// </remarks>
public partial class ResourceFormData : FormDataBase
{
    private ResourceFormData() { }

    /// Generates random data for any empty fields
    public static ResourceFormData GenerateRandomData()
    {
        var rfData = new ResourceFormData();

        // Generate database-mapped fields

        rfData.ResourceName = $"Auto-Resource-{Guid.NewGuid().ToString()[..8]}";

        rfData.ResourceDescription = _faker.Lorem.Paragraph();

        rfData.DigitalLink = _faker.Internet.Url();

        rfData.ResourceTypeId = _faker.Random.Int(1, 2);

        // Set GuidanceTypeId based on ResourceTypeId logic:
        // - If ResourceTypeId is 1: GuidanceTypeId should be 1 or 2
        // - If ResourceTypeId is 2: GuidanceTypeId should always be null

        if (rfData.ResourceTypeId == 1)
        {
            rfData.GuidanceTypeId = _faker.Random.Int(1, 2);
        }
        else if (rfData.ResourceTypeId == 2)
        {
            rfData.GuidanceTypeId = null;
        }

        //Always Active I think? We can add more later
        rfData.ResourceStatusId = 2;

        rfData.GovUkLastUpdated = DateTime.Now.ToString("yyyy-MM-dd");

        rfData.CreatedBy = Global.TestEmailAddress;

        rfData.DateCreated = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");

        rfData.DateUpdated = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff");

        return rfData;
    }
}