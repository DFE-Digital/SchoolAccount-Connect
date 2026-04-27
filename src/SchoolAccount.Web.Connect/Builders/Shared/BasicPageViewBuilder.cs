using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Models.Shared;

namespace SchoolAccount.Web.Connect.Builders.Shared;

public class BasicPageViewBuilder (IOrganisationContext organisationContext)
{
    public BasicPageViewModel Build()
    {
        if (organisationContext.IsAuthenticated != true)
        {
            return new BasicPageViewModel();
        }
        
        return new BasicPageViewModel(organisationContext.Organisation.Name);
    }
}
