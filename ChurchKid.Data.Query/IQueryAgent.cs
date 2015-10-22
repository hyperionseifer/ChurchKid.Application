using System.Collections.Generic;
using System.Data.Common;

namespace ChurchKid.Data.Query
{
    public interface IQueryAgent<T>
    {

        ICollection<T> Get(string filter);

        ICollection<T> Get(string filter, string sort);

        ICollection<T> Get(string filter, params DbParameter[] parameters);

        T Get(int id);

        ICollection<T> GetAll();

        ICollection<T> GetAll(string sort);


    }
}
