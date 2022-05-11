namespace Model
{
    public static class ModelEx
    {
        public static bool? IsNew(this BaseModel model)
        {
            if (model == null) return null;
            return model.Id > 0;
        }
    }
}
