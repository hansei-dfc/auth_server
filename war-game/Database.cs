using MySql.Data.MySqlClient;
using war_game.Utility;

namespace war_game {
    public class Database {
        public string ConnectionString { get; set; }

        public Database(string connectionString) {
            ConnectionString = connectionString;
        }

        public MySqlConnection Sql {
            get {
                var sql = new MySqlConnection(ConnectionString);
                sql.Open();
                return sql;
            }
        }

        public TRet UseSql<TRet>(Func<MySqlConnection, TRet> func) {
            using (var r = Sql) return func(r);
        }

        public MySqlCommand Command(string? command, params object?[] objects) =>
            new SqlCommand(command, objects: objects).Get(Sql);

        public async Task<int> ExecuteNonQueryAsync(string? command, params object?[] objects) =>
            await UseSql(sql => new SqlCommand(command, objects: objects).Get(sql).ExecuteNonQueryAsync());
    }
}
