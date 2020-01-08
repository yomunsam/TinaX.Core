using CatLib.Container;

namespace TinaX
{
    public static class CatLibExtends
    {
        public static void SetAlias<Tlias>(this IBindData bindData)
        {
            bindData.Alias<Tlias>();
        }
    }
}
