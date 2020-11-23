using Com.Bateeq.Service.Pos.Lib.Models.SalesDoc;
using Com.Bateeq.Service.Pos.Lib.Models.SalesReturn;
using Com.Bateeq.Service.Pos.Lib.ViewModels.SalesDoc;
using Com.Bateeq.Service.Pos.Lib.ViewModels.SalesDocReturn;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Pos.Lib.Services.SalesDocReturnService
{
    public interface ISalesDocReturnService
    {
        Tuple<List<SalesDocReturn>, int, Dictionary<string, string>, List<string>> ReadModel(string storecode, int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}", string Username = "");
        SalesDocReturn MapToModel(SalesDocReturnViewModel viewModel);
        SalesDocReturnViewModel MapToViewModel(SalesDocReturn model);
        Task<int> Create(SalesDocReturn model, SalesDocReturnViewModel viewModel);
        SalesDocReturn ReadModelById(int id);
        List<SalesDoc> SalesReturnReport(string storecode, DateTimeOffset dateFrom, DateTimeOffset dateTo, string shift);
        SalesDocViewModel MaptoViewModel(SalesDoc model);
    }
}
