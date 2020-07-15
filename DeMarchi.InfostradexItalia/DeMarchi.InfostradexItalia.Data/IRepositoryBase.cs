using System;
using System.Collections.Generic;
using System.Text;

namespace DeMarchi.InfostradexItalia.Data
{
    public interface IRepositoryBase<TModel> : IRepositoryBase<TModel, int>
    {

    }
    public interface IRepositoryBase<TModel, Tkey>
    {
        IEnumerable<TModel> GetAll();
        TModel GetById(Tkey id);
        void Insert(TModel value);
        void Update(TModel value);
        void Delete(Tkey id);
    }
}
