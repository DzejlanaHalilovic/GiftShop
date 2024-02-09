using Micro.Sinhro.Gift.Contracts.Gift.Response;

namespace Micro.Sinhro.Gift.Interfaces
{
    public interface IGiftService
    {
        List<Models.Gift> GetGifts();
        Models.Gift GetGift(int id);
        CreateResponse CreateGift(Models.Gift request);
        UpdateResponse UpdateGift(Models.Gift request);
        DeleteResponse DeleteGift(int id);

    }
}
