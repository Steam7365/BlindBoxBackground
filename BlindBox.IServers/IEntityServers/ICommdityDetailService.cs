using BlindBox.Models;
using BlindBox.Models.HelpModel;

namespace BlindBox.IServers
{
    public interface ICommdityDetailService: IBaseService<CommdityDetail>
    {
       public List<DescInfo> GetDescInfo(CommdityDetail commdityDetails);
    }
}