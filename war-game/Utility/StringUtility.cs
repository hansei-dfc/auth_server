namespace war_game.Utility {
    public static class StringUtility {
        public static bool IsNullOrWhiteSpace(this string? s) =>
            string.IsNullOrWhiteSpace(s);
    }
}
