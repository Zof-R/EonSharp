using System.Threading.Tasks;

namespace EonSharp.Api.Bot
{
	public interface IColoredCoin
	{
		Task<ColoredInfo> GetInfo(string id, int timestamp);
	}
}