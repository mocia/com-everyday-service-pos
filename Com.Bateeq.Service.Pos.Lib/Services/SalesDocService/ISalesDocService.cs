using Com.Bateeq.Service.Pos.Lib.Models.SalesDoc;
using Com.Bateeq.Service.Pos.Lib.ViewModels.SalesDoc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Bateeq.Service.Pos.Lib.Services.SalesDocService
{
    public interface ISalesDocService
    {
        Tuple<List<SalesDoc>, int, Dictionary<string, string>, List<string>> ReadModel(string storecode, int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}", string Username = "");
        SalesDoc ReadModelById(int id);
        Task<SalesDoc> Create(SalesDoc model);
        SalesDocViewModel MaptoViewModel(SalesDoc model);
        SalesDoc MapToModel(SalesDocViewModel viewModel);
        Tuple<List<SalesDoc>, int, Dictionary<string, string>, List<string>> ReadModelVoid(string storecode, int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}", string Username = "");
        Task<int> Void(int id, string user, int clientTimeZoneOffset = 7);
        SalesDoc ReadModelByCode(string code, string storecode);
        Tuple<List<SalesDoc>, int, Dictionary<string, string>, List<string>> ReadModelReturn(string storecode, int Page = 1, int Size = 25, string Order = "{}", string Keyword = null, string Filter = "{}", string Username = "");
    }
}
