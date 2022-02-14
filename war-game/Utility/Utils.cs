namespace war_game.Utility {
    public static class Utils {
        public static TRet Using<TRet, TRes>(this TRes res, Func<TRes, TRet> func) where TRes : IDisposable {
            using (var r = res) return func(r);
        }
    }
}
