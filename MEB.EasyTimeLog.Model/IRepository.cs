using System;
using System.Collections.Generic;

namespace MEB.EasyTimeLog.Model
{
    public interface IRepository<TE, in TK>
    {
        TE Get(TK key);
        IList<TE> GetAll();
        IList<TE> GetAll(string sortType, string sortValue);
        TE Save(TE entity);
        void Delete(TK key);


        void LoadEntities();
        void SaveEntities();

        TE TranslateFromJson(string jsonText);
        string TranslateToJson(TE element);
    }
}