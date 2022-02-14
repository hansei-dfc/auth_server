namespace war_game.Utility {
    public static class RandomUtility {
        public static readonly Random Random = new();

        public static ulong CreateId(Random? random = null) {
            var ran = random ?? Random;
        RET:
            var buf = new byte[sizeof(ulong)];
            ran.NextBytes(buf);
            var i = Convert.ToUInt64(buf);
            if (i == 0) goto RET;
            return i;
        }
    }
}
