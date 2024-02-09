using Micro.Sinhro.Gift.Contracts.Gift.Response;
using Micro.Sinhro.Gift.Interfaces;
using Micro.Sinhro.Gift.Persistance;

namespace Micro.Sinhro.Gift.Services
{
    public class GiftService : IGiftService
    {

        public readonly GiftDbContex giftDbContex;
        public GiftService(GiftDbContex giftDbContex)
        {
            this.giftDbContex = giftDbContex;
        }

        public CreateResponse CreateGift(Models.Gift request)
        {

            giftDbContex.Gifts.Add(request);
            var result = giftDbContex.SaveChanges();
            if (result > 0)
            {
                return new CreateResponse { message = "Gift created successfully", success = true };
            }
            return new CreateResponse { message = "Gift could not be created", success = false };

        }

        public DeleteResponse DeleteGift(int id)
        {
            var carForDelete = giftDbContex.Gifts.FirstOrDefault(x => x.Id == id);
            if (carForDelete == null)
                return new DeleteResponse { message = "Gift does not exist", success = false };
            var result = giftDbContex.Remove(carForDelete);
            giftDbContex.SaveChanges();
            if (result != null)
                return new DeleteResponse { message = "Gift deleted successfully", success = true };
            return new DeleteResponse { message = "Gift could not be deleted", success = false };

        }

        public Models.Gift GetGift(int id)
        {
            return giftDbContex.Gifts.FirstOrDefault(x => x.Id == id);
        }

        public List<Models.Gift> GetGifts()
        {
            return giftDbContex.Gifts.ToList();
        }

        public UpdateResponse UpdateGift(Models.Gift request)
        {
            giftDbContex.Update(request);
            var result = giftDbContex.SaveChanges();
            if (result > 0)
            {
                return new UpdateResponse { message = "Gift updated successfully", success = true };
            }
            return new UpdateResponse { message = "Gift could not be updated", success = false };
        }

    }
}
