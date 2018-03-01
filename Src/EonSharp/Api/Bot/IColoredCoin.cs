using System.Threading.Tasks;

namespace EonSharp.Api.Bot
{
	public interface IColoredCoin
	{
		Task<ColoredCoinInfo> GetInfo(string id);
	}
}