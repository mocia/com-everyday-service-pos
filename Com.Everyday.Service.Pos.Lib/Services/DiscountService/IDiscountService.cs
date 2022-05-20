using Com.Everyday.Service.Pos.Lib.ViewModels.Discount;
using Com.Everyday.Service.Pos.Lib.Models.Discount;
using Com.Everyday.Service.Pos.Lib.ViewModels.Discount;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Everyday.Service.Pos.Lib.Services.DiscountService
{
    public interface IDiscountService
    {
        Tuple<List<Discount>, int, Dictionary<string, string>, List<string>> ReadModel(int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}", string Username = "");
        Task<int> Create(Discount model);
        Discount ReadModelById(int id);
        Discount MapToModel(DiscountViewModel viewModel);
        DiscountViewModel MapToViewModel(Discount model);
        Task<int> Update(int id, Discount model, string user, int clientTimeZoneOffset = 7);
        int Delete(int id);
        List<DiscountReadViewModel> GetDiscounts(string code, DateTime date);
    }
}
