namespace MEB.EasyTimeLog.Model
{
    public interface IRepository<TE>
    {
        TE TranslateFromJson(string jsonText);

        string TranslateToJson(TE element);
    }
}