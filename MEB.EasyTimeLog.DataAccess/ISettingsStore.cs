namespace MEB.EasyTimeLog.DataAccess
{
    public interface ISettingsStore<TE, in TK>
    {
        TE Get(TK key);
        void Save(TK key, TE value);
    }
}