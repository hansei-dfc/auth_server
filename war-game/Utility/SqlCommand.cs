using MySql.Data.MySqlClient;

namespace war_game.Utility {
    public struct SqlCommand {
        public SqlCommand(string? command, params object?[] objects) {
            Command = null; Whare = null; Parameters = new();
            Init(command, null, objects);
        }
        
        public SqlCommand(string? command, string? where, params object?[] objects) {
            Command = null; Whare = null; Parameters = new();
            Init(command, where, objects);
        }

        void Init(string? command, string? where, object?[] objects) {
            Command = command;
            Whare = where;
            Parameters = new();
            if (objects.Length <= 0) return;
            if (objects.Length % 2 != 0) goto THROW_FORMAT;

            Type string_type = typeof(string);

            for (int i = 0; i < objects.Length / 2; i++) {
                if (objects[i * 2]?.GetType() != string_type) goto THROW_FORMAT;
                Parameters.Add((string)objects[i * 2], objects[(i * 2) + 1]);
            } 
            return;

            THROW_FORMAT:
            throw new FormatException();
        }

        /// <summary>
        /// 커맨드
        /// </summary>
        public string? Command { get; set; }

        /// <summary>
        /// 어디
        /// </summary>
        public string? Whare { get; set; }

        /// <summary>
        /// 파라미터
        /// </summary>
        public Dictionary<string, object?> Parameters { get; set; }

        public string GetCommand() {
            var c = Command;
            if (!Whare.IsNullOrWhiteSpace()) c += " where " + Whare;
            return c;
        }

        public MySqlCommand Get(MySqlConnection connection = null) {
            var com = new MySqlCommand(GetCommand(), connection);
            if(Parameters?.Count >= 1)
                foreach (var item in Parameters) 
                    com.Parameters.AddWithValue(item.Key, item.Value);
            return com;
        }

        public static implicit operator MySqlCommand(SqlCommand command) => command.Get();
    }
}
