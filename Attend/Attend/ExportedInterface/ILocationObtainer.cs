
namespace Attend.ExportedInterface
{
    public interface ILocationObtainer
    {
        void bindLocationService();

        void isServiceStarted();

        bool locationAvailablity(out double latitude, out double longitude);
    }

}
