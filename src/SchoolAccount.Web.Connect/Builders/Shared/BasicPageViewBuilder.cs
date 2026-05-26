using SchoolAccount.Kernel;
using SchoolAccount.Web.Connect.Models.Shared;

namespace SchoolAccount.Web.Connect.Builders.Shared;

public class BasicPageViewBuilder(IOrganisationContext organisationContext)
{
    public async Task<BasicPageViewModel> Build()
    {
        if (!await organisationContext.IsAuthorised())
        {
            return new BasicPageViewModel();
        }

        return new BasicPageViewModel(organisationContext.Organisation.Name);
    }
}
