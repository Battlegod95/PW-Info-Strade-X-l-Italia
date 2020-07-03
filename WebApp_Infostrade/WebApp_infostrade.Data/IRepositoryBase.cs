using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp_infostrade.Data
{
    public interface IRepositoryBase<TModel> : IRepositoryBase<TModel, int>
    {

    }
    public interface IRepositoryBase<TModel, Tkey>
    {
        IEnumerable<TModel> GetAll();
        TModel GetById(Tkey id);
        void Inser(TModel value);
        void Update(TModel value);
        void Delete(Tkey id);
    }
}
