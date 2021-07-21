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

        public static IBindData SetAlias(this IBindData bindData, string alias)
        {
            bindData.Alias(alias);
            return bindData;
        }
    }
}
