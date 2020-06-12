using CatLib.Container;

namespace TinaX
{
    public static class CatLibExtends
    {
        public static IBindData SetAlias<TAlias>(this IBindData bindData)
        {
            bindData.Alias<TAlias>();
            return bindData;
        }
    }
}
