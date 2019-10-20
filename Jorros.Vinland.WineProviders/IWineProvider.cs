using System.Threading.Tasks;

namespace Jorros.Vinland.WineProviders
{
    public interface IWineProvider
    {
        Task<OrderBoxResponse> OrderBoxAsync();
    }
}