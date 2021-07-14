using System.Collections.Generic;

namespace WebStore.Interfaces.TestAPI
{
    public interface IValuesService
    {
        IEnumerable<string> GetAll();

        string GetByIndex(int index);

        void Add(string value);

        void Update(int index, string value);

        bool Delete(int index);
    }
}
