using System.Collections.Generic;

namespace MEB.EasyTimeLog.Model
{
    public interface IRepository<TE, in TK>
    {
        TE Get(TK key);
        IEnumerable<TE> GetAll();
        TE Save(TE entity);


        void LoadEntities();
        void SaveEntities();

        TE TranslateFromJson(string jsonText);
        string TranslateToJson(TE element);
    }
}